import os
import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery

SHEET_CONTENT_START = 2
WAIT_TIME = 3

class MonsterGen:
    """
    A class for syncronizing the strings in the project files
    with the google doc spreadsheets containing translations.
    """
    def open_sheet(self, credentials, id):
        http = credentials.authorize(httplib2.Http())
        discoveryUrl = ('https://sheets.googleapis.com/$discovery/rest?' 'version=v4')
        self._service = discovery.build('sheets', 'v4', http=http, discoveryServiceUrl=discoveryUrl)
        self._id = id

    def close_sheet(self):
        pass


    def write_data_text(self, txt_name, sheet_name):
        """
        Writes data from a txt to the sheet
        """
        # query sheet for existing rows
        header_row, sheet = self._query_sheet(sheet_name)
        # get rows from txt
        header, table = self._query_txt(os.path.join("..", "DataAsset", "Monster", txt_name+".txt"))
        # merge the data together, back into the google sheets
        self._merge_sheet_table(sheet_name, sheet, table)

    def load_sheet_text(self, txt_name, sheet_name):
        """
        Loads sheet text and writes it to txt file.
        """
        header_row, sheet = self._query_sheet(sheet_name)

        self._write_txt(os.path.join("..", "DataAsset", "Monster", txt_name + ".out.txt"), header_row, sheet)




    def _query_sheet(self, sheet_name):
        """
        Gets all data from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """
        check_range = sheet_name + "!A"+str(SHEET_CONTENT_START)+":A"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        return self._query_sheet_range(sheet_name, sheet_name + "!"+str(SHEET_CONTENT_START)+":"+str(SHEET_CONTENT_START+total_rows-1))

    def _query_sheet_range(self, sheet_name, range_name):
        """
        Gets all values from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """

        header_range = sheet_name + "!A"+str(SHEET_CONTENT_START-1)+":"+str(SHEET_CONTENT_START-1)
        header_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=header_range).execute()
        header_row = header_result.get('values', [[]])[0]

        result = self._service.spreadsheets().get(spreadsheetId=self._id, ranges=range_name, includeGridData=True).execute()
        time.sleep(WAIT_TIME)

        content_rows = []
        for row in result['sheets'][0]['data'][0]['rowData']:
            content_row = []
            for cell in row['values']:
                if 'userEnteredValue' in cell and 'stringValue' in cell['userEnteredValue']:
                    content_str = cell['userEnteredValue']['stringValue']
                elif 'userEnteredValue' in cell and 'boolValue' in cell['userEnteredValue']:
                    content_str = str(cell['userEnteredValue']['boolValue'])
                elif 'userEnteredValue' in cell and 'numberValue' in cell['userEnteredValue']:
                    content_str = str(cell['userEnteredValue']['numberValue'])
                else:
                    content_str = ''
                if '\n' in content_str:
                    print('Newline found in ' + sheet_name + ': ' + content_str)
                content_row.append(content_str)

            content_rows.append(content_row)


        return header_row, content_rows

    def _query_txt(self, txt_path):
        """
        Gets all values from a pre-formatted txt file, separated by header and values.
        """
        header = []
        output = []
        in_header = False
        with open(txt_path, encoding='utf-8') as txt:
            for line in txt:
                # remember to remove the newline
                cols = line[:-1].split('\t')
                if not in_header:
                    output.append(cols)
                else:
                    header = cols
                in_header = False
        return header, output


    def _merge_sheet_table(self, sheet_name, sheet, table):
        """
        Takes the local array of values,
        and writes directly to the remote google sheet.
        """

        # create a dictionary and list of the combined key/values
        cmb_keys = []
        # maps key to row
        cmb_dict = {}

        for row in sheet:
            fam_idx = int(row[0])
            species_idx = int(row[2])
            form_idx = int(row[4])
            key = (fam_idx, species_idx, form_idx)
            cmb_keys.append(key)
            cmb_dict[key] = [False, True, True, row]

        for row in table:
            fam_idx = int(row[0])
            species_idx = int(row[2])
            form_idx = int(row[4])
            key = (fam_idx, species_idx, form_idx)
            if key in cmb_dict:
                cmb_dict[key][0] = True
                existing = cmb_dict[key][3]
                diff = False
                for ii in range(len(row)):
                    if existing[ii].lower() != row[ii].lower():
                        diff = True
                        break
                cmb_dict[key][2] = diff
                cmb_dict[key][3] = row
            else:
                cmb_keys.append(key)
                cmb_dict[key] = [True, False, True, row]

        cmb_keys = sorted(cmb_keys)

        resp = self._service.spreadsheets().get(spreadsheetId=self._id, ranges=sheet_name).execute()

        if len(resp['sheets']) != 1:
            raise ValueError("Could not find unambiguous sheet " + sheet_name)
        else:
            sheet_id = resp['sheets'][0]['properties']['sheetId']

        sheet_ind = SHEET_CONTENT_START
        traversed = 0

        for cmb_key in cmb_keys:
            traversed = traversed+1
            if traversed % 100 == 0:
                print(str(traversed) + " merged")

            pair = cmb_dict[cmb_key]
            local_exist = pair[0]
            remote_exist = pair[1]
            changed = pair[2]
            row = pair[3]

            # fix the typings and formattings of cells
            row[0] = int(row[0])
            row[2] = int(row[2])
            row[4] = int(row[4])
            row[8] = row[8].lower() == "true"
            row[9] = row[9].lower() == "true"
            if local_exist != remote_exist:
                if local_exist:
                    # Entries missing in the local copy will be deleted directly from the google sheet.
                    body = { "deleteDimension": { "range": { "sheetId": sheet_id, "dimension": "ROWS", "startIndex": sheet_ind-1, "endIndex": sheet_ind } } }
                    self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': [body]}).execute()
                    print("    * Deleted key "+str(cmb_key))
                    time.sleep(WAIT_TIME)
                    sheet_ind -= 1
                elif remote_exist:
                    # Entries missing in the google sheet will be written directly to the google sheet.
                    moved_comment = ""

                    body = { "insertDimension": { "range": { "sheetId": sheet_id, "dimension": "ROWS", "startIndex": sheet_ind-1, "endIndex": sheet_ind } } }
                    self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': [body]}).execute()

                    requests = []
                    body = { 'values': [row] }

                    range_name = sheet_name + "!"+str(sheet_ind)+":"+str(sheet_ind)
                    self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()

                    print("    * Added " + str(cmb_key))
                    time.sleep(WAIT_TIME)
            elif changed:
                range_name = sheet_name + "!"+str(sheet_ind)+":"+str(sheet_ind)
                body = { 'values': [row] }
                self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()

                print("    * Changed " + str(cmb_key))
                time.sleep(1)

            sheet_ind += 1




    def _write_txt(self, txt_path, header_row, sheet):
        """
        Writes all translation entries to a pre-formatted txt file.
        """
        with open(txt_path, 'w', encoding='utf-8') as txt:
            txt.write('\t'.join(header_row)+"\n")
            for line in sheet:
                txt.write('\t'.join(line)+"\n")


import os
import shutil

import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery

SHEET_CONTENT_START = 2
WAIT_TIME = 3

class ItemGen:
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
        header, table = self._query_txt(os.path.join("..", "DataAsset", "Item", txt_name+".txt"))
        # merge the data together, back into the google sheets
        self._write_sheet_table(sheet_name, table)

    def load_sheet_text(self, txt_name, sheet_name):
        """
        Loads sheet text and writes it to txt file.
        """
        header_row, sheet = self._query_sheet(sheet_name)

        self._write_txt(os.path.join("..", "DataAsset", "Item", txt_name + ".out.txt"), header_row, sheet)


    def merge_data_text(self, txt_name, sheet_name):
        """
        Merges together the specified mons in the data sheet with the mons in the txt
        """
        # the google sheet needs family numbers (first dex in the group)
        # it also needs individual dex numbers
        header_row, sheet = self._query_sheet(sheet_name)
        # load data from translation table file
        tbl_path = os.path.join("..", "DataAsset", "Item", txt_name+".txt")
        _, table = self._query_txt(os.path.join("..", "DataAsset", "Item", txt_name+".txt"))
        prev_table = []
        prev_path = os.path.join("..", "DataAsset", "Item", txt_name+"_prev.txt")
        if os.path.exists(prev_path):
            _, prev_table = self._query_txt(prev_path)
        # merge the data together, back into the google sheets
        new_prev_table = self._merge_sheet_table(sheet_name, sheet, table, prev_table)

        self._write_txt(prev_path, header_row, new_prev_table)

    def _sheet_format(self, table):
        # fill in user, evo, index, rarity.
        # ON should be false by default.
        # Generic Name should be 0* None
        # Trade should be *
        # Effect should be 000: None

        new_table = []
        for row in table:
            trade = ""
            if row[2] == "TRUE":
                trade = "*"
            new_row = ["0* None", "", row[1], row[0], "FALSE", row[2], "", "", "",
                       "=IF(G{0} = "",IF(F{0},CONCAT(C{0},\"'s Family\"),C{0}),G{0})", row[3], trade, row[4],
                      "000: None","","","","","","","","","","","","","","","","","","",""]
            new_table.append(new_row)

        return new_table

    def _get_family_dict(self, sheet):

        new_dict = {}
        for row in sheet:
            family_id = int(row[3])
            if family_id not in new_dict:
                new_dict[family_id] = []
            new_dict[family_id].append(row)
        return new_dict

    def _get_family_diff(self, f1, f2):

        has_diff = False
        name_diff = []

        for ii in range(max(len(f1), len(f2))):
            if ii < len(f1) and ii < len(f2):
                if f1[ii] != f2[ii]:
                    has_diff = True
                name_diff.append((f1[ii], f2[ii]))
            elif ii < len(f1):
                has_diff = True
                name_diff.append((f1[ii], ""))
            else:
                has_diff = True
                name_diff.append(("", f2[ii]))

        if has_diff:
            return name_diff
        return None

    def _get_existing_families(self, remote_families, min_species):

        existing_families = []
        for species in remote_families:
            for row in remote_families[species]:
                if row[2] in min_species:
                    existing_families.append(species)
                    break

        return existing_families

    def _merge_sheet_table(self, sheet_name, sheet, table, prev_table):
        """
        Takes the array of exclusive items from google sheets,
        and merges it with the array of exclusive items from the local copy.
        It writes directly to the remote google sheet, and can only add/edit.  No deletes.
        """
        # create a dictionary that maps family dex number to group
        remote_families = self._get_family_dict(sheet)
        local_families = self._get_family_dict(self._sheet_format(table))
        prev_families = self._get_family_dict(prev_table)

        # only diffs will be processed
        changed_families = {}
        for species in local_families:
            if species not in prev_families:
                changed_families[species] = local_families[species]
                continue
            prev_col = [r[2] for r in prev_families[species]]
            local_col = [r[2] for r in local_families[species]]
            family_diff = self._get_family_diff(prev_col, local_col)
            if family_diff is not None:
                changed_families[species] = local_families[species]

        cmb_families = self._get_family_dict(sheet)
        # add the changed_families's values into cmb_families
        deferred_families = []
        for species in changed_families:
            local_col = [r[2] for r in changed_families[species]]
            if species not in cmb_families:
                # if the family is new, but some members are in an existing group, ask if it should be added
                min_species = []
                for family_species in local_col:
                    if family_species not in min_species:
                        min_species.append(family_species)
                existing_families = self._get_existing_families(remote_families, min_species)
                if len(existing_families) > 0:
                    family_str = ", ".join(min_species)
                    existing_str = ", ".join([str(a) for a in existing_families])
                    overwrite = input("New family {0} has {1}, which already exist in families {2}.\nDo you want to add it? y/n/(d)\n".format(species, family_str, existing_str))
                    if overwrite.lower() != 'y':
                        if overwrite.lower() != 'n':
                            deferred_families.append(species)
                        continue
                cmb_families[species] = changed_families[species]
                continue
            cur_col = [r[2] for r in cmb_families[species]]
            family_diff = self._get_family_diff(cur_col, local_col)
            if family_diff is not None:
                family_diff_rows = ["{0}\t->\t{1}".format(a[0], a[1]) for a in family_diff]
                family_diff_str = "\n".join(family_diff_rows)
                # if the user sequence is different, ask if the new one should overwrite the old one
                overwrite = input("Family {0} has changed:\n{1}\nAdopt these changes? y/n/(d)\n".format(species, family_diff_str))
                if overwrite.lower() != 'y':
                    if overwrite.lower() != 'n':
                        deferred_families.append(species)
                    continue
                for idx, row in enumerate(changed_families[species]):
                    trade = ""
                    if row[2] == "TRUE":
                        trade = "*"
                    if idx < len(cmb_families[species][idx]):
                        old_row = cmb_families[species][idx]
                        old_row[2] = row[2]
                        old_row[5] = row[5]
                        old_row[10] = row[10]
                        old_row[11] = trade
                        old_row[12] = row[12]
                    else:
                        cmb_families[species][idx].append(row)


        # afterwards, save the EvoTreeRef.txt as a _prev file,
        # so the next time you run this operation, only diffs will be processed
        new_prev = []
        for species in local_families:
            if species not in deferred_families:
                group = local_families[species]
                for row in group:
                    new_prev.append(row)


        # create a list of the combined keys
        cmb_keys = []
        for species in cmb_families:
            cmb_keys.append(species)

        # sort the keys properly
        cmb_keys = sorted(cmb_keys)

        return new_prev


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
        in_header = True
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


    def _write_sheet_table(self, sheet_name, table):
        """
        Takes the local array of values,
        and writes directly to the remote google sheet.
        """

        range_name = sheet_name + "!" + str(SHEET_CONTENT_START) + ":" + str(SHEET_CONTENT_START+len(table)-1)
        body = {'values': table}
        self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name,
                                                     valueInputOption="RAW", body=body).execute()




    def _write_txt(self, txt_path, header_row, sheet):
        """
        Writes all translation entries to a pre-formatted txt file.
        """
        with open(txt_path, 'w', encoding='utf-8') as txt:
            txt.write('\t'.join(header_row)+"\n")
            for line in sheet:
                txt.write('\t'.join(line)+"\n")


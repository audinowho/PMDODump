import os
import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery

SHEET_CONTENT_START = 4
KEY_INDEX = 0
COMMENT_INDEX = 1
WAIT_TIME = 3
DEFAULT_COLOR = {'red' : 1.0, 'green' : 1.0, 'blue' : 1.0 }

def create_color_req(sheet_id, row_range, col_range, color):
    return {
        "repeatCell": {
            "range": {
                "sheetId": sheet_id,
                "startRowIndex": row_range[0],
                "endRowIndex": row_range[1],
                "startColumnIndex": col_range[0],
                "endColumnIndex": col_range[1]
            },
            "cell": { "userEnteredFormat": { "backgroundColor": color } },
            "fields": "userEnteredFormat(backgroundColor)"
        }
    }

class Localization:
    """
    A class for syncronizing the strings in the project files
    with the google doc spreadsheets containing translations.
    """
    def open_sheet(self, credentials, id):
        http = credentials.authorize(httplib2.Http())
        discoveryUrl = ('https://sheets.googleapis.com/$discovery/rest?' 'version=v4')
        self._service = discovery.build('sheets', 'v4', http=http, discoveryServiceUrl=discoveryUrl)
        self._id = id
        self.changelogger = open("../DataAsset/String/changelog.txt", 'w', encoding='utf-8')

    def close_sheet(self):
        self.changelogger.close()

    def merge_main_text(self, sheet_name, resx_path, support):
        """
        Merges together the project's hardcoded strings (such as menu text) with the remote google sheet.
        Writes back to both of them.
        """
        # load data from google sheets
        header, colored_sheet = self._query_sheet_colored(sheet_name)
        # load data from resx file
        resource = self._query_resx(resx_path)
        # merge the data together, back into the google sheets
        self._merge_sheet_table(sheet_name, colored_sheet, resource)
        # read the google sheets again
        header, sheet = self._query_sheet(sheet_name)
        # update support
        self._update_support(header, sheet, support)
        # write the resx file
        self._write_resx(header, sheet, resx_path)

    def merge_script_text(self, sheet_name, folder, support):
        """
        Merges together the project's script strings (such as map-specific dialogue) with the remote google sheet.
        Writes back to both of them.
        """
        # load data from google sheets
        header, colored_sheet = self._query_sheet_colored(sheet_name)
        # load data from all findable translation resx
        # maps *sub* folder to list of translation rows
        resource_keys = {}
        self._recursive_find_strings(folder, "", resource_keys)
        resource = []
        # pool into one big resource, giving foldername prefixes
        for key in resource_keys:
            rows = resource_keys[key]
            for row in rows:
                string_key = key + "/" + row[0]
                resource.append([string_key]+row[1:])
        # merge the data together, back into the google sheets
        self._merge_sheet_table(sheet_name, colored_sheet, resource)
        # read the google sheets again
        header, sheet = self._query_sheet(sheet_name)
        # update support
        self._update_support(header, sheet, support)
        # remove prefixes and determine where the translations go
        sheet_dict = {}
        for row in sheet:
            key_split = row[KEY_INDEX].rsplit("/", 1)
            key_path = os.path.join(folder, key_split[0])
            if key_path not in sheet_dict:
                sheet_dict[key_path] = []
            sheet_dict[key_path].append([key_split[1]]+row[1:])

        # write each data to its own resx file
        for resx_path in sheet_dict:
            self._write_resx(header, sheet_dict[resx_path], os.path.join(resx_path, "strings"))

    def _recursive_find_strings(self, folder, sub_folder, resource_keys):
        """
        Recursively searches a folder for "strings.resx"
        """

        folder_list = os.listdir(os.path.join(folder, sub_folder))
        for sub in folder_list:
            path = os.path.join(folder, sub_folder, sub)
            if os.path.isdir(path):
                self._recursive_find_strings(folder, os.path.join(sub_folder, sub), resource_keys)
            elif path.endswith("strings.resx"):
                resx_path, ext = os.path.splitext(path)
                resource = self._query_resx(resx_path)
                resource_keys[sub_folder] = resource


    def merge_data_text(self, txt_name, sheet_name, type_name, support):
        """
        Merges together the project's data entry strings (such as item/move names) with the remote google sheet.
        Writes back to both of them.
        """
        # for data strings other than zones/maps/groundmaps, the localization file must be a separate C# file
        # first run the game in a setting that spits out all english values of the data type to a file
        # (will have to make a huge switch statement for this unfortunately)
        # the resulting file is sheet_name.txt, which this function will read in.
        # keys will be generated as num_name, num_desc, etc.
        # Take note that these keys will be parsed back into the type names, so they must use type names!
        # load data from google sheets
        header, colored_sheet = self._query_sheet_colored(sheet_name)
        # load data from translation table file
        table = self._query_txt(os.path.join("..", "DataAsset", "String", txt_name+".txt"))
        # merge the data together, back into the google sheets
        self._merge_sheet_table(sheet_name, colored_sheet, table)
        # read the google sheets again
        header, sheet = self._query_sheet(sheet_name)
        # write to output txt
        self._write_txt(os.path.join("..", "DataAsset", "String", txt_name + ".out.txt"), header, sheet)
        # update support
        self._update_support(header, sheet, support)


    def merge_enum_text(self, sheet_name, type_name, support):
        """
        Merges together the project's enum entry strings (specifically with exclusive items) with the remote google sheet.
        Writes back to both of them.
        """
        # for strings representing exclusive items, the localization file must be a separate C# file
        # first run the game in a setting that spits out all english values of the enum to a file
        # (will have to make a huge switch statement for this unfortunately)
        # the resulting file is type_name.txt, which this function will read in.
        # keys will be generated as the enum name.
        # load data from google sheets
        header, colored_sheet = self._query_sheet_colored(sheet_name)
        # load data from translation table file
        table = self._query_txt(os.path.join("..", "DataAsset", "String", type_name+".txt"))
        # merge the data together, back into the google sheets
        self._merge_sheet_table(sheet_name, colored_sheet, table)
        # read the google sheets again
        header, sheet = self._query_sheet(sheet_name)
        # write to output txt
        self._write_txt(os.path.join("..", "DataAsset", "String", type_name + ".out.txt"), header, sheet)
        # update support
        self._update_support(header, sheet, support)

    def load_special_text(self, sheet_name, support):
        """
        Merges together the specially treated hardcoded text with the remote google sheet.
        Writes back to both of them, and updates support to map the location codes to actual names.
        """
        header_row, sheet = self._query_sheet(sheet_name)

        for val_index in range(COMMENT_INDEX+1,len(header_row)):
            lang = sheet[0][val_index]
            val = header_row[val_index].lower()
            if lang != "" and val in support:
                support[val] = lang


        self._write_txt(os.path.join("..", "DataAsset", "String", "Special.out.txt"), header_row, sheet[1:8])

        self._write_txt(os.path.join("..", "DataAsset", "String", "Skin.out.txt"), header_row, sheet[8:10])

        self._write_txt(os.path.join("..", "DataAsset", "String", "Element.out.txt"), header_row, sheet[10:29])

        self._write_txt(os.path.join("..", "DataAsset", "String", "Rank.out.txt"), header_row, sheet[29:41])

        self._write_txt(os.path.join("..", "DataAsset", "String", "AI.out.txt"), header_row, sheet[41:])



    def load_support_text(self, file_name, support):
        """
        Reads a very specific value from the Special spreadsheet and writes it to hardcode.
        Along with writing the list of supported languages
        """
        with open(file_name, 'w', encoding='utf-8') as txt:
            txt.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + \
                      "<root>\n")

            for k in support:
                txt.write("  <data name=\""+k+"\" xml:space=\"preserve\">\n" + \
                    "    <value>"+support[k]+"</value>\n" + \
                    "    <comment></comment>  </data>\n")
            txt.write("</root>")


    def _query_sheet(self, sheet_name):
        """
        Gets all translations from a given google sheet, automatically figuring out height/width.
        Includes header row for language key.
        """
        check_range = sheet_name + "!A"+str(SHEET_CONTENT_START)+":A"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        return self._query_sheet_range(sheet_name, total_rows)

    def _query_sheet_colored(self, sheet_name):
        """
        Gets all translations from a given google sheet, automatically figuring out height/width.
        Includes header row for language key.
        """
        check_range = sheet_name + "!A"+str(SHEET_CONTENT_START)+":A"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        return self._query_sheet_range_colored(sheet_name, total_rows)


    def _query_sheet_range(self, sheet_name, total_rows):

        header_row, content_rows = self._query_sheet_range_colored(sheet_name, total_rows)

        uncolored_rows = []
        for idx in range(len(content_rows)):
            content_row = content_rows[idx]
            uncolored_row = []
            for idx2 in range(len(content_row)):
                cell = content_row[idx2]
                content_str = cell[0]
                color = cell[1]

                if color['red'] == 0.91764706 and color['green'] == 0.6 and color['blue'] == 0.6:
                    content_str = ''

                if '\n' in content_str:
                    print('Newline found in ' + sheet_name + ': ' + content_str)

                uncolored_row.append(content_str)
            uncolored_rows.append(uncolored_row)

        return header_row, uncolored_rows

    def _query_sheet_range_colored(self, sheet_name, total_rows):
        """
        Gets all translations from a given google sheet, automatically figuring out height/width.
        Includes header row for language key.
        """

        header_range = sheet_name + "!A"+str(SHEET_CONTENT_START-1)+":"+str(SHEET_CONTENT_START-1)
        header_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=header_range).execute()
        header_row = header_result.get('values', [[]])[0]

        result_rows = []

        for row_min in range(SHEET_CONTENT_START, SHEET_CONTENT_START+total_rows, 400):
            row_max = min(row_min + 400, SHEET_CONTENT_START+total_rows)
            range_name = sheet_name + "!"+str(row_min)+":"+str(row_max-1)
            result = self._service.spreadsheets().get(spreadsheetId=self._id, ranges=range_name, includeGridData=True).execute()
            result_rows = result_rows + result['sheets'][0]['data'][0]['rowData']
            time.sleep(WAIT_TIME)

        content_rows = []
        for row in result_rows:
            content_row = []
            for cell in row['values']:
                color = DEFAULT_COLOR
                if 'userEnteredFormat' in cell and 'backgroundColor' in cell['userEnteredFormat']:
                    color = cell['userEnteredFormat']['backgroundColor']

                if 'userEnteredValue' in cell and 'stringValue' in cell['userEnteredValue']:
                    content_str = cell['userEnteredValue']['stringValue']
                else:
                    content_str = ''

                content_row.append((content_str, color))

            content_rows.append(content_row)

        return header_row, content_rows

    def _query_resx(self, resx_path):
        """
        Gets all translation entries from a given resx file.  Only key, comments, and english.
        """
        keys = []
        resx_dict = {}
        resx_comment_dict = {}

        tree = ET.parse(resx_path + '.resx')
        root = tree.getroot()
        for data in root.iter('data'):
            key = data.get('name')
            keys.append(key)
            en_val = data.find('value').text
            if en_val is not None:
                resx_dict[key] = en_val
            else:
                resx_dict[key] = ""
            comment = data.find('comment')
            if comment is not None:
                resx_comment_dict[key] = comment.text
            else:
                resx_comment_dict[key] = ""

        for file in glob.glob(resx_path + ".*.resx"):
            tree = ET.parse(file)
            root = tree.getroot()
            for data in root.iter('data'):
                key = data.get('name')
                if key not in resx_dict:
                    keys.append(key)
                    resx_dict[key] = ""
                    resx_comment_dict[key] = ""

        keys = sorted(keys, key=str.lower)
        result = []
        for key in keys:
            result.append([key, resx_comment_dict[key], resx_dict[key]])
        return result

    def _query_txt(self, txt_path):
        """
        Gets all translation entries from a pre-formatted txt file.  Only key, comments, and english.
        """
        output = []
        header = True
        with open(txt_path, encoding='utf-8') as txt:
            for line in txt:
                # remember to remove the newline
                cols = line[:-1].split('\t')
                if not header:
                    output.append(cols[:3])
                header = False
        return output

    def _update_support(self, header_row, sheet, support):
        """
        Updates the dictionary of supported languages based on what is seen in the sheet.
        """
        for index in range(COMMENT_INDEX+1, len(header_row)):
            lang = header_row[index].lower()

            for row in sheet:
                if index >= len(row):
                    continue
                val = row[index]
                if val != "":
                    support[lang] = lang.upper()
                    break


    def _merge_sheet_table(self, sheet_name, colored_sheet, table):
        """
        Takes the array of localization strings from google sheets,
        and merges it with the array of localization strings from the local copy.
        It writes directly to the remote google sheet.
        """
        # create a dictionary and list of the combined key/values
        cmb_keys = []
        # maps key to english table translation, google sheet translation, and google sheet alt translations
        cmb_dict = {}

        cols = len(colored_sheet[0])

        self.changelogger.write("Sheet: " + sheet_name + "\n")
        # adds the sheet translations
        for row in colored_sheet:
            key = row[0][0]
            comment = row[1][0]
            en = row[2][0]
            cmb_keys.append(key)
            cmb_dict[key] = [None, en, comment, row[3:]]

        test_keys = sorted(cmb_keys, key=str.lower)
        for ii in range(len(test_keys)):
            if test_keys[ii] != cmb_keys[ii]:
                raise ValueError("Sheet " + sheet_name + " ordered incorrectly! At "+test_keys[ii] + " vs. " + cmb_keys[ii])

        # maps the key of a new table entry to the key of a list of missing table entries
        possible_renames = {}

        # adds the table translations
        for row in table:
            key = row[0]
            comment = row[1]
            en = row[2]
            if key in cmb_dict:
                cmb_dict[key][0] = en
            else:
                cmb_keys.append(key)
                cmb_dict[key] = [en, None, "", []]
            if en not in possible_renames:
                possible_renames[en] = [[], []]
            possible_renames[en][0].append(key)

        # find all old keys that have new keys' english strings
        for cmb_key in cmb_keys:
            remote_str = cmb_dict[cmb_key][1]
            if remote_str in possible_renames:
                possible_renames[remote_str][1].append(cmb_key)

        for changed_str in possible_renames:
            pair = possible_renames[changed_str]
            if len(pair[0]) == 1 and len(pair[1]) == 1 and pair[0][0] == pair[1][0]:
                pair[1] = []

        # sort the keys properly
        cmb_keys = sorted(cmb_keys, key=str.lower)

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
            local_str = pair[0]
            remote_str = pair[1]
            if local_str != remote_str:
                if local_str is None:
                    # Entries missing in the local copy will be deleted directly from the google sheet.
                    body = { "deleteDimension": { "range": { "sheetId": sheet_id, "dimension": "ROWS", "startIndex": sheet_ind-1, "endIndex": sheet_ind } } }
                    self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': [body]}).execute()
                    self.changelogger.write("    * Deleted key "+cmb_key+"\n")
                    time.sleep(WAIT_TIME)
                    sheet_ind -= 1
                elif remote_str is None:
                    # Entries missing in the google sheet will be written directly to the google sheet.
                    moved_comment = ""
                    moved_color_strings = []
                    rename_options = possible_renames[local_str][1]
                    if len(rename_options) > 0 and local_str != "":
                        # Give the option for the user to detect renames, if a deleted entry matches a newly added entry
                        print("Newly added local key "+cmb_key+" has an English value:\n"+local_str)
                        print("It may be renamed from sheet values:")
                        for ii in range(len(rename_options)):
                            print(str(ii)+": "+rename_options[ii])
                        overwrite = input("Enter the number it was renamed from to move over its translations, otherwise nothing will be moved.")
                        try:
                            chosen_move = int(overwrite)
                            chosen_key = rename_options[chosen_move]
                            moved_comment = cmb_dict[chosen_key][2]
                            moved_color_strings = cmb_dict[chosen_key][3]
                        except:
                            pass
                    body = { "insertDimension": { "range": { "sheetId": sheet_id, "dimension": "ROWS", "startIndex": sheet_ind-1, "endIndex": sheet_ind } } }
                    self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': [body]}).execute()

                    requests = []
                    moved_strings = []
                    for cell_ind, color_string in enumerate(moved_color_strings):
                        moved_strings.append(color_string[0])
                        requests.append(create_color_req(sheet_id, (sheet_ind-1, sheet_ind), (cell_ind+3, cell_ind+4), color_string[1]))
                    body = { 'values': [[cmb_key, moved_comment, local_str] + moved_strings] }

                    range_name = sheet_name + "!"+str(sheet_ind)+":"+str(sheet_ind)
                    self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()
                    # set the colors too
                    if len(requests) > 0:
                        self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': requests}).execute()

                    if len(moved_strings) == 0:
                        self.changelogger.write("    * Added " + cmb_key + "\n")
                    else:
                        self.changelogger.write("    * Added " + cmb_key + ", transferred from " + chosen_key + "\n")
                    time.sleep(WAIT_TIME)
                else:
                    if local_str == "":
                        # considered an erase
                        range_name = sheet_name + "!"+str(sheet_ind)+":"+str(sheet_ind)
                        body = { 'values': [[cmb_key,"",local_str] + ([""] * cols)] }
                        self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()
                        self.changelogger.write("    * Erased contents of " + cmb_key + "\n")
                        time.sleep(WAIT_TIME)
                    else:
                        # check against renames
                        renaming = False
                        moved_comment = ""
                        moved_color_strings = []
                        rename_options = possible_renames[local_str][1]
                        if len(rename_options) > 0:
                            renaming = True
                            # Give the option for the user to detect renames, if a deleted entry matches a newly added entry
                            print("Changed local key "+cmb_key+" has an English value:\n"+local_str)
                            print("It may be renamed from sheet values:")
                            for ii in range(len(rename_options)):
                                print(str(ii)+": "+rename_options[ii])
                            overwrite = input("Enter the number it was renamed from to move its translations, leave blank to simply overwrite.")
                            try:
                                chosen_move = int(overwrite)
                            except:
                                renaming = False

                            if renaming:
                                chosen_key = rename_options[chosen_move]
                                moved_comment = cmb_dict[chosen_key][2]
                                moved_color_strings = cmb_dict[chosen_key][3]

                        if renaming:
                            requests = []
                            moved_strings = []
                            for cell_ind, color_string in enumerate(moved_color_strings):
                                moved_strings.append(color_string[0])
                                requests.append(create_color_req(sheet_id, (sheet_ind-1, sheet_ind), (cell_ind+3, cell_ind+4), color_string[1]))

                            body = { 'values': [[cmb_key, moved_comment, local_str] + moved_strings + ([""] * cols)] }

                            range_name = sheet_name + "!"+str(sheet_ind)+":"+str(sheet_ind)
                            self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()
                            # set the colors too
                            if len(requests) > 0:
                                self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': requests}).execute()

                            self.changelogger.write("    * Changed " + chosen_key + " to "+ cmb_key+ ", retaining translations\n")
                        else:
                            # If english translation has changed in the table, update it and mark the other translations red.
                            # Give the option for the user to invalidate the other translations, or to just let it slide
                            overwrite = 'y'
                            if remote_str != "":
                                print("Sheet string is being overwritten by string:")
                                print("sheet: "+remote_str)
                                print("local: "+local_str)
                                overwrite = input("If this alters the meaning of the string, input \'y\' to mark current translations as invalid. Input \'e\' to erase the translations entirely.")

                            if overwrite.lower() == 'e':
                                range_name = sheet_name + "!"+str(sheet_ind)+":"+str(sheet_ind)
                                body = { 'values': [[cmb_key,"",local_str] + ([""] * cols)] }
                                self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()
                                self.changelogger.write("    * Erased contents of " + cmb_key +"\n")
                            else:
                                range_name = sheet_name + "!C"+str(sheet_ind)+":C"+str(sheet_ind)
                                body = { 'values': [[local_str]] }
                                self._service.spreadsheets().values().update(spreadsheetId=self._id, range=range_name, valueInputOption="RAW", body=body).execute()

                                if overwrite.lower() == 'y':
                                    requests = []
                                    for cell_ind in range(len(pair[3])):
                                        if pair[3][cell_ind][0] != "":
                                            requests.append(create_color_req(sheet_id, (sheet_ind-1, sheet_ind), (cell_ind+3, cell_ind+4),
                                                                             { "red": 234 / 255, "green": 153 / 255, "blue": 153 / 255 }))
                                    if len(requests) > 0:
                                        self._service.spreadsheets().batchUpdate(spreadsheetId=self._id, body={'requests': requests}).execute()
                                    self.changelogger.write("    * Significantly changed " + cmb_key +" from \""+ remote_str +"\" to \""+ local_str +"\"\n")
                                else:
                                    self.changelogger.write("    * Changed " + cmb_key +" from \"" + remote_str +"\" to \"" + local_str +"\"\n")

            sheet_ind += 1
        self.changelogger.write("\n")

    def _write_resx(self, header_row, sheet, resx_path):
        """
        Takes the array of localization strings from google sheets,
        and writes it to a .resx file
        """
        for index in range(COMMENT_INDEX+1, len(header_row)):
            lang = header_row[index].lower()

            file_name = resx_path + '.'
            if lang == "en":
                file_name = file_name + 'resx'
            else:
                file_name = file_name + lang + '.resx'
            with open(file_name, 'w', encoding='utf-8') as txt:
                txt.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + \
                    "<root>\n" + \
                    "  <!-- \n" + \
                    "    Microsoft ResX Schema \n" + \
                    "    \n" + \
                    "    Version 2.0\n" + \
                    "    \n" + \
                    "    The primary goals of this format is to allow a simple XML format \n" + \
                    "    that is mostly human readable. The generation and parsing of the \n" + \
                    "    various data types are done through the TypeConverter classes \n" + \
                    "    associated with the data types.\n" + \
                    "    \n" + \
                    "    Example:\n" + \
                    "    \n" + \
                    "    ... ado.net/XML headers & schema ...\n" + \
                    "    <resheader name=\"resmimetype\">text/microsoft-resx</resheader>\n" + \
                    "    <resheader name=\"version\">2.0</resheader>\n" + \
                    "    <resheader name=\"reader\">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>\n" + \
                    "    <resheader name=\"writer\">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>\n" + \
                    "    <data name=\"Name1\"><value>this is my long string</value><comment>this is a comment</comment></data>\n" + \
                    "    <data name=\"Color1\" type=\"System.Drawing.Color, System.Drawing\">Blue</data>\n" + \
                    "    <data name=\"Bitmap1\" mimetype=\"application/x-microsoft.net.object.binary.base64\">\n" + \
                    "        <value>[base64 mime encoded serialized .NET Framework object]</value>\n" + \
                    "    </data>\n" + \
                    "    <data name=\"Icon1\" type=\"System.Drawing.Icon, System.Drawing\" mimetype=\"application/x-microsoft.net.object.bytearray.base64\">\n" + \
                    "        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>\n" + \
                    "        <comment>This is a comment</comment>\n" + \
                    "    </data>\n" + \
                    "                \n" + \
                    "    There are any number of \"resheader\" rows that contain simple \n" + \
                    "    name/value pairs.\n" + \
                    "    \n" + \
                    "    Each data row contains a name, and value. The row also contains a \n" + \
                    "    type or mimetype. Type corresponds to a .NET class that support \n" + \
                    "    text/value conversion through the TypeConverter architecture. \n" + \
                    "    Classes that don\'t support this are serialized and stored with the \n" + \
                    "    mimetype set.\n" + \
                    "    \n" + \
                    "    The mimetype is used for serialized objects, and tells the \n" + \
                    "    ResXResourceReader how to depersist the object. This is currently not \n" + \
                    "    extensible. For a given mimetype the value must be set accordingly:\n" + \
                    "    \n" + \
                    "    Note - application/x-microsoft.net.object.binary.base64 is the format \n" + \
                    "    that the ResXResourceWriter will generate, however the reader can \n" + \
                    "    read any of the formats listed below.\n" + \
                    "    \n" + \
                    "    mimetype: application/x-microsoft.net.object.binary.base64\n" + \
                    "    value   : The object must be serialized with \n" + \
                    "            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter\n" + \
                    "            : and then encoded with base64 encoding.\n" + \
                    "    \n" + \
                    "    mimetype: application/x-microsoft.net.object.soap.base64\n" + \
                    "    value   : The object must be serialized with \n" + \
                    "            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter\n" + \
                    "            : and then encoded with base64 encoding.\n" + \
                    "\n" + \
                    "    mimetype: application/x-microsoft.net.object.bytearray.base64\n" + \
                    "    value   : The object must be serialized into a byte array \n" + \
                    "            : using a System.ComponentModel.TypeConverter\n" + \
                    "            : and then encoded with base64 encoding.\n" + \
                    "    -->\n" + \
                    "  <xsd:schema id=\"root\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">\n" + \
                    "    <xsd:import namespace=\"http://www.w3.org/XML/1998/namespace\" />\n" + \
                    "    <xsd:element name=\"root\" msdata:IsDataSet=\"true\">\n" + \
                    "      <xsd:complexType>\n" + \
                    "        <xsd:choice maxOccurs=\"unbounded\">\n" + \
                    "          <xsd:element name=\"metadata\">\n" + \
                    "            <xsd:complexType>\n" + \
                    "              <xsd:sequence>\n" + \
                    "                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" />\n" + \
                    "              </xsd:sequence>\n" + \
                    "              <xsd:attribute name=\"name\" use=\"required\" type=\"xsd:string\" />\n" + \
                    "              <xsd:attribute name=\"type\" type=\"xsd:string\" />\n" + \
                    "              <xsd:attribute name=\"mimetype\" type=\"xsd:string\" />\n" + \
                    "              <xsd:attribute ref=\"xml:space\" />\n" + \
                    "            </xsd:complexType>\n" + \
                    "          </xsd:element>\n" + \
                    "          <xsd:element name=\"assembly\">\n" + \
                    "            <xsd:complexType>\n" + \
                    "              <xsd:attribute name=\"alias\" type=\"xsd:string\" />\n" + \
                    "              <xsd:attribute name=\"name\" type=\"xsd:string\" />\n" + \
                    "            </xsd:complexType>\n" + \
                    "          </xsd:element>\n" + \
                    "          <xsd:element name=\"data\">\n" + \
                    "            <xsd:complexType>\n" + \
                    "              <xsd:sequence>\n" + \
                    "                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />\n" + \
                    "                <xsd:element name=\"comment\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"2\" />\n" + \
                    "              </xsd:sequence>\n" + \
                    "              <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" msdata:Ordinal=\"1\" />\n" + \
                    "              <xsd:attribute name=\"type\" type=\"xsd:string\" msdata:Ordinal=\"3\" />\n" + \
                    "              <xsd:attribute name=\"mimetype\" type=\"xsd:string\" msdata:Ordinal=\"4\" />\n" + \
                    "              <xsd:attribute ref=\"xml:space\" />\n" + \
                    "            </xsd:complexType>\n" + \
                    "          </xsd:element>\n" + \
                    "          <xsd:element name=\"resheader\">\n" + \
                    "            <xsd:complexType>\n" + \
                    "              <xsd:sequence>\n" + \
                    "                <xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />\n" + \
                    "              </xsd:sequence>\n" + \
                    "              <xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" />\n" + \
                    "            </xsd:complexType>\n" + \
                    "          </xsd:element>\n" + \
                    "        </xsd:choice>\n" + \
                    "      </xsd:complexType>\n" + \
                    "    </xsd:element>\n" + \
                    "  </xsd:schema>\n" + \
                    "  <resheader name=\"resmimetype\">\n" + \
                    "    <value>text/microsoft-resx</value>\n" + \
                    "  </resheader>\n" + \
                    "  <resheader name=\"version\">\n" + \
                    "    <value>2.0</value>\n" + \
                    "  </resheader>\n" + \
                    "  <resheader name=\"reader\">\n" + \
                    "    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>\n" + \
                    "  </resheader>\n" + \
                    "  <resheader name=\"writer\">\n" + \
                    "    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>\n" + \
                    "  </resheader>\n")

                for row in sheet:
                    if index >= len(row):
                        continue
                    key = row[KEY_INDEX]
                    comment = row[COMMENT_INDEX]
                    val = row[index]
                    if val != "" or lang == "en":
                        txt.write("  <data name=\""+key+"\" xml:space=\"preserve\">\n")
                        txt.write("    <value>"+val+"</value>\n")
                        txt.write("    <comment>"+comment+"</comment>")
                        txt.write("  </data>\n")
                txt.write("</root>")


    def _write_txt(self, txt_path, header_row, sheet):
        """
        Writes all translation entries to a pre-formatted txt file.
        """
        with open(txt_path, 'w', encoding='utf-8') as txt:
            txt.write('\t'.join(header_row)+"\n")
            for line in sheet:
                txt.write('\t'.join(line)+"\n")


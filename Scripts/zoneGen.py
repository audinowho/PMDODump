import os
import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery

SHEET_CONTENT_START = 2
WAIT_TIME = 3

class ZoneGen:
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
        header, table = self._query_txt(os.path.join("..", "DataAsset", "Zone", txt_name+".txt"))
        # merge the data together, back into the google sheets
        self._write_sheet_table(sheet_name, table)

    def load_sheet_texts(self):
        """
        Loads sheet text and writes it to txt file.
        """
        sheet_metadata = self._service.spreadsheets().get(spreadsheetId=self._id).execute()
        sheets = sheet_metadata.get('sheets', '')
        for idx, sheet in enumerate(sheets):
            if idx <= 1:
                continue
            sheet_name = sheet['properties']['title']

            item_values = self._query_items(sheet_name)
            mon_values = self._query_mons(sheet_name)

            with open(os.path.join("..", "DataAsset", "Zone", sheet_name.replace(' ', '_') + ".out.txt"), 'w', encoding='utf-8') as txt:
                for item in item_values:
                    txt.write(item + "\n")
                txt.write("\n\n")
                for mon in mon_values:
                    txt.write(mon + "\n")

    def parseRange(self, startStr, endStr):
        start = '0'
        if startStr != '-':
            start = str(int(startStr)-1)
        end = 'max_floors'
        if endStr != '-':
            end = endStr

        return 'new IntRange({0}, {1})'.format(start, end)

    def _query_items(self, sheet_name):
        """
        Gets all data from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """
        check_range = sheet_name + "!B"+str(SHEET_CONTENT_START)+":B"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        _, items = self._query_sheet_range(sheet_name, sheet_name + "!A"+str(SHEET_CONTENT_START)+":J"+str(SHEET_CONTENT_START+total_rows-1))

        commands = []
        commands.append('//items')
        commands.append('ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();')
        commands.append('itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;')
        commands.append('\n')
        category = ''
        for row in items:
            if row[0] != "":
                category = row[0]
                commands.append('//{0}'.format(category))
                commands.append('CategorySpawn<InvItem> {0} = new CategorySpawn<InvItem>();'.format(category))
                commands.append('{0}.SpawnRates.SetRange({1}, {2});'.format(category, row[3],
                                                                            self.parseRange(row[1], row[2])))
                commands.append('itemSpawnZoneStep.Spawns.Add("{0}", {0});'.format(category))
                commands.append('\n')

            num, name = row[4].split(': ')
            val = ''
            if row[6] != '0':
                val = ', {0}'.format(row[6])
            frange = self.parseRange(row[7], row[8])
            rate = int(row[9])
            if num != '----':
                if row[5] == '0':
                    if val != '':
                        val = ', false{0}'.format(val)
                    commands.append('{0}.Spawns.Add(new InvItem({1}{2}), {3}, {4});//{5}'.format(
                            category, num, val, frange, rate, name))
                elif row[5] == '100':
                    commands.append('{0}.Spawns.Add(new InvItem({1}, true{2}), {3}, {4});//{5}'.format(
                        category, num, val, frange, rate, name))
                else:
                    chance = int(row[5])
                    sticky = chance * rate / 100
                    nonstick = rate - sticky
                    commands.append('{0}.Spawns.Add(new InvItem({1}, true{2}), {3}, {4});//{5}'.format(
                        category, num, val, frange, round(sticky), name))
                    if val != '':
                        val = ', false{0}'.format(val)
                    commands.append('{0}.Spawns.Add(new InvItem({1}{2}), {3}, {4});//{5}'.format(
                            category, num, val, frange, round(nonstick), name))
        commands.append('\n')
        commands.append('floorSegment.ZoneSteps.Add(itemSpawnZoneStep);')

        return commands

    def _query_mons(self, sheet_name):
        """
        Gets all data from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """
        check_range = sheet_name + "!L" + str(SHEET_CONTENT_START) + ":L"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        _, mons = self._query_sheet_range(sheet_name, sheet_name + "!L" + str(SHEET_CONTENT_START) + ":Y" + str(
            SHEET_CONTENT_START + total_rows - 1))

        commands = []
        commands.append('//mobs')
        commands.append('TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();')
        commands.append('poolSpawn.Priority = PR_RESPAWN_MOB;')
        commands.append('\n')

        for row in mons:
            num, name = row[0].split(': ')
            frange = self.parseRange(row[9], row[10])
            if num != '---':
                attr_nums = ['-1', '-1', '-1', '-1', '-1']
                attributes = ''
                for idx in range(2, 7):
                    inum, iname = row[idx].split(': ')
                    if inum != '---':
                        attributes += ' : {0} {1}'.format(inum, iname)
                        attr_nums[idx - 2] = inum

                level = "new RandRange({0})".format(row[7])
                anum, aname = row[8].split(': ')
                if row[12] != '---':
                    level += ", TeamMemberSpawn.MemberRole." + row[12]
                if anum != '--':
                    level += ", {0}".format(anum)
                commands.append('//{0} {1}{2}'.format(num, name, attributes))
                # add coment if exists
                if len(row) > 13 and row[13] != "":
                   commands.append('//{0}'.format(row[13]))
                rate = row[11]
                mon_id = num
                form = int(row[1])
                if form > 0:
                    mon_id = 'new MonsterID({0}, {1}, -1, Gender.Unknown)'.format(num, form)
                commands.append('poolSpawn.Spawns.Add(GetTeamMob({0}, {1}, {2}), {3}, {4});'.format(
                    mon_id, ", ".join(attr_nums), level, frange, rate))

        return commands

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


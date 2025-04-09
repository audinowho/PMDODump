import os
import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery
from sheetMerge import SheetMerge

SHEET_CONTENT_START = 2

class ZoneGen(SheetMerge):
    """
    A class for syncronizing the strings in the project files
    with the google doc spreadsheets containing translations.
    """
    def __init__(self, credentials, id):
        super().__init__(credentials, id, SHEET_CONTENT_START)



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
            print("Reading " + sheet_name)

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
        if startStr != '-' and startStr != "":
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

            num = row[4]
            val = ''
            if row[6] != '0' and row[6] != '':
                val = ', {0}'.format(row[6])
            frange = self.parseRange(row[7], row[8])
            rate = int(row[9])
            if num != '----' and num != "":
                if row[5] == '0':
                    if val != '':
                        val = ', false{0}'.format(val)
                    commands.append('{0}.Spawns.Add(new InvItem("{1}"{2}), {3}, {4});'.format(
                            category, num, val, frange, rate))
                elif row[5] == '100':
                    commands.append('{0}.Spawns.Add(new InvItem("{1}", true{2}), {3}, {4});'.format(
                        category, num, val, frange, rate))
                else:
                    chance = int(row[5])
                    sticky = chance * rate / 100
                    nonstick = rate - sticky
                    commands.append('{0}.Spawns.Add(new InvItem("{1}", true{2}), {3}, {4});'.format(
                        category, num, val, frange, round(sticky)))
                    if val != '':
                        val = ', false{0}'.format(val)
                    commands.append('{0}.Spawns.Add(new InvItem("{1}"{2}), {3}, {4});'.format(
                            category, num, val, frange, round(nonstick)))
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
            num = row[0]
            frange = self.parseRange(row[9], row[10])
            if num != '---' and num != "":
                attr_nums = ['""', '""', '""', '""', '""']
                for idx in range(2, 7):
                    inum = row[idx]
                    if inum != '---':
                        attr_nums[idx - 2] = '"{0}"'.format(inum)

                level = "new RandRange({0})".format(row[7])
                anum = row[8]
                if row[12] != '---' and row[12] != "":
                    level += ", TeamMemberSpawn.MemberRole." + row[12]
                if anum != '--' and anum != "":
                    level += ', "{0}"'.format(anum)

                # add comment if exists
                if len(row) > 13 and row[13] != "":
                   commands.append('//{0}'.format(row[13]))
                rate = row[11]
                mon_id = '"{0}"'.format(num)
                form = int(row[1])
                if form > 0:
                    mon_id = 'new MonsterID("{0}", {1}, "", Gender.Unknown)'.format(num, form)
                commands.append('poolSpawn.Spawns.Add(GetTeamMob({0}, {1}, {2}), {3}, {4});'.format(
                    mon_id, ", ".join(attr_nums), level, frange, rate))

        return commands



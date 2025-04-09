import os
import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery
from sheetMerge import SheetMerge

SHEET_CONTENT_START = 5
SPECIES_INDEX = 1
FORM_INDEX = 2
GENDER_INDEX = 3
TALK_INDEX = 4

class TalkGen(SheetMerge):
    """
    A class for syncronizing the strings in the project files
    with the google doc spreadsheets containing translations.
    """
    def __init__(self, credentials, id):
        super().__init__(credentials, id, SHEET_CONTENT_START)


    def merge_talk_text(self, sheet_name, resx_path):
        """
        Merges together the project's hardcoded strings (such as menu text) with the remote google sheet.
        Writes back to both of them.
        """
        # load data from google sheets
        _, sheet = self._query_sheet(sheet_name)
        # load data from resx file
        resource = self._query_resx(resx_path)
        # merge the data together, back into the google sheets
        out_resource = self._merge_sheet_resource(sheet, resource)
        # write the resx file
        self._write_resx(out_resource, resx_path)


    def _merge_sheet_resource(self, sheet, resource):
        """
        Takes the array of talk strings from google sheets,
        and merges it with the array of strings from the local copy.
        Returns an output array
        """
        # create a dictionary and list of the combined key/values
        cmb_keys = []
        cmb_dict = {}
        cmb_comment_dict = {}

        for row in resource:
            cmb_dict[row[0]] = row[2]
            comment = row[1]
            if comment is None:
                comment = ""
            cmb_comment_dict[row[0]] = comment

        for row in sheet:
            species = row[SPECIES_INDEX]
            form = row[FORM_INDEX]
            gender = row[GENDER_INDEX]
            result_key = "TALK_REST_" + "{:04d}".format(int(species))
            if form != "":
                result_key = result_key + "_{:02d}".format(int(form))
            if gender != "":
                result_key = result_key + "_{:01d}".format(int(gender))
            result_key = result_key + "_VAR_00"

            cmb_dict[result_key] = row[TALK_INDEX]

            if result_key not in cmb_comment_dict:
                cmb_comment_dict[result_key] = ""

        keys = []
        for k in cmb_dict:
            keys.append(k)

        keys = sorted(keys, key=str.lower)

        for key in keys:
            cmb_keys.append([key, cmb_comment_dict[key], cmb_dict[key]])

        return cmb_keys




import os
import httplib2

from apiclient import discovery
from oauth2client import client
from oauth2client import tools
from oauth2client.file import Storage
from localization import Localization
from subprocess import call

import argparse
flags = argparse.ArgumentParser(parents=[tools.argparser]).parse_args()

# If modifying these scopes, delete your previously saved credentials
# at ~/.credentials/sheets.googleapis.com-python-quickstart.json
SCOPES = 'https://www.googleapis.com/auth/spreadsheets'
CLIENT_SECRET_FILE = 'client_secret.json'
APPLICATION_NAME = 'Localization Sync'
SHEET_ID_FILE = 'string_sheet_id.txt'

def get_credentials():
    """Gets valid user credentials from storage.

    If nothing has been stored, or if the stored credentials are invalid,
    the OAuth2 flow is completed to obtain the new credentials.

    Returns:
        Credentials, the obtained credential.
    """
    home_dir = ''
    credential_dir = os.path.join(home_dir, '.credentials')
    if not os.path.exists(credential_dir):
        os.makedirs(credential_dir)
    credential_path = os.path.join(credential_dir, 'sheets.googleapis.com-localization.json')

    store = Storage(credential_path)
    credentials = store.get()
    if not credentials or credentials.invalid:
        flow = client.flow_from_clientsecrets(CLIENT_SECRET_FILE, SCOPES)
        flow.user_agent = APPLICATION_NAME
        credentials = tools.run_flow(flow, store, flags)
        print('Storing credentials to ' + credential_path)
    return credentials

def get_sheet_id():
    """
    Gets the ID of the google spreadsheet to access.
    """
    with open(SHEET_ID_FILE) as file:
        return file.readline().strip()

def main():
    """
    Synchronizes all strings in the entire project.  Hardcode, Script, and Data.
    This code makes some hardcoded assumptions about the rest of the project file structure:
    -The location of the hardcoded string table
    -The location of the script directory
    -The existence of various forms of content
    -The existence of a build in the build folder (thus, the build step will actually have to be executed twice in a full deploy).
    """
    credentials = get_credentials()
    sheet_id = get_sheet_id()

    localize = Localization()
    localize.open_sheet(credentials, sheet_id)
    print("Sheet opened.")

    # Data
    # first run the game in a setting that spits out all english values of the data type to their own files
    # call("../PMDO/PMDO.exe -strings", shell=True)

    support = { "en": "EN" }

    # Hardcode
    localize.merge_main_text("Hardcode",os.path.join("..","DumpAsset","Strings","strings"),support)
    print("Hardcode merged.")
    localize.merge_main_text("Content",os.path.join("..","DumpAsset","Strings","stringsEx"),support)
    print("Content merged.")

    # Script
    localize.merge_script_text("Script",os.path.join("..","DumpAsset","Data","Script", "origin"),support)
    print("Scripts merged.")

    # functions.append(localize.merge_data_text("Dex","MonsterData",support))
    localize.merge_data_text("Intrinsic","Abilities","IntrinsicData",support)
    print("Abilities merged.")
    localize.merge_data_text("Item","Items","ItemData",support)
    print("Items merged.")
    localize.merge_data_text("MapStatus","MapStatuses","MapStatusData",support)
    print("MapStatuses merged.")
    localize.merge_data_text("Skill","Moves","SkillData",support)
    print("Moves merged.")
    localize.merge_data_text("Status","Statuses","StatusData",support)
    print("Statuses merged.")
    localize.merge_data_text("Tile","Tiles","TileData",support)
    print("Tiles merged.")

    localize.merge_enum_text("Excl Names","ExclusiveItemType",support)
    print("Exclusive Names merged.")

    localize.merge_enum_text("Excl Effects","ExclusiveItemEffect",support)
    print("Exclusive Effects merged.")

    localize.merge_data_text("Zone","Zones","ZoneData",support)
    print("Zones merged.")
    localize.merge_data_text("Map","Maps","Map",support)
    print("Maps merged.")
    localize.merge_data_text("Ground","GroundMaps","GroundMap",support)
    print("GroundMaps merged.")
    localize.load_special_text("Special",support)
    print("Special merged.")

    localize.load_support_text(os.path.join("..", "DumpAsset", "Strings", "Languages.xml"), support)

    print("Complete.")

if __name__ == '__main__':
    main()
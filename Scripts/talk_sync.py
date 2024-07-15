import os
import httplib2

from apiclient import discovery
from oauth2client import client
from oauth2client import tools
from oauth2client.file import Storage
from talkGen import TalkGen
from subprocess import call

import argparse
flags = argparse.ArgumentParser(parents=[tools.argparser]).parse_args()

# If modifying these scopes, delete your previously saved credentials
# at ~/.credentials/sheets.googleapis.com-python-quickstart.json
SCOPES = 'https://www.googleapis.com/auth/spreadsheets'
CLIENT_SECRET_FILE = 'client_secret.json'
APPLICATION_NAME = 'Localization Sync'
SHEET_ID_FILE = 'talk_sheet_id.txt'

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

    talk = TalkGen()
    talk.open_sheet(credentials, sheet_id)
    print("Sheet opened.")

    # Merge the talkstrings in StringsEx
    talk.merge_talk_text("Species",os.path.join("..","DumpAsset","Strings","stringsEx"))
    print("Complete.")

if __name__ == '__main__':
    main()
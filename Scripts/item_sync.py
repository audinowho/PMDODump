import os
import httplib2

from apiclient import discovery
from oauth2client import client
from oauth2client import tools
from oauth2client.file import Storage
from itemGen import ItemGen
from subprocess import call

import argparse
flags = argparse.ArgumentParser(parents=[tools.argparser]).parse_args()

# If modifying these scopes, delete your previously saved credentials
# at ~/.credentials/sheets.googleapis.com-python-quickstart.json
SCOPES = 'https://www.googleapis.com/auth/spreadsheets'
CLIENT_SECRET_FILE = 'client_secret.json'
APPLICATION_NAME = 'Item Sync'
SHEET_ID_FILE = 'item_sheet_id.txt'

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
    Synchronizes exclusive items for the project.
    """
    credentials = get_credentials()
    sheet_id = get_sheet_id()

    itemGen = ItemGen()
    itemGen.open_sheet(credentials, sheet_id)
    print("Sheet opened.")

    # Reference
    # first run the game in a setting that spits out all item types to their own files
    # call("../PMDO/PMDO.exe -itemprep", shell=True)


    # Reference
    itemGen.write_data_text("PreAutoItem", "Resources")
    itemGen.write_data_text("AutoItemRef", "Reference")

    print("Updated References.")

    # Item Data
    itemGen.load_sheet_text("ExclusiveItem", "Content")

    print("Pulled Item Data.")

    print("Complete.")

if __name__ == '__main__':
    main()
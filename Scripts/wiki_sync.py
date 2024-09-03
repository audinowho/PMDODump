import os
import httplib2

from apiclient import discovery
from oauth2client import client
from oauth2client import tools
from oauth2client.file import Storage

import requests
from subprocess import call

import argparse
flags = argparse.ArgumentParser(parents=[tools.argparser]).parse_args()

CLIENT_SECRET_FILE = 'client_secret.json'
APPLICATION_NAME = 'Wiki Sync'

RETRIEVE_HEADERS = {
    'User-Agent': 'MediaWiki REST API docs examples/0.1 (https://www.mediawiki.org/wiki/API_talk:REST_API)'
}

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

def main():
    """
    Synchronizes data with the wiki
    """
    #credentials = get_credentials()

    url = 'https://wiki.pmdo.pmdcollab.org/w/rest.php/v1/page/Critical_Hit/bare'
    response = requests.get(url, headers=RETRIEVE_HEADERS)
    data = response.json()

    print(data)


    print("Complete.")

if __name__ == '__main__':
    main()
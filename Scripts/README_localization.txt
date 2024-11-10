Introduction:

Localization is done by gathering all of the strings in the Project, and putting them onto a google Spreadsheet.
The Spreadsheet is then updated by the translators, and those tables are copied back into the Project.
There are many places where strings need to be copied to and from.  run_sync.py is a script that is created to do all of that automatically.

How to use:

In order to run this script, you will need to follow the Prerequisites and Step 1 of Google Sheets API setup:
https://developers.google.com/sheets/api/quickstart/python

Copy the client_secret.json to the working directory.

You must then get the ID of the google Spreadsheet (found in tis URL) containing the translation data and save it to a file named sheet_id.txt, also in the working directory.

Before you run the script, you must write out all of the move, item, ability etc. names into a table.  You can achieve this by running the main application with "-strings" as an argument.
They will appear in the /temp/ folder, one directory level above this script.

All of the files we've generated so far are in the .ignore file so don't worry about them showing up in source control.

Finally, you can run the script string_sync.py
It will read the state of the google sheets, and the state of the strings in the Project, merging them together and writing back to both.
This means that both the Project strings and the strings in the google doc are affected in one step.


The auto-merge code in string_sync uses the following rules for merging:
* If there's a new string key that was added to the Project, it will be added to the Spreadsheet.
* If there's a string key missing in the Project that exists in the Spreadsheet, it will be removed from the Spreadsheet.
* If the english (aka, the default) translation in the Project is different from the translation on the Spreadsheet, then the english translation on the Spreadsheet is updated, and the other translations are marked red (R234 G153 B153).
* The Project writes all english translations to the google Spreadsheet without question.
* The google Spreadsheet writes all non-english translations to the Project without question.
* The script can detect potential renames. If an english string from one key matches the english string of another key form the past, it will ask the user to resolve them manually.
* The script assumes that the keys in the Spreadsheet have been organized to be in lexicographic case-insensitive order.  If they aren't, then the script will throw an error!
* You cannot add any non-english translations to the Project directly unless you expect them to be overwritten by the script the next time it's run.  The google Spreadsheet writes all non-english translations to the Project without question!


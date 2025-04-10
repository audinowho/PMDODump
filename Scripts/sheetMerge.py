import os
import shutil

import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery

WAIT_TIME = 3

class SheetMerge:
    """
    A class for syncronizing the strings in the project files
    with the google doc spreadsheets.
    """

    def __init__(self, credentials, id, sheet_content_start):
        self._credentials = credentials
        self._id = id
        self._sheet_content_start = sheet_content_start

    def open_sheet(self):
        http = self._credentials.authorize(httplib2.Http())
        discoveryUrl = ('https://sheets.googleapis.com/$discovery/rest?' 'version=v4')
        self._service = discovery.build('sheets', 'v4', http=http, discoveryServiceUrl=discoveryUrl)

    def close_sheet(self):
        pass


    def _query_sheet(self, sheet_name):
        """
        Gets all data from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """
        check_range = sheet_name + "!A"+str(self._sheet_content_start)+":A"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        return self._query_sheet_range(sheet_name, sheet_name + "!"+str(self._sheet_content_start)+":"+str(self._sheet_content_start+total_rows-1))

    def _query_sheet_range(self, sheet_name, range_name, user_entered=False):
        """
        Gets all values from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """

        header_range = sheet_name + "!A"+str(self._sheet_content_start-1)+":"+str(self._sheet_content_start-1)
        header_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=header_range).execute()
        header_row = header_result.get('values', [[]])[0]

        result = self._service.spreadsheets().get(spreadsheetId=self._id, ranges=range_name, includeGridData=True).execute()
        time.sleep(WAIT_TIME)

        max_cells = 0
        content_rows = []
        for row in result['sheets'][0]['data'][0]['rowData']:
            content_row = []
            if 'values' in row:
                for cell in row['values']:
                    content_str = ''
                    read_val = None

                    chosen_val = 'effectiveValue'
                    if user_entered:
                        chosen_val = 'userEnteredValue'
                    if chosen_val in cell:
                        read_val = cell[chosen_val]

                    if read_val is not None:
                        if 'stringValue' in read_val:
                            content_str = read_val['stringValue']
                        elif 'boolValue' in read_val:
                            content_str = str(read_val['boolValue'])
                        elif 'numberValue' in read_val:
                            content_str = str(read_val['numberValue'])
                        elif 'formulaValue' in read_val:
                            content_str = str(read_val['formulaValue'])

                    if '\n' in content_str:
                        print('Newline found in ' + sheet_name + ': ' + content_str)
                    content_row.append(content_str)

            max_cells = max(len(content_row), max_cells)
            content_rows.append(content_row)

        for row_idx in range(len(content_rows)):
            while len(content_rows[row_idx]) < max_cells:
                content_rows[row_idx].append("")

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

        keys = sorted(keys, key=str.lower)
        result = []
        for key in keys:
            result.append([key, resx_comment_dict[key], resx_dict[key]])
        return result

    def _write_sheet_table(self, sheet_name, table):
        """
        Takes the local array of values,
        and writes directly to the remote google sheet.
        """

        range_name = sheet_name + "!" + str(self._sheet_content_start) + ":" + str(self._sheet_content_start+len(table)-1)
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

    def _write_resx(self, resource, resx_path):
        """
        Takes the array of localization strings from google sheets,
        and writes it to a .resx file
        """

        file_name = resx_path + '.' + 'resx'
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

            for row in resource:
                key = row[0]
                comment = row[1]
                val = row[2]

                txt.write("  <data name=\""+key+"\" xml:space=\"preserve\">\n")
                txt.write("    <value>"+val+"</value>\n")
                txt.write("    <comment>"+comment+"</comment>")
                txt.write("  </data>\n")
            txt.write("</root>")
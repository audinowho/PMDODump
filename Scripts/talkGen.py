import os
import httplib2
import xml.etree.ElementTree as ET
import glob
import time
from apiclient import discovery

SHEET_CONTENT_START = 5
WAIT_TIME = 3
SPECIES_INDEX = 1
FORM_INDEX = 2
GENDER_INDEX = 3
TALK_INDEX = 4

class TalkGen:
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





    def _query_sheet(self, sheet_name):
        """
        Gets all data from a given google sheet, automatically figuring out height/width.
        Includes header row.
        """
        check_range = sheet_name + "!A"+str(SHEET_CONTENT_START)+":A"
        check_result = self._service.spreadsheets().values().get(spreadsheetId=self._id, range=check_range).execute()
        total_rows = len(check_result.get('values', []))

        return self._query_sheet_range(sheet_name, sheet_name + "!"+str(SHEET_CONTENT_START)+":"+str(SHEET_CONTENT_START+total_rows-1))

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

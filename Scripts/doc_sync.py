import os
import re
import json
import shutil

import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom

def convertToDocs(node, class_from, class_to):
    node_class_name = node.attrib["name"]
    node_class_name = node_class_name[2:]

    wiki_section = []
    class_call = "== {0}:{1} ==".format(class_to, node_class_name[(len(class_from)+1):])
    wiki_section.append(class_call)

    summary_node = node.find("summary")
    if summary_node is not None and summary_node.text is not None:
        summary_lines = summary_node.text.split("\n")
        summary_lines = [line.strip() for line in summary_lines]
        wiki_section.append("\n".join(summary_lines))

    params = []
    for param_node in node.iter('param'):
        params.append("* <code>{0}</code>: {1}".format(param_node.attrib["name"], param_node.text))

    if len(params) > 0:
        args_header = "=== Arguments ==="
        wiki_section.append(args_header)

        wiki_section.append("\n".join(params))

    returns = ""
    returns_node = node.find("returns")
    if returns_node is not None and returns_node.text is not None:
        returns = returns_node.text

    if returns != "":
        returns_header = "=== Returns ==="
        wiki_section.append(returns_header)

        wiki_section.append(returns)

    example_node = node.find("example")

    if example_node is not None:
        example_header = "=== Example ==="
        wiki_section.append(example_header)

        example_lines = example_node.text.split("\n")
        example_lines = [line.strip() for line in example_lines]
        example = "<pre>\n{0}\n</pre>".format("\n".join(example_lines))
        wiki_section.append(example)

    return "\n\n".join(wiki_section)

def getRelevantClassMap(node, classes):

    node_class_name = node.attrib["name"]
    if not node_class_name.startswith("M:") and not node_class_name.startswith("F:"):
        return None
    node_class_name = node_class_name[2:]
    for class_name in classes:
        if node_class_name.startswith(class_name):
            return class_name

    return None

def convertClassesToWiki(doc_path, classes):
    tree = ET.parse(os.path.join("..","DumpAsset","Editor","Docs", doc_path))
    root = tree.getroot()
    members_node = root.find('members')

    with open(os.path.join("..", "DataAsset", "Docs", "Script.txt"), 'w', encoding='utf-8') as txt:
        for member_node in members_node.iter('member'):
            class_key = getRelevantClassMap(member_node, classes)
            if class_key is not None:
                doc_txt = convertToDocs(member_node, class_key, classes[class_key])
                txt.write(doc_txt + "\n\n")

def main():

    class_maps = {
        "RogueEssence.Script.ScriptUI": "UI",
        "RogueEssence.Script.ScriptGround": "GROUND",
        "RogueEssence.Script.ScriptDungeon": "DUNGEON",
        "RogueEssence.Script.ScriptGame": "GAME",
        "RogueEssence.Script.ScriptSound": "SOUND",
        "RogueEssence.Script.ScriptStrings": "STRINGS",
        "RogueEssence.Script.ScriptTask": "TASK",
        "RogueEssence.Script.ScriptAI": "AI"
    }

    convertClassesToWiki("RogueEssence.xml", class_maps)


    print("Complete.")

if __name__ == '__main__':
    main()
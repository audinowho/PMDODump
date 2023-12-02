import os
import re
import json
import shutil

import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom

def convertToDocs(node, class_from, class_to, sep):
    node_class_name = node.attrib["name"]
    node_class_name = node_class_name[2:]

    wiki_section = []
    class_call = "== {0}{1}{2} ==".format(class_to, sep, node_class_name[(len(class_from)+1):])
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

def getRelevantClassMap(node, classes, node_types):

    node_class_name = node.attrib["name"]
    node_separator = None
    for node_type in node_types:
        if node_class_name.startswith(node_type + ":"):
            node_separator = node_types[node_type]

    if node_separator is not None:
        node_class_name = node_class_name[2:]
        for class_name in classes:
            if node_class_name.startswith(class_name):
                return (class_name, node_separator)

    return None, None

def convertClassesToWiki(out_file, classes, node_types):
    doc_paths = ["RogueEssence.xml", "PMDC.xml", "RogueElements.xml"]

    with open(os.path.join("..", "DataAsset", "Docs", out_file), 'w', encoding='utf-8') as txt:

        for doc_path in doc_paths:
            tree = ET.parse(os.path.join("..","DumpAsset","Editor","Docs", doc_path))
            root = tree.getroot()
            members_node = root.find('members')
            for member_node in members_node.iter('member'):
                class_key, sep = getRelevantClassMap(member_node, classes, node_types)
                if class_key is not None:
                    doc_txt = convertToDocs(member_node, class_key, classes[class_key], sep)
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

    convertClassesToWiki("Script.txt", class_maps, {"M": ":", "F": ":"})

    class_maps = {
        "RogueEssence.Dungeon.Character": "Character",
        "RogueEssence.Dungeon.CharData": "CharData"
    }
    convertClassesToWiki("Character.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Ground.GroundChar": "GroundChar",
        "RogueEssence.Ground.GroundAIUser": "GroundAIUser",
        "RogueEssence.Ground.BaseTaskUser": "BaseTaskUser",
        "RogueEssence.Ground.GroundEntity": "GroundEntity"
    }
    convertClassesToWiki("GroundChar.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Dungeon.BattleContext": "BattleContext",
        "RogueEssence.Dungeon.UserTargetGameContext": "UserTargetGameContext",
        "RogueEssence.Dungeon.GameContext": "GameContext"
    }

    convertClassesToWiki("BattleContext.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.BattleData": "BattleData"
    }

    convertClassesToWiki("BattleData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.TileData": "TileData"
    }

    convertClassesToWiki("TileData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.MapStatusData": "MapStatusData"
    }

    convertClassesToWiki("MapStatusData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.StatusData": "StatusData"
    }

    convertClassesToWiki("StatusData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.SkillData": "SkillData"
    }

    convertClassesToWiki("SkillData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.IntrinsicData": "IntrinsicData"
    }

    convertClassesToWiki("IntrinsicData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.ItemData": "ItemData"
    }

    convertClassesToWiki("ItemData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.MonsterData": "MonsterData",
        "RogueEssence.Data.BaseMonsterForm": "BaseMonsterForm",
        "PMDC.Data.MonsterFormData": "MonsterFormData"
    }

    convertClassesToWiki("MonsterData.txt", class_maps, {"P": ".", "F": "."})

    class_maps = {
        "RogueEssence.Data.DataManager": "_DATA"
    }

    convertClassesToWiki("DataManager.txt", class_maps, {"P": ".", "F": ".", "M": ":"})

    print("Complete.")

if __name__ == '__main__':
    main()
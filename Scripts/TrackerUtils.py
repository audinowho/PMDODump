import sys
import os
import re
import shutil
import math
import struct
import glob
import time
import urllib
import urllib.request
from PIL import Image, ImageDraw, ImageFont
import datetime
import json
from io import BytesIO
import zipfile
import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom
import utils as exUtils
import Constants


MAX_SECONDARY_CREDIT = 2
PHASE_INCOMPLETE = 0
PHASE_EXISTS = 1
PHASE_FULL = 2


def getCreditEntries(path):
    credits = getFileCredits(path)

    found_names = {}
    credit_strings = []
    for credit in credits:
        credit_id = credit[1]
        if credit_id not in found_names:
            credit_strings.append(credit_id)
            found_names[credit_id] = True
    return credit_strings

def getFileCredits(path):
    id_list = []
    if os.path.exists(os.path.join(path, "credits.txt")):
        with open(os.path.join(path, "credits.txt"), 'r', encoding='utf-8') as txt:
            for line in txt:
                id_list.append(line.strip().split('\t'))
    return id_list

def appendCredits(path, id, diff):
    with open(os.path.join(path, "credits.txt"), 'a+', encoding='utf-8') as txt:
        txt.write("{0}\t{1}\t{2}\n".format(str(datetime.datetime.utcnow()), id, diff))

class CreditEntry:
    """
    A class for handling recolors
    """
    def __init__(self, name, contact):
        self.name = name
        self.sprites = False
        self.portraits = False
        self.contact = contact

class TrackerNode:

    def __init__(self, node_dict):
        temp_list = [i for i in node_dict]
        temp_list = sorted(temp_list)

        main_dict = { }
        for key in temp_list:
            main_dict[key] = node_dict[key]

        self.__dict__ = main_dict

        self.sprite_credit = CreditNode(node_dict["sprite_credit"])
        self.portrait_credit = CreditNode(node_dict["portrait_credit"])

        sub_dict = { }
        for key in self.subgroups:
            sub_dict[key] = TrackerNode(self.subgroups[key])
        self.subgroups = sub_dict

    def getDict(self):
        node_dict = { }
        for k in self.__dict__:
            node_dict[k] = self.__dict__[k]

        node_dict["sprite_credit"] = self.sprite_credit.getDict()
        node_dict["portrait_credit"] = self.portrait_credit.getDict()

        sub_dict = { }
        for sub_idx in self.subgroups:
            sub_dict[sub_idx] = self.subgroups[sub_idx].getDict()
        node_dict["subgroups"] = sub_dict
        return node_dict

class CreditNode:

    def __init__(self, node_dict):
        temp_list = [i for i in node_dict]
        temp_list = sorted(temp_list)

        main_dict = { }
        for key in temp_list:
            main_dict[key] = node_dict[key]

        self.__dict__ = main_dict

    def getDict(self):
        node_dict = { }
        for k in self.__dict__:
            node_dict[k] = self.__dict__[k]
        return node_dict

def loadNameFile(name_path):
    name_dict = { }
    first = True
    with open(name_path, encoding='utf-8') as txt:
        for line in txt:
            if first:
                first = False
                continue
            cols = line[:-1].split('\t')
            name_dict[cols[1]] = CreditEntry(cols[0], cols[2])
    return name_dict

def initCreditDict():
    credit_dict = { }
    credit_dict["primary"] = ""
    credit_dict["secondary"] = []
    credit_dict["total"] = 0
    return credit_dict

def initSubNode(name, canon):
    sub_dict = { }
    sub_dict["name"] = name
    sub_dict["canon"] = canon
    sub_dict["modreward"] = not canon
    sub_dict["portrait_complete"] = 0
    sub_dict["portrait_credit"] = initCreditDict()
    sub_dict["portrait_link"] = ""
    sub_dict["portrait_files"] = {}
    sub_dict["portrait_bounty"] = {}
    sub_dict["portrait_modified"] = ""
    sub_dict["portrait_pending"] = {}
    sub_dict["portrait_recolor_link"] = ""
    sub_dict["portrait_required"] = False
    sub_dict["sprite_complete"] = 0
    sub_dict["sprite_credit"] = initCreditDict()
    sub_dict["sprite_files"] = {}
    sub_dict["sprite_bounty"] = {}
    sub_dict["sprite_link"] = ""
    sub_dict["sprite_modified"] = ""
    sub_dict["sprite_pending"] = {}
    sub_dict["sprite_recolor_link"] = ""
    sub_dict["sprite_required"] = False
    sub_dict["subgroups"] = {}
    return TrackerNode(sub_dict)

def getCurrentCompletion(dict, prefix):
    if prefix == "sprite":
        completion = PHASE_FULL
        while completion > PHASE_INCOMPLETE:
            has_all = True
            for idx in Constants.COMPLETION_ACTIONS[completion]:
                file = Constants.ACTIONS[idx]
                if file not in dict.__dict__[prefix + "_files"]:
                    has_all = False
                    break
            if has_all:
                break
            completion -= 1
        return completion
    else:
        completion = PHASE_FULL
        search_flip = False
        for file in dict.__dict__[prefix + "_files"]:
            if file.endswith("^"):
                search_flip = True
                break
        while completion > PHASE_INCOMPLETE:
            has_all = True
            for idx in Constants.COMPLETION_EMOTIONS[completion]:
                file = Constants.EMOTIONS[idx]
                if file not in dict.__dict__[prefix + "_files"]:
                    has_all = False
                    break
                if search_flip:
                    file_flip = file + "^"
                    if file_flip not in dict.__dict__[prefix + "_files"]:
                        has_all = False
                        break
            if has_all:
                break
            completion -= 1
        return completion



def updateFiles(dict, species_path, prefix):
    file_list = []
    if prefix == "sprite":
        if os.path.exists(os.path.join(species_path, Constants.MULTI_SHEET_XML)):
            tree = ET.parse(os.path.join(species_path, Constants.MULTI_SHEET_XML))
            root = tree.getroot()
            anims_node = root.find('Anims')
            for anim_node in anims_node.iter('Anim'):
                name = anim_node.find('Name').text
                file_list.append(name)
    else:
        for inFile in os.listdir(species_path):
            if inFile.endswith(".png"):
                file, _ = os.path.splitext(inFile)
                file_list.append(file)

    for file in file_list:
        if file not in dict.__dict__[prefix + "_files"]:
            dict.__dict__[prefix + "_files"][file] = False

def updateCreditFromEntries(credit_data, credit_entries):
    # updates just the total count and the secondary
    # the primary author is user-defined
    credit_data.total = len(credit_entries)
    credit_data.secondary.clear()

    for credit_entry in reversed(credit_entries):
        if credit_entry != credit_data.primary:
            credit_data.secondary.insert(0, credit_entry)
            if len(credit_data.secondary) >= MAX_SECONDARY_CREDIT:
                break



def fileSystemToJson(dict, species_path, prefix, tier):
    # get last modify date of everything that isn't credits.txt or dirs
    last_modify = ""
    for inFile in os.listdir(species_path):
        fullPath = os.path.join(species_path, inFile)
        if os.path.isdir(fullPath):
            if inFile not in dict.subgroups:
                # init name if applicable
                if tier == 1:
                    if inFile == "0000":
                        dict.subgroups[inFile] = initSubNode("", dict.canon)
                    else:
                        dict.subgroups[inFile] = initSubNode("Form" + inFile, dict.canon)
                elif tier == 2:
                    if inFile == "0001":
                        dict.subgroups[inFile] = initSubNode("Shiny", dict.canon)
                    else:
                        dict.subgroups[inFile] = initSubNode("", dict.canon)
                elif tier == 3:
                    if inFile == "0001":
                        dict.subgroups[inFile] = initSubNode("Male", dict.canon)
                    elif inFile == "0002":
                        dict.subgroups[inFile] = initSubNode("Female", dict.canon)
                    else:
                        dict.subgroups[inFile] = initSubNode("", dict.canon)

            fileSystemToJson(dict.subgroups[inFile], fullPath, prefix, tier + 1)
        elif inFile == "credits.txt":
            credit_entries = getCreditEntries(species_path)
            credit_data = dict.__dict__[prefix + "_credit"]
            updateCreditFromEntries(credit_data, credit_entries)
        else:
            modify_datetime = datetime.datetime.utcfromtimestamp(os.path.getmtime(fullPath))
            if str(modify_datetime) > last_modify:
                last_modify = str(modify_datetime)

    updateFiles(dict, species_path, prefix)

    updated = False
    if dict.__dict__[prefix + "_modified"] < last_modify:
        dict.__dict__[prefix + "_modified"] = last_modify
        updated = True

    # the link always starts off blank, or is set to blank when last-modified is updated
    if updated:
        dict.__dict__[prefix + "_link"] = ""

def isDataPopulated(sub_dict):
    if sub_dict.sprite_credit.primary != "":
        return True
    if sub_dict.portrait_credit.primary != "":
        return True

    for sub_idx in sub_dict.subgroups:
        if isDataPopulated(sub_dict.subgroups[sub_idx]):
            return True
    return False

def deleteData(path):
    if os.path.exists(path):
        shutil.rmtree(path)

def getIdxName(tracker_dict, full_idx):
    if len(full_idx) == 0:
        return []
    if full_idx[0] not in tracker_dict:
        return []
    node = tracker_dict[full_idx[0]]
    if node.name == "":
        return getIdxName(node.subgroups, full_idx[1:])
    else:
        return [node.name] + getIdxName(node.subgroups, full_idx[1:])


def findSlotIdx(sub_dict, name):
    for idx in sub_dict:
        if sub_dict[idx].name.lower() == name.lower():
            return idx
    return None

def iterateTracker(tracker_dict, func, full_idx):
    for idx in tracker_dict:
        node = tracker_dict[idx]

        full_idx.append(idx)
        func(full_idx)
        sub_dict = node.subgroups
        iterateTracker(sub_dict, func, full_idx)
        full_idx.pop()

def findFullTrackerIdx(tracker_dict, name_args, depth):
    # base case
    if depth >= len(name_args):
        return []

    # recursive case
    blank_idx = None
    for idx in tracker_dict:
        if tracker_dict[idx].name.lower() == name_args[depth].lower():
            sub_dict = tracker_dict[idx].subgroups
            full_idx = findFullTrackerIdx(sub_dict, name_args, depth+1)
            if full_idx is None:
                return None
            else:
                return [idx] + full_idx
        elif tracker_dict[idx].name == "":
            blank_idx = idx
    # didn't find any name matches, check if the base name is blank
    if blank_idx is not None:
        full_idx = findFullTrackerIdx(tracker_dict[blank_idx].subgroups, name_args, depth)
        if full_idx is None:
            return None
        else:
            return [blank_idx] + full_idx
    # otherwise, it means we just can't find it
    return None

def isShinyIdx(full_idx):
    if len(full_idx) < 3:
        return False
    return full_idx[2] == "0001"

def createShinyIdx(full_idx, shiny):
    new_idx = [i for i in full_idx]
    if len(new_idx) < 3 and not shiny:
        return new_idx

    while len(new_idx) < 3:
        new_idx.append("0000")

    if shiny:
        new_idx[2] = "0001"
    else:
        new_idx[2] = "0000"

    while len(new_idx) > 1 and new_idx[-1] == "0000":
        new_idx.pop()
    return new_idx

def getNodeFromIdx(tracker_dict, full_idx, depth):
    if full_idx is None:
        return None
    if len(full_idx) == 0:
        return None
    if full_idx[depth] not in tracker_dict:
        return None

    # base case
    node = tracker_dict[full_idx[depth]]
    if depth == len(full_idx) - 1:
        return node

    # recursive case, kind of weird
    return getNodeFromIdx(node.subgroups, full_idx, depth+1)

def getStatsFromFilename(filename):
    # attempt to parse the filename to a destination
    file, ext = os.path.splitext(filename)
    name_idx = file.split("-")
    prefix = name_idx[0].split("_")

    asset_type = prefix[0]
    recolor = "_".join(prefix[1:])

    if asset_type != "sprite" and asset_type != "portrait":
        return False, None, None, None
    if recolor != "" and recolor != "recolor":
        return False, None, None, None

    if len(name_idx) < 2:
        return False, None, None, None

    full_idx = name_idx[1:]
    return True, full_idx, asset_type, recolor == "recolor"

def genderDiffExists(form_dict, asset_type):
    if "0000" not in form_dict.subgroups:
        return False
    normal_dict = form_dict.subgroups["0000"].subgroups
    female_idx = findSlotIdx(normal_dict, "Female")
    if female_idx is None:
        return False
    female_dict = normal_dict[female_idx]

    return female_dict.__dict__[asset_type + "_required"]

def genderDiffPopulated(form_dict, asset_type):
    if "0000" not in form_dict.subgroups:
        return False
    normal_dict = form_dict.subgroups["0000"].subgroups
    female_idx = findSlotIdx(normal_dict, "Female")
    if female_idx is None:
        return False
    female_dict = normal_dict[female_idx]

    return female_dict.__dict__[asset_type + "_credit"].primary != ""

def createGenderDiff(form_dict, asset_type):
    if "0000" not in form_dict.subgroups:
        form_dict.subgroups["0000"] = initSubNode("", form_dict.canon)
    normal_dict = form_dict.subgroups["0000"]
    createShinyGenderDiff(normal_dict, asset_type)

    shiny_dict = form_dict.subgroups["0001"]
    createShinyGenderDiff(shiny_dict, asset_type)

def createShinyGenderDiff(color_dict, asset_type):
    female_idx = findSlotIdx(color_dict.subgroups, "Female")
    if female_idx is None:
        female_dict = initSubNode("Female", color_dict.canon)
        color_dict.subgroups["0002"] = female_dict
    else:
        female_dict = color_dict.subgroups[female_idx]
    female_dict.__dict__[asset_type + "_required"] = True


def removeGenderDiff(form_dict, asset_type):
    normal_dict = form_dict.subgroups["0000"]
    nothing_left = removeColorGenderDiff(normal_dict, asset_type)
    if nothing_left:
        del form_dict.subgroups["0000"]

    shiny_dict = form_dict.subgroups["0001"]
    removeColorGenderDiff(shiny_dict, asset_type)

def removeColorGenderDiff(color_dict, asset_type):
    # return whether or not the gender was fully deleted
    female_idx = findSlotIdx(color_dict.subgroups, "Female")
    if female_idx is None:
        return True

    female_dict = color_dict.subgroups[female_idx]
    female_dict.__dict__[asset_type + "_required"] = False
    if not female_dict.__dict__["sprite_required"] and not female_dict.__dict__["portrait_required"]:
        del color_dict.subgroups[female_idx]
        return True

    return False

def createFormNode(name, canon):
    forme_dict = initSubNode(name, canon)
    forme_dict.sprite_required = True
    forme_dict.portrait_required = True
    shiny_dict = initSubNode("Shiny", canon)
    forme_dict.subgroups["0001"] = shiny_dict
    shiny_dict.sprite_required = True
    shiny_dict.portrait_required = True
    return forme_dict

def createSpeciesNode(name):

    sub_dict = initSubNode(name, True)
    sub_dict.sprite_required = True
    sub_dict.portrait_required = True
    forme_dict = initSubNode("", True)
    sub_dict.subgroups["0000"] = forme_dict
    shiny_dict = initSubNode("Shiny", True)
    forme_dict.subgroups["0001"] = shiny_dict
    shiny_dict.sprite_required = True
    shiny_dict.portrait_required = True
    return sub_dict

def clearSubmissions(node):
    node.sprite_pending = { }
    node.portrait_pending = { }

    for sub_idx in node.subgroups:
        clearSubmissions(node.subgroups[sub_idx])

"""
Name operations
"""
def updateNameFile(name_path, name_dict, include_all):
    with open(name_path, 'w+', encoding='utf-8') as txt:
        txt.write("Name\tDiscord\tContact\n")
        for handle in name_dict:
            if include_all or name_dict[handle].sprites or name_dict[handle].portraits:
                txt.write("{0}\t{1}\t{2}\n".format(name_dict[handle].name, handle, name_dict[handle].contact))

def addNameStat(name_dict, name, portrait):
    if name != "":
        if name not in name_dict:
            name_dict[name] = CreditEntry("", "")
        if not portrait:
            name_dict[name].sprites = True
        else:
            name_dict[name].portraits = True

def updateNameStats(name_dict, dict):
    addNameStat(name_dict, dict.sprite_credit.primary, False)
    for secondary in dict.sprite_credit.secondary:
        addNameStat(name_dict, secondary, False)
    addNameStat(name_dict, dict.portrait_credit.primary, True)
    for secondary in dict.portrait_credit.secondary:
        addNameStat(name_dict, secondary, True)

    for sub_dict in dict.subgroups:
        updateNameStats(name_dict, dict.subgroups[sub_dict])

def renameJsonCredits(dict, old_name, new_name):
    if dict.sprite_credit.primary == old_name:
        dict.sprite_credit.primary = new_name
    for idx in range(len(dict.sprite_credit.secondary)):
        if dict.sprite_credit.secondary[idx] == old_name:
            dict.sprite_credit.secondary[idx] = new_name

    if dict.portrait_credit.primary == old_name:
        dict.portrait_credit.primary = new_name
    for idx in range(len(dict.portrait_credit.secondary)):
        if dict.portrait_credit.secondary[idx] == old_name:
            dict.portrait_credit.secondary[idx] = new_name

    for sub_dict in dict.subgroups:
        renameJsonCredits(dict.subgroups[sub_dict], old_name, new_name)


def renameFileCredits(species_path, old_name, new_name):
    # renames all mentions of an author to a different author
    for inFile in os.listdir(species_path):
        fullPath = os.path.join(species_path, inFile)
        if os.path.isdir(fullPath):
            renameFileCredits(fullPath, old_name, new_name)
        elif inFile == "credits.txt":
            id_list = []
            with open(fullPath, 'r', encoding='utf-8') as txt:
                for line in txt:
                    id_list.append(line.strip().split('\t'))
            for entry in id_list:
                if entry[1] == old_name:
                    entry[1] = new_name
            with open(fullPath, 'w', encoding='utf-8') as txt:
                for entry in id_list:
                    txt.write(entry[0] + "\t" + entry[1] + "\n")

def getDirFromIdx(base_path, asset_type, full_idx):
    full_arr = [base_path, asset_type] + full_idx
    return os.path.join(*full_arr)

def moveNodeFiles(dir_from, dir_to, is_dir):
    cur_files = os.listdir(dir_from)
    for file in cur_files:
        # exclude tmp as it is a special folder name for temp files
        if file == "tmp":
            continue
        full_base_path = os.path.join(dir_from, file)
        if os.path.isdir(full_base_path) == is_dir:
            shutil.move(full_base_path, os.path.join(dir_to, file))


def swapNodeMiscFeatures(node_from, node_to):
    for key in node_from.__dict__:
        if key.startswith("sprite"):
            pass
        elif key.startswith("portrait"):
            pass
        elif key == "subgroups":
            pass
        else:
            tmp = node_to.__dict__[key]
            node_to.__dict__[key] = node_from.__dict__[key]
            node_from.__dict__[key] = tmp

def swapNodeAssetFeatures(node_from, node_to, asset_type):
    for key in node_from.__dict__:
        if key.startswith(asset_type):
            tmp = node_to.__dict__[key]
            node_to.__dict__[key] = node_from.__dict__[key]
            node_from.__dict__[key] = tmp


def swapFolderPaths(base_path, tracker, asset_type, full_idx_from, full_idx_to):

    # swap the nodes in tracker, don't do it recursively
    chosen_node_from = getNodeFromIdx(tracker, full_idx_from, 0)
    chosen_node_to = getNodeFromIdx(tracker, full_idx_to, 0)

    swapNodeAssetFeatures(chosen_node_from, chosen_node_to, asset_type)

    # prepare to swap textures
    gen_path_from = getDirFromIdx(base_path, asset_type, full_idx_from)
    gen_path_to = getDirFromIdx(base_path, asset_type, full_idx_to)

    if not os.path.exists(gen_path_from):
        os.makedirs(gen_path_from, exist_ok=True)

    if not os.path.exists(gen_path_to):
        os.makedirs(gen_path_to, exist_ok=True)

    # move textures to a temp folder
    gen_path_tmp = os.path.join(gen_path_from, "tmp")
    os.makedirs(gen_path_tmp, exist_ok=True)
    moveNodeFiles(gen_path_from, gen_path_tmp, False)

    # swap the folders
    moveNodeFiles(gen_path_to, gen_path_from, False)
    moveNodeFiles(gen_path_tmp, gen_path_to, False)
    shutil.rmtree(gen_path_tmp)


def swapAllSubNodes(base_path, tracker, full_idx_from, full_idx_to):
    # swap the subnode objects
    chosen_node_from = getNodeFromIdx(tracker, full_idx_from, 0)
    chosen_node_to = getNodeFromIdx(tracker, full_idx_to, 0)

    tmp = chosen_node_from.subgroups
    chosen_node_from.subgroups = chosen_node_to.subgroups
    chosen_node_to.subgroups = tmp

    # swap the subfolders for each asset
    asset_types = ["sprite", "portrait"]

    for asset_type in asset_types:
        gen_path_from = getDirFromIdx(base_path, asset_type, full_idx_from)
        gen_path_to = getDirFromIdx(base_path, asset_type, full_idx_to)

        if not os.path.exists(gen_path_from):
            os.makedirs(gen_path_from, exist_ok=True)

        if not os.path.exists(gen_path_to):
            os.makedirs(gen_path_to, exist_ok=True)

        # move dirs to a temp folder
        gen_path_tmp = os.path.join(gen_path_from, "tmp")
        os.makedirs(gen_path_tmp, exist_ok=True)
        moveNodeFiles(gen_path_from, gen_path_tmp, True)

        # swap the folders
        moveNodeFiles(gen_path_to, gen_path_from, True)
        moveNodeFiles(gen_path_tmp, gen_path_to, True)
        shutil.rmtree(gen_path_tmp)


def hasLock(dict, asset_type, recursive):
    for file in dict.__dict__[asset_type + "_files"]:
        if dict.__dict__[asset_type + "_files"][file]:
            return True

    if recursive:
        for subgroup in dict.subgroups:
            if hasLock(dict.subgroups[subgroup], asset_type, recursive):
                return True

    return False

def setCanon(dict, canon):
    dict.canon = canon

    for subgroup in dict.subgroups:
        setCanon(dict.subgroups[subgroup], canon)

"""
String operations
"""
def sanitizeName(str):
    return re.sub("\W+", "_", str).title()

def sanitizeCredit(str):
    return re.sub("\t\n", "", str)


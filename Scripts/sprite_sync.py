import os
import json
import shutil
from PIL import Image

import TrackerUtils
import utils as exUtils

TRANSFER_NONE = -2
TRANSFER_DEFAULT = -1

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


def initTransferNode():
    sub_dict = { }
    sub_dict["name"] = ""
    sub_dict["portrait_dest"] = None
    sub_dict["sprite_dest"] = None
    sub_dict["subgroups"] = {}
    return TransferNode(sub_dict)


def loadTransferMap(path):
    over_transfer = initTransferNode()
    over_transfer.portrait_dest = TRANSFER_NONE
    over_transfer.sprite_dest = TRANSFER_NONE
    try:
        with open(path) as f:
            new_tracker = json.load(f)
            transfer_tracker = {}
            for species_idx in new_tracker:
                transfer_tracker[species_idx] = TransferNode(new_tracker[species_idx])
            over_transfer.subgroups = transfer_tracker
    except:
        pass
    return over_transfer


class TransferNode:

    def __init__(self, node_dict):
        temp_list = [i for i in node_dict]
        temp_list = sorted(temp_list)

        main_dict = { }
        for key in temp_list:
            main_dict[key] = node_dict[key]

        self.__dict__ = main_dict

        sub_dict = { }
        for key in self.subgroups:
            sub_dict[key] = TransferNode(self.subgroups[key])
        self.subgroups = sub_dict

    def getDict(self):
        node_dict = { }
        for k in self.__dict__:
            node_dict[k] = self.__dict__[k]

        sub_dict = { }
        for sub_idx in self.subgroups:
            sub_dict[sub_idx] = self.subgroups[sub_idx].getDict()
        node_dict["subgroups"] = sub_dict
        return node_dict


def generateMap(transfer_dict, dict, name_stack, added_nodes):
    if len(name_stack) == 2 and dict.name != "" and transfer_dict.name != dict.name:
        result_str = " ".join(name_stack)
        if not dict.canon:
            result_str = "*" + result_str
        added_nodes.append(result_str)
    transfer_dict.name = dict.name
    if transfer_dict.portrait_dest is None:
        if not dict.canon:
            transfer_dict.portrait_dest = TRANSFER_NONE
        elif dict.name == "":
            transfer_dict.portrait_dest = TRANSFER_NONE
        else:
            transfer_dict.portrait_dest = TRANSFER_DEFAULT

    if transfer_dict.sprite_dest is None:
        if not dict.canon:
            transfer_dict.sprite_dest = TRANSFER_NONE
        elif dict.name == "":
            transfer_dict.sprite_dest = TRANSFER_NONE
        else:
            transfer_dict.sprite_dest = TRANSFER_DEFAULT

    for subgroup in dict.subgroups:
        sub_node = dict.subgroups[subgroup]
        if subgroup not in transfer_dict.subgroups:
            transfer_dict.subgroups[subgroup] = initTransferNode()
        generateMap(transfer_dict.subgroups[subgroup], sub_node, name_stack + [sub_node.name], added_nodes)

def importFiles(orig_path, out_path, file_diff):
    for in_file in os.listdir(orig_path):
        full_path = os.path.join(orig_path, in_file)
        dest_path = os.path.join(out_path, in_file)
        if not os.path.isdir(full_path):
            if full_path.endswith(".png") and os.path.exists(dest_path):
                new_img = Image.open(full_path).convert("RGBA")
                old_img = Image.open(dest_path).convert("RGBA")
                if not exUtils.imgsEqual(new_img, old_img, False):
                    file_diff.append(in_file)
            shutil.copy(full_path, dest_path)

def importFolders(prefix, base_path, transfer_dict, out_path, dict_path, diff):

    src_path = os.path.join(base_path, prefix, *dict_path)

    file_diff = []
    transfer_dest = transfer_dict.__dict__[prefix + "_dest"]
    if transfer_dest > TRANSFER_NONE:
        if not os.path.exists(src_path):
            return

        redirected_dict = dict_path.copy()
        if transfer_dest > TRANSFER_DEFAULT:
            if len(redirected_dict) < 2:
                redirected_dict.append("0000")
            redirected_dict[1] = "{:04d}".format(transfer_dest)

        dest_path = os.path.join(out_path, prefix, *redirected_dict)
        os.makedirs(dest_path, exist_ok=True)

        importFiles(src_path, dest_path, file_diff)

    row_diff = dict_path.copy()
    while len(row_diff) < 4:
        row_diff.append('0000')
    row_diff.append(",".join(file_diff))
    diff.append(row_diff)

    for subgroup in transfer_dict.subgroups:
        sub_node = transfer_dict.subgroups[subgroup]
        importFolders(prefix, base_path, sub_node, out_path, dict_path + [subgroup], diff)


def updateTransfers(base_path, out_path):
    with open(os.path.join(base_path, "tracker.json")) as f:
        new_tracker = json.load(f)
        tracker = {}
        for species_idx in new_tracker:
            tracker[species_idx] = TrackerUtils.TrackerNode(new_tracker[species_idx])

    over_dict = TrackerUtils.initSubNode("", True)
    over_dict.subgroups = tracker
    TrackerUtils.fileSystemToJson(over_dict, os.path.join(base_path, "sprite"), "sprite", 0)
    TrackerUtils.fileSystemToJson(over_dict, os.path.join(base_path, "portrait"), "portrait", 0)

    # get the transfer dict so far
    over_transfer = loadTransferMap(os.path.join(out_path, "transfer.json"))

    # update the transfer based on newly added nodes
    added_nodes = []
    generateMap(over_transfer, over_dict, [], added_nodes)

    new_transfer = {}
    for species_idx in over_transfer.subgroups:
        new_transfer[species_idx] = over_transfer.subgroups[species_idx].getDict()
    with open(os.path.join(out_path, "transfer.json"), 'w', encoding='utf-8') as txt:
        json.dump(new_transfer, txt, indent=2)

    with open(os.path.join(out_path, "added_nodes.txt"), 'w', encoding='utf-8') as txt:
        for node in added_nodes:
            txt.write(node + "\n")

def transferWithMap(base_path, out_path, over_transfer):
    sprite_diffs = []
    portrait_diffs = []
    importFolders("sprite", base_path, over_transfer, out_path, [], sprite_diffs)
    importFolders("portrait", base_path, over_transfer, out_path, [], portrait_diffs)

    with open(os.path.join(out_path, "sprite_diffs.csv"), 'a', encoding='utf-8') as txt:
        for diff in sprite_diffs:
            txt.write(",".join(diff) + "\n")
    with open(os.path.join(out_path, "portrait_diffs.csv"), 'a', encoding='utf-8') as txt:
        for diff in portrait_diffs:
            txt.write(",".join(diff) + "\n")



def main():
    """
    Synchronizes sprites from spritecollab (and a custom folder).
    """
    base_path = os.path.join("..","..","Spritebot","SpriteCollab")
    out_path = os.path.join("..","RawAsset")

    # delete old diffs
    if os.path.exists(os.path.join(out_path, "sprite_diffs.csv")):
        os.remove(os.path.join(out_path, "sprite_diffs.csv"))
    if os.path.exists(os.path.join(out_path, "portrait_diffs.csv")):
        os.remove(os.path.join(out_path, "portrait_diffs.csv"))

    updateTransfers(base_path, out_path)

    # list denoting which sprites/portrait to transfer and to where
    main_transfer = loadTransferMap(os.path.join(out_path, "transfer.json"))
    transferWithMap(base_path, out_path, main_transfer)

    custom_path = os.path.join("..","..","Spritebot","Custom")
    # same list, but for custom sprites not in spritebot
    custom_transfer = loadTransferMap(os.path.join(out_path, "custom_transfer.json"))
    transferWithMap(custom_path, out_path, custom_transfer)

    print("Complete.")

if __name__ == '__main__':
    main()
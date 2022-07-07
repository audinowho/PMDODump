# -*- coding: utf-8 -*-

from PIL import Image
import sys
import os
import re
import shutil
import math
import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom

TILE_SIZE = 24
TOTAL_TILES = 47
PALETTE_SIZE = 10
TOTAL_COLORS = 16

TILE_MAP = [(1,4, 0x00),
            (1,6, 0x01),
            (2,7, 0x02),
            (2,3, 0x03),
            (2,0, 0x13),
            (1,8, 0x04),
            (0,4, 0x05),
            (2,5, 0x06),
            (2,2, 0x26),
            (2,10, 0x07),
            (1,18, 0x17),
            (1,17, 0x27),
            (2,1, 0x37),
            (0,7, 0x08),
            (0,3, 0x09),
            (0,0, 0x89),
            (1,3, 0x0A),
            (1,9, 0x0B),
            (0,19, 0x1B),
            (1,19, 0x8B),
            (1,0, 0x9B),
            (0,5, 0x0C),
            (0,2, 0x4C),
            (0,10, 0x0D),
            (0,17, 0x4D),
            (0,18, 0x8D),
            (0,1, 0xCD),
            (1,11, 0x0E),
            (0,20, 0x2E),
            (1,20, 0x4E),
            (1,2, 0x6E),
            (1,7, 0x0F),
            (1,21, 0x1F),
            (1,22, 0x2F),
            (0,13, 0x3F),
            (0,22, 0x4F),
            (0,23, 0x5F),
            (1,12, 0x6F),
            (0,15, 0x7F),
            (0,21, 0x8F),
            (1,14, 0x9F),
            (1,23, 0xAF),
            (0,16, 0xBF),
            (2,13, 0xCF),
            (1,16, 0xDF),
            (1,15, 0xEF),
            (1,1, 0xFF)]

DTEF_MAP = [0x89, 0x9B, 0x13, 0x09, 0x0A, 0x03,
            0xCD, 0xFF, 0x37, 0x05, 0x00, 0x06,
            0x4C, 0x6E, 0x26, 0x0C, 0x01, -1,
            0x3F, 0xCF, 0x0B, 0x08, 0x0F, 0x02,
            0x9F, 0x6F, 0x0E, 0x0D, 0x04, 0x07,
            0x7F, 0xEF, 0x4D, 0x27, 0x1B, 0x8B,
            0xBF, 0xDF, 0x8D, 0x17, 0x2E, 0x4E,
            0x8F, 0x1F, 0x4F, 0x2F, 0x5F, 0xAF]

def IsFullBlank(img):
    datas = img.getdata()
    for data in datas:
        if data[3] != 0:
            return False
    return True

def CheckBlank(img, start):
    datas = img.getdata()
    for i in range(TILE_SIZE):
        for j in range(TILE_SIZE):
            index = (i + start[0]) + (j + start[1]) * img.size[0]
            #print(str(datas[0]))
            if datas[index][3] != 0:
                return False
    return True

def HasData(img, tier):
    if (tier * 3) * (TILE_SIZE + 1) >= img.size[0]:
        return False
    for tile in range(len(TILE_MAP)):
        tile_xy = (tier * 3 + TILE_MAP[tile][0], TILE_MAP[tile][1])
        if not CheckBlank(img, (tile_xy[0] * (TILE_SIZE + 1), tile_xy[1] * (TILE_SIZE + 1))):
            return True
    return False
    
def Recolor(img, palette, index):
    if index == 0:
        return img
    
    srcData = img.getdata()
    
    newImg = Image.new('RGBA', img.size, (0, 0, 0, 0))
    datas = []
    for pixel in srcData:
        mappedPix = pixel
        for candColor in range(len(palette[0])):
            if palette[0][candColor] == pixel:
                mappedPix = palette[index][candColor]
                break
        datas.append(mappedPix)
    newImg.putdata(datas)
    return newImg

def ArrangeDtefBase(inImg):
    type_dict = { }
    for idx, mapping in enumerate(TILE_MAP):
        tile_pixel_xy = (0, idx * TILE_SIZE)
        if not CheckBlank(inImg, tile_pixel_xy):
            imgCrop = inImg.crop((0, idx * TILE_SIZE, TILE_SIZE, (idx + 1) * TILE_SIZE))
            type_dict[mapping[2]] = imgCrop

    outImg = Image.new('RGBA', (TILE_SIZE * 6, TILE_SIZE * 8), (0, 0, 0, 0))
    for idx, pos_mapping in enumerate(DTEF_MAP):
        dest_xy = (idx % 6, idx // 6)
        tile_type = DTEF_MAP[idx]
        if tile_type in type_dict:
            outImg.paste(type_dict[tile_type], (dest_xy[0] * TILE_SIZE, dest_xy[1] * TILE_SIZE))
    return outImg

def ArrangeDtefFrames(inImg, frame_count):
    frames = []
    for idx in range(frame_count):
        frame = inImg.crop((idx * TILE_SIZE, 0, (idx + 1) * TILE_SIZE, 1128))
        frames.append(ArrangeDtefBase(frame))
    return frames

def ArrangeDtefVariants(inImg, variant_count):
    variants = []
    frame_count = inImg.size[0] // TILE_SIZE // variant_count
    for idx in range(variant_count):
        frame = inImg.crop((idx * frame_count * TILE_SIZE, 0, (idx + 1) * frame_count * TILE_SIZE, 1128))
        variants.append(ArrangeDtefFrames(frame, frame_count))
    return variants

def ParseDtefInfo(filename):
    args = filename.split('.')
    name = args[0]
    layer = int(args[1])
    duration = int(args[2])
    variant_count = int(args[3])
    return name, layer, duration, variant_count

def ReshapeVariantLayers(layers):
    variants = []
    for xx, layer in enumerate(layers):
        for yy, variant in enumerate(layer):
            while yy >= len(variants):
                variants.append([])
            while xx >= len(variants[yy]):
                variants[yy].append(None)
            variants[yy][xx] = variant

    return variants


def ConvertToDtefType(outDir, idx, variants, durations):
    if not os.path.exists(outDir):
        os.makedirs(outDir)

    with open(os.path.join(outDir, "tileset.dtef.xml"), 'w', encoding='utf-8') as txt:
        txt.write("<?xml version=\"1.0\" ?>\n"
                  "<DungeonTileset dimensions=\"24\">\n"
                  "  <RogueEssence>\n"
                  "    <Wall>"+str(idx)+"</Wall>\n"
                  "  </RogueEssence>\n"
                  "</DungeonTileset>\n")

    for idx_v, layers in enumerate(variants):
        repImg = Image.new('RGBA', (TILE_SIZE * 6, TILE_SIZE * 8), (0, 0, 0, 0))
        for idx_l, frames in enumerate(layers):
            for idx_f, frame in enumerate(frames):
                if idx_f == 0:
                    repImg.paste(frame, (0, 0), frame)
                if len(frames) > 1:
                    frame.save(os.path.join(outDir, 'tileset_{0}_frame{1}_{2}.{3}.png'.format(idx_v, idx_l, idx_f, durations[idx_v][idx_l])))
        repImg.save(os.path.join(outDir, 'tileset_{0}.png'.format(idx_v)))



def ConvertToDtef(inDir, outDir):
    ground_layers = []
    wall_layers = []
    water_layers = []

    ground_layer_durations = []
    wall_layer_durations = []
    water_layer_durations = []

    ground_idx = -1
    wall_idx = -1
    water_idx = -1

    for file in os.listdir(inDir):
        filename, ext = os.path.splitext(file)
        if ext.lower() == '.png':
            name, layer, duration, variant_count = ParseDtefInfo(filename)
            ##get the layer image
            tileset = Image.open(os.path.join(inDir, file))
            tileset = tileset.convert("RGBA")

            variants = ArrangeDtefVariants(tileset, variant_count)
            variant_durations = [duration] * len(variants)

            dest_layers = None
            dest_durations = None
            if name == 'ground':
                dest_layers = ground_layers
                dest_durations = ground_layer_durations
            if name == 'wall':
                dest_layers = wall_layers
                dest_durations = wall_layer_durations
            if name == 'water':
                dest_layers = water_layers
                dest_durations = water_layer_durations

            while layer >= len(dest_layers):
                dest_layers.append(None)
            while layer >= len(dest_durations):
                dest_durations.append(None)

            dest_layers[layer] = variants
            dest_durations[layer] = variant_durations
        elif ext.lower() == '.xml':
            tree = ET.parse(os.path.join(inDir, file))
            node = tree.getroot()
            node = node.find('RogueEssence')
            ground_idx = int(node.find('Floor').text)
            wall_idx = int(node.find('Wall').text)
            water_idx = int(node.find('Secondary').text)


    walls = ReshapeVariantLayers(wall_layers)
    waters = ReshapeVariantLayers(water_layers)
    grounds = ReshapeVariantLayers(ground_layers)

    wall_durations = ReshapeVariantLayers(wall_layer_durations)
    water_durations = ReshapeVariantLayers(water_layer_durations)
    ground_durations = ReshapeVariantLayers(ground_layer_durations)

    ConvertToDtefType(outDir + "Wall", wall_idx, walls, wall_durations)
    ConvertToDtefType(outDir + "Secondary", water_idx, waters, water_durations)
    ConvertToDtefType(outDir + "Floor", ground_idx, grounds, ground_durations)



def ConvertLayer(outDir, name, tileset, paletteset, anim_durs, layers, totalIndex):
    #print(outDir)
    has_items = False
    for layer_index in range(len(layers)):
        layer = layers[layer_index]
        ##get variant numbers
        variants = 0
        for tier in range(len(layer)):
            if HasData(tileset, layer[tier]):
                variants = tier + 1
        
        if variants > 0:
            if not os.path.exists(outDir):
                os.makedirs(outDir)
            has_items = True
            
            ##get all animation recolors
            palette = []
            frame_wait = 0
            ##search through the elements in anim_durs and take the first index that isn't zero
            for slot in layer:
                if anim_durs[slot] > 0:
                    frame_wait = anim_durs[slot]
                    
                    palette_variant = 0
                    for palette_index in range(0, slot):
                        if anim_durs[palette_index] > 0:
                            palette_variant = palette_variant + 1
                    
                    palette_size = (paletteset.size[1] - 1) // (PALETTE_SIZE + 1)
                    end_palette = False
                    for row in range(palette_size):
                        recolor = []
                        for col in range(TOTAL_COLORS):
                            color = paletteset.getpixel(((col + TOTAL_COLORS * palette_variant) * (PALETTE_SIZE + 1) + 1, row * (PALETTE_SIZE + 1) + 1))
                            if color[3] > 0:
                                recolor.append(color)
                            else:
                                end_palette = True
                                break
                        if end_palette:
                            break
                        else:
                            palette.append(recolor)
                    
                    break
            if len(palette) == 0:
                palette.append([])
            
            outImg = Image.new('RGBA', (TILE_SIZE * len(palette) * variants, TILE_SIZE * TOTAL_TILES), (0, 0, 0, 0))
            for variant in range(variants):
                for tile in range(TOTAL_TILES):
                    tile_xy = (layer[variant] * 3 + TILE_MAP[tile][0], TILE_MAP[tile][1])
                    if not CheckBlank(tileset, (tile_xy[0] * (TILE_SIZE + 1), tile_xy[1] * (TILE_SIZE + 1))):
                        imgCrop = tileset.crop((tile_xy[0] * (TILE_SIZE + 1), tile_xy[1] * (TILE_SIZE + 1), \
                            tile_xy[0] * (TILE_SIZE + 1) + TILE_SIZE,tile_xy[1] * (TILE_SIZE + 1) + TILE_SIZE))
                        for anim in range(len(palette)):
                            ##recolor
                            imgRecolor = Recolor(imgCrop, palette, anim)
                            outImg.paste(imgRecolor, ((variant * len(palette) + anim) * TILE_SIZE, tile * TILE_SIZE))
            ##also include the frame rate of the layer
            outImg.save(os.path.join(outDir, name + '-' + format(layer_index, '02d') + '-' + format(frame_wait, '02d') + '-' + format(variants, '02d') + '.png'))
            
            
    return has_items

def ConvertTileset(inDir, outDir, fileName, totalIndex):
    '''
    Takes in a generic-rip PMD tileset folder, containing all tile combinations and palettes.
    Converts it to PMDO-input tileset folder, consisting of all tile combinations in all animation (palette) frames
    :param inDir: parent folder to read from
    :param outDir: parent folder to write to
    :param fileName: folder name
    :param totalIndex: current number of folders processed
    :return: new number of folders processed
    '''
    if '-' not in fileName:
        return totalIndex
    tileName = fileName.split('-')[1]
    ##each tileset has the following, and will be in the following bin:
    ##Wall, Wall Mask, Wall Alt 1, Wall Alt 1 Mask, Wall Alt 2, Wall Alt 2 Mask, Ground, Ground Alt 1, Ground Alt 2, Water, Sparkle
    ##wall, wall mask, wall,       wall mask,        wall,      wall mask,       ground,  ground,       ground,      water,  sparkle
    ##aka, 5 tilesets in total, at max
    ##the 2 wall will become their own folder
    ##the 1 ground will become its own folder
    ##the 2 water will become their own folder
    ##skip if that tile does not exist
    anim_durs = [0,0,0,0,0,0,0,0,0,0,0,0]
    tileset = None
    paletteset = None
    for file in os.listdir(inDir):
        if file.lower().startswith('layer'):
            ##get the layer image
            tileset = Image.open(os.path.join(inDir, file))
            tileset = tileset.convert("RGBA")
        elif file.lower().startswith('palette'):
            ##get the palette image
            name, ext = os.path.splitext(file)
            durs = name.split('-')
            if len(durs) != len(anim_durs) and len(durs) != len(anim_durs) + 1:
                raise Exception('Incorrect palette length')
            for dur in range(1, len(durs)):
                anim_durs[dur-1] = int(durs[dur])
            paletteset = Image.open(os.path.join(inDir, file))
            paletteset = paletteset.convert("RGBA")
    if tileset == None:
        print('EMPTY FILE: ' + tileName)
        return totalIndex
    print(tileName)

    ##try to save wall variants in a folder with suffix "wall"
    has_wall = ConvertLayer(os.path.join(outDir, format(totalIndex, '03d') + '-' + tileName), 'wall', tileset, paletteset, anim_durs, [[0,2,4],[1,3,5]], totalIndex)
    ##try to save ground variants in a folder with suffix "ground"
    has_ground = ConvertLayer(os.path.join(outDir, format(totalIndex, '03d') + '-' + tileName), 'ground', tileset, paletteset, anim_durs, [[6,7,8]], totalIndex)
    ##try to save water variants in a folder with suffix "water"
    has_water = ConvertLayer(os.path.join(outDir, format(totalIndex, '03d') + '-' + tileName), 'water', tileset, paletteset, anim_durs, [[9],[10],[11]], totalIndex)
    if has_wall or has_ground or has_water:
        return totalIndex + 1
    else:
        return totalIndex


def ConvertAllToDtef(inDir, outDir):
    for file in os.listdir(inDir):
        print(file)
        ConvertToDtef(os.path.join(inDir, file), os.path.join(outDir, file))

def ConvertAllTilesets(inDir, outDir):
    totalIndex = 1
    for file in os.listdir(inDir):
        totalIndex = ConvertTileset(os.path.join(inDir, file), outDir, file, totalIndex)


# -*- coding: utf-8 -*-

from PIL import Image
import sys
import os
import re
import shutil
import math

TILE_SIZE = 24
TOTAL_TILES = 47
PALETTE_SIZE = 10
TOTAL_COLORS = 16

TILE_MAP = [(1,4),##x00
            (1,6),##x01
            (2,7),##x02
            (2,3),##x03
            (2,0),##x13
            (1,8),##x04
            (0,4),##x05
            (2,5),##x06
            (2,2),##x26
            (2,10),##x07
            (1,18),##x17
            (1,17),##x27
            (2,1),##x37
            (0,7),##x08
            (0,3),##x09
            (0,0),##x89
            (1,3),##x0A
            (1,9),##x0B
            (0,19),##x1B
            (1,19),##x8B
            (1,0),##x9B
            (0,5),##x0C
            (0,2),##x4C
            (0,10),##x0D
            (0,17),##x4D
            (0,18),##x8D
            (0,1),##xCD
            (1,11),##x0E
            (0,20),##x2E
            (1,20),##x4E
            (1,2),##x6E
            (1,7),##x0F
            (1,21),##x1F
            (1,22),##x2F
            (0,13),##x3F
            (0,22),##x4F
            (0,23),##x5F
            (1,12),##x6F
            (0,15),##x7F
            (0,21),##x8F
            (1,14),##x9F
            (1,23),##xAF
            (0,16),##xBF
            (2,13),##xCF
            (1,16),##xDF
            (1,15),##xEF
            (1,1)]##xFF

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

def ConvertAllTilesets(inDir, outDir):
    totalIndex = 1
    for file in os.listdir(inDir):
        totalIndex = ConvertTileset(os.path.join(inDir, file), outDir, file, totalIndex)


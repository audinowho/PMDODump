
from PIL import Image
import os
import re
import shutil

def shrinkSprites(inDir, outDir):
        os.chdir(inDir)
        fileList = os.listdir(inDir)
        for img in fileList:
                if os.path.isfile(img):
                        shrinkSprite(inDir, img, outDir)

def shrinkSprite(inDir, inFile, outDir):
    print(inDir + "/" + inFile)
    inImg = Image.open(inDir + "/" + inFile)
    inImg = inImg.convert('RGBA')
    width, height = inImg.size
    imgSize = inImg.resize((width/25, height/25), Image.NEAREST)
    imgSize.save(outDir + "/" + inFile.replace(".bmp",".png"))

def formatSprites(inDir):
        os.chdir(inDir)
        fileList = os.listdir(inDir)
        for img in fileList:
                if os.path.isfile(img):
                        clearSprite(inDir + "/" + img)
                        shadowSprite(inDir + "/" + img)
                        trimSprite(inDir + "/" + img)

def clearSprite(inDir):
      print("Convert: " + inDir)
      inImg = Image.open(inDir)
      imgFix = inImg.convert("RGBA")
      datas = imgFix.getdata()

      trans = datas[0]
      newData = []
      for item in datas:
          if item[0] == 255 and item[1] == 255 and item[2] == 255:
              newData.append((255, 255, 255, 0))
          else:
              newData.append(item)

      imgFix.putdata(newData)
      imgFix.save(inDir)

def shadowSprite(inDir):
      inImg = Image.open(inDir)
      inImg = inImg.convert('RGBA')
      width, height = inImg.size
      imgShadow = Image.new("RGBA", (width, height), (0,0,0,0))
      imgShadow.paste(inImg, (1,1), inImg)
      imgShadow.paste(inImg, (1,0), inImg)

      datas = inImg.getdata()

      trans = datas[0]
      newData = []
      for item in datas:
            if item[3] == 255:
                  newData.append((255, 255, 255, 255))
            else:
                  newData.append(item)

      inImg.putdata(newData)
      imgShadow.paste(inImg, (0,0), inImg)

      imgShadow.save(inDir)

def trimSprite(inDir):
      inImg = Image.open(inDir)
      inImg = inImg.convert('RGBA')
      width, height = inImg.size
      

      pixels = inImg.load()
      pixelStart = 0
      
      for i in range(width):
            foundPixel = False
            for j in range(height):
                  if (pixels[i,j][3] != 0):
                        foundPixel = True
                        pixelStart = i
                        break
            if foundPixel:
                  break;

      for i in range(width-1, 0, -1):
            foundPixel = False
            for j in range(height):
                  if (pixels[i,j][3] != 0):
                        foundPixel = True
                        pixelEnd = i+1
                        break
            if foundPixel:
                  break;

      imgTrim = inImg.crop((pixelStart, 0, pixelEnd, height))
      
      imgTrim.save(inDir)


def createCells(inDir, outDir, tileSize):
        
        os.chdir(inDir)
        fileList = os.listdir(inDir)
        intList = []
        smallest = -1
        largest = -1
        for img in fileList:
                if os.path.isfile(img):
                        intImg = int(re.sub(r'(\d+).png',r'\1',img,0,re.I))
                        
                        if intImg < smallest or smallest == -1:
                                smallest = intImg
                        if intImg > largest:
                                largest = intImg
                        intList.append(intImg)
        intList.sort()
        
        fullWidth = 16 * tileSize[0]
        heightCells = (largest - smallest + 1)
        fullHeight = heightCells
        if (fullHeight % 16 == 0):
                fullHeight = fullHeight / 16 * tileSize[1]
        else:
                fullHeight = (fullHeight / 16 + 1) * tileSize[1]

        fullImg = Image.new( 'RGBA', (fullWidth,fullHeight), (0,0,0,255))

        cellImg = Image.new( 'RGBA', (tileSize[0]-2, tileSize[1]-2), (128,128,128,255))

        for i in range(16):
                for j in range(heightCells):
                        fullImg.paste(cellImg, (i*tileSize[0]+1,j*tileSize[1]+1))
        
        count = 0
        for img in intList:
                inImg = Image.open(inDir + r'\\' + str(img) + r'.png')
                inImg = inImg.convert('RGBA')
                width, height = inImg.size
                
                x = ((img-smallest) % 16) * tileSize[0]
                y = ((img-smallest) / 16) * tileSize[1]

                redImg = Image.new( 'RGBA', (width+2, height+2), (255,0,0,255))
                
                
                fullImg.paste(redImg, (x+1,y+1))
                fullImg.paste(inImg, (x+2,y+2))

                #if width < tileSize - 2:
                #        windowImg = Image.new( 'RGBA', (tileSize - width - 2, height ), (0,0,0,0))
                #        fullImg.paste(windowImg, (x+width+1,y))

        fullImg.save(outDir + "/" + str(smallest) + ".png")
                  
def CheckBlank(img, start, size):
    datas = img.getdata()
    for i in range(size):
        for j in range(size):
            index = (i + start[0]) + (j + start[1]) * img.size[0]
            #print(str(datas[0]))
            if datas[index][3] != 0:
                return False
    return True
      
def GetFontSize(img, start, size):
    width = 0
    height = 0
    datas = img.getdata()
    for i in range(size-1):
        index = (i + 1 + start[0]) + (1 + start[1]) * img.size[0]
        if datas[index] != (255,0,0,255):
            width = i
            break
        
    for j in range(size-1):
        index = (1 + start[0]) + (j + 1 + start[1]) * img.size[0]
        if datas[index] != (255,0,0,255):
            height = j
            break
    ##print(str(width) + " " + str(height))
    return (width, height)
    
def divSheet(inDir):
      if not os.path.isdir(os.path.join(inDir, 'out')):
          os.makedirs(os.path.join(inDir, 'out'))
      startIndex = 0
      size = 16
      row = 10
      inImg = Image.open(os.path.join(inDir, "in.png"))
      inImg = inImg.convert('RGBA')
      width, height = inImg.size
      for x in range(0, width // size):
          for y in range(0, height // size):
              charIndex = startIndex + y * row + x
              if not CheckBlank(inImg, (x * size, y * size), size):
                  if not os.path.isdir(os.path.join(inDir, 'out', str(charIndex))):
                      os.makedirs(os.path.join(inDir, 'out', str(charIndex)))
                  imgNew = inImg.crop((x * size, y * size, x * size + size, y * size + size))
                  imgNew.save(os.path.join(inDir, 'out', str(charIndex), 'None.png'))
                  
def divRectSheet(inDir):
      if not os.path.isdir(os.path.join(inDir, 'out')):
          os.makedirs(os.path.join(inDir, 'out'))
      
      for inFile in os.listdir(inDir):
          imgStart, ext = os.path.splitext(inFile)
          if ext == '.png':
              startIndex = int(imgStart)
              inImg = Image.open(os.path.join(inDir, inFile))
              inImg = inImg.convert('RGBA')
              width, height = inImg.size
              size = width // 16
              for x in range(0, width // size):
                  for y in range(0, height // size):
                      charIndex = startIndex + y * 16 + x
                      rect = GetFontSize(inImg, (x * size, y * size), size)
                      if rect[0] > 0 and rect[1] > 0:
                          imgNew = inImg.crop((x * size + 2, y * size + 2, x * size + rect[0], y * size + rect[1]))
                          imgNew.save(os.path.join(inDir, 'out', str(charIndex) + '.png'))


def foundImg(inImg, firstWidth, x):    
    data = inImg.getdata()
    
    for i in range(0, firstWidth):
        for j in range(0, inImg.size[1]):
            if data[i + j * inImg.size[0]] != data[(x + i) + j * inImg.size[0]]:
                return False
    return True

def detectRowSheet(inDir, firstWidth):
      if not os.path.isdir(os.path.join(inDir, 'out')):
          os.makedirs(os.path.join(inDir, 'out'))
      
      for inFile in os.listdir(inDir):
          imgStart, ext = os.path.splitext(inFile)
          if ext == '.png':
              startIndex = int(imgStart)
              inImg = Image.open(os.path.join(inDir, inFile))
              inImg = inImg.convert('RGBA')
              width, height = inImg.size
              #make a list of all x points where this character is found
              x_points = []
              x = 0
              while x < width - firstWidth + 1:
                  if foundImg(inImg, firstWidth, x):
                      x = x + firstWidth
                      x_points.append(x)
                  x = x + 1
              for pt in range(0, len(x_points)-1):
                  charIndex = startIndex + pt
                  rect = (x_points[pt], 0, x_points[pt+1]-firstWidth, height)
                  imgNew = inImg.crop(rect)
                  imgNew.save(os.path.join(inDir, 'out', str(charIndex) + '.png'))

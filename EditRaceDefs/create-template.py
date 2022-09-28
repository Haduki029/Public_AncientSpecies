'''
Defsのxmlファイルを取り込む。
引数として対象のフォルダ指定。

'''

import sys
import glob
import re
import os

import xml.etree.ElementTree as ET

def getFilesList(folderName):
    # folderName = '../1.3/Defs/'
    file_path_list = glob.glob("{}/**/*.xml".format(folderName), recursive=True)
    return (file_path_list)

def copyFiles(filePathList):
    
    for filePath in filePathList:
        tree = ET.parse(filePath)
        # ファイルパスについてDefsの前をトリミング
        targetStr = "Defs"
        idx = filePath.find(targetStr)
        deleteStr = filePath[:idx]
        filePath = filePath.replace(deleteStr, "")
        
        fileName = "../DefsTemplate/" + filePath
        # フォルダ有無チェックのためフォルダパス取得
        folderPath = os.path.dirname(fileName)
        
        # 保存先フォルダが無い場合に作成する。
        os.makedirs(folderPath, exist_ok=True)

        # xmlファイルとして出力
        with open(fileName, "wb") as file:
            tree.write(file, encoding='utf-8', xml_declaration=True)

if __name__ == '__main__':
    args = sys.argv
    if 2 <= len(args):
       file_path_list = getFilesList(args[1])
       copyFiles(file_path_list)
    else:
        print('Arguments are too short')
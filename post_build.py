import glob, os, sys
import shutil
import argparse

def make_folder_if_not_exist(directory):
  if os.path.exists(directory):
    shutil.rmtree(directory)
  os.makedirs(directory)

def GetLicense(x):
  result = {
    'My': "License",
    'Sonic': "../../../SonicZentropyLicense"
  }.get(x, None)
  if result:
    result = "%s.txt" % result
  return result

parser = argparse.ArgumentParser(description='Perform post-build actions')
parser.add_argument('--ModName', metavar='M', type=str, nargs=1, help='The name of the mod')
parser.add_argument('--TargetDir', metavar='T', type=str, nargs=1, help="The dir of the compiled results")
parser.add_argument('--License', metavar='L', type=str, nargs="+", help="The license the mod is released under")
parser.add_argument('--ExtraFiles', metavar='F', type=str, nargs="*", help="Any and all extra files")

args = parser.parse_args()

ModName = args.ModName[0]
TargetDir = args.TargetDir[0]
License = args.License[0] if args.License != None else None
ExtraFiles = args.ExtraFiles

readme = "Readme.txt"
changeLog = "ChangeLog.txt"
info = "info.json"

licenseToAdd = GetLicense(License)

Files = [readme, changeLog, info]
if licenseToAdd != None:
  Files.append(licenseToAdd)
Files.append("%s.dll" % ModName)
if ExtraFiles != None and len(ExtraFiles) > 0:
  for file in ExtraFiles:
    Files.append(file)

make_folder = "%s%s\%s" % (TargetDir, ModName, ModName)
make_folder_if_not_exist(make_folder)

for mod_file in Files:
  from_path = "%s%s" % (TargetDir, mod_file)
  to_path = "%s%s\%s\." % (TargetDir, ModName, ModName)
  shutil.copy(from_path, to_path)

mod_folder = "%s%s" % (TargetDir, ModName)
zip_file_name = "%s.zip" % (mod_folder)
if os.path.isfile(zip_file_name):
  os.remove(zip_file_name)
os.system("izarcc -a -r -p -cx %s %s\\" % (zip_file_name, mod_folder))

result_dir = "..\..\..\Result"
if not os.path.exists(result_dir):
  os.makedirs(result_dir)

shutil.copy(zip_file_name, "%s\\." % result_dir)

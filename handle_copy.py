import glob, os, sys
from subprocess import check_output
import shutil

def make_folder_if_not_exist(directory):
  if os.path.exists(directory):
    shutil.rmtree(directory)

  os.makedirs(directory)

if len(sys.argv) == 0:
  exit()

if sys.argv[1] == "GOG":
  directory="D:\\Program Files (x86)\\GOG Galaxy\\Games\\Pillars of Eternity II Deadfire\\Mods\\"
else:
  directory="D:\\Program Files (x86)\\Steam\\steamapps\\common\\Pillars of Eternity II\\Mods\\"

os.chdir(sys.argv[1])
for f in glob.glob("*.zip"):
  result = os.path.splitext(f)[0]

  mod_dir = directory + result
  make_folder_if_not_exist(mod_dir)

  tempDir = ".\\temp"
  if os.path.exists(tempDir):
    shutil.rmtree(tempDir)
  os.makedirs(tempDir)

  shutil.copyfile(f, tempDir + "\\t.zip")
  os.chdir(tempDir)

  check_output("izarce -e *.zip", shell=True)
  os.remove("t.zip")

  for filename in glob.glob("*"):
    shutil.copy(filename, mod_dir)
  os.chdir("..")

  shutil.rmtree(tempDir)
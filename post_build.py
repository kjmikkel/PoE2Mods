import glob, os, sys
import shutil
import argparse

def make_folder_if_not_exist(directory):
  if os.path.exists(directory):
    shutil.rmtree(directory)

  os.makedirs(directory)

parser = argparse.ArgumentParser(description='Perform post-build actions')
parser.add_argument('Project', metavar='P', type=str, nargs=1, help='The project dir')
parser.add_argument('Target', metavar='T', type=str, nargs=1, help='The target directory for the build')
parser.add_argument('ModName', metavar='M', type=str, nargs=1, help='The name of the mod')
parser.add_argument('Files', metavar='F', type=str, nargs='+', help='The files, besides the pw.dll, that are to be copied')

args = parser.parse_args()

Project = args.Project[0]
Target = args.Target[0]
ModName = args.ModName[0]
Files = args.Files
Files.append("%s.pw.dll" % ModName)

make_folder = "%s%s\Mods\%s" % (Target, ModName, ModName)
make_folder_if_not_exist(make_folder)

for mod_file in Files:
  from_path = "%s%s" % (Target, mod_file)
  print(from_path)
  to_path = "%s%s\Mods\%s\." % (Target, ModName, ModName)
  shutil.copy(from_path, to_path)

mod_folder = "%s%s" % (Target, ModName)
zip_file_name = "%s.zip" % (mod_folder)
if os.path.isfile(zip_file_name):
  os.remove(zip_file_name)
os.system("izarcc -a -r -p -cx %s %s\\" % (zip_file_name, mod_folder))

result_dir = "%s..\Result" % Project
if not os.path.exists(result_dir):
  os.makedirs(result_dir)

shutil.copy(zip_file_name, "%s\\." % result_dir)

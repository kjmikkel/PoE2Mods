import os
from shutil import copyfile
import argparse


def create_from_and_to(TargetDir, GameModFolder, ModName, file):
    return ["%s\%s" % (TargetDir, file), "%s\%s\%s" % (GameModFolder, ModName, file)]


# Make the argument parser
parser = argparse.ArgumentParser(description="Perform copy operations")
parser.add_argument(
    "--ModName", metavar="M", type=str, nargs=1, help="The name of the mod"
)
parser.add_argument(
    "--TargetDir",
    metavar="T",
    type=str,
    nargs=1,
    help="The dir of the compiled results",
)
parser.add_argument(
    "--GameModFolder",
    metavar="G",
    type=str,
    nargs=1,
    help="The path to the game directory",
)

# Parse the arguments
args = parser.parse_args()
ModName = args.ModName[0]
TargetDir = args.TargetDir[0]
GameModFolder = (
    args.GameModFolder[0]
    if args.GameModFolder != None and len(args.GameModFolder) > 0
    else None
)

# Find the path to the game folder
if GameModFolder == None:
    GameModFolder = (
        "C:\Program Files (x86)\GOG Galaxy\Games\Pillars of Eternity II Deadfire\Mods"
    )

# If we cannot find the game folder we can do nothing
if not os.path.exists(GameModFolder):
    exit(0)

# Copy the files
files_to_copy = []
files_to_copy.append(
    create_from_and_to(TargetDir, GameModFolder, ModName, "%s.dll" % ModName)
)
files_to_copy.append(create_from_and_to(TargetDir, GameModFolder, ModName, "info.json"))

path_name = "%s\%s" % (GameModFolder, ModName)

if not os.path.exists(path_name):
    os.mkdir(path_name)

for file_copy in files_to_copy:
    print("%s %s" % (file_copy[0], file_copy[1]))
    copyfile(file_copy[0], file_copy[1])

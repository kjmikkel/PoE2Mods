DEL /q GOG\.
DEL /q Steam\.
DEL /q Result\.

REM We need to get to use the correct command line utility
CALL "D:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"

CALL GOG.bat
msbuild /t:rebuild /m
COPY result\. GOG\.
python handle_copy.py GOG

CALL Steam.bat
msbuild /t:rebuild /m
COPY result\. Steam\.
python handle_copy.py Steam
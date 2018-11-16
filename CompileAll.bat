DEL /q GOG\.
DEL /q Steam\.
DEL /q Result\.

CALL GOG.bat
START /w "D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat" msbuild /t:rebuild /m
COPY result\. GOG\.

CALL Steam.bat
START /w "D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat" msbuild /t:rebuild /m
COPY result\. Steam\.
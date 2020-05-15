DEL /q Result\.

REM We need to get to use the correct command line utility
CALL "D:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"

msbuild /t:Clean
msbuild /t:rebuild /m
@ECHO OFF
REM Save the current working directory (i.e. where this file is right now)
SET CURRENT=%cd%
REM Path to the OpenAssemblyCreator
SET OPEN_ASSEM=%CURRENT%\..\PoE2Mods.pw\OpenAssemblyCreator\bin\Release\OpenAssemblyCreator.exe
REM The path to the ..\Pillars of Eternity II Deadfire\PillarsOfEternityII_Data\Managed directory - can be absolute or relative
SET SRC="D:\Program Files (x86)\GOG Galaxy\Games\Pillars of Eternity II Deadfire\PillarsOfEternityII_Data\Managed"
REM The path to where you want to save the now opened assembly - say "C:\Assembly-CSharp.dll" - can be either absolute or relative
SET TGT="E:\Users\Spil\Documents\Programming\Deadfire\PoE2Mods\Packages\Assembly-CSharp.dll"

REM Go to the directory where the Assembly-CSharp.dll file is found
cd /D %SRC%

REM Call the OpenAssemblyCreator.exe on the Assembly-CSharp.dll file 
%OPEN_ASSEM% Assembly-CSharp.dll %TGT%

REM Make a backup of the original file, so it can be accessed later, in case something goes wrong during testing
set month=%date:~4,2%
if "%month:~0,1%" == " " set month=0%month:~1,1%
echo month=%month%
set day=%date:~0,2%
if "%day:~0,1%" == " " set day=0%day:~1,1%
echo day=%day%
set hour=%time:~0,2%
if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
echo hour=%hour%
set min=%time:~3,2%
if "%min:~0,1%" == " " set min=0%min:~1,1%
echo min=%min%
set secs=%time:~6,2%
if "%secs:~0,1%" == " " set secs=0%secs:~1,1%
echo secs=%secs%

set datetimef=%year%%month%%day%_%hour%_%min%_%secs%

REM Perform the actual copying
@ECHO ON
COPY Assembly-CSharp.dll %TGT%_%datetimef%
@ECHO OFF

REM Go back to the original directory
cd /D %CURRENT%
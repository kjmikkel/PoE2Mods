@ECHO OFF 
SET CURRENT=%cd%
SET OPEN_ASSEM=%CURRENT%\..\PoE2Mods.pw\OpenAssemblyCreator\bin\Release\OpenAssemblyCreator.exe
SET TGT="E:\Users\Spil\Documents\Programming\Deadfire\PoE2Mods\Packages\Assembly-CSharp.dll"
SET SRC="D:\Program Files (x86)\GOG Galaxy\Games\Pillars of Eternity II Deadfire\PillarsOfEternityII_Data\Managed"

cd /D %SRC%

%OPEN_ASSEM% Assembly-CSharp.dll %TGT%

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

@ECHO ON
COPY Assembly-CSharp.dll %TGT%_%datetimef%
@ECHO OFF

cd /D %CURRENT%
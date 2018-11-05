@ECHO OFF
REM Save the current working directory (i.e. where this file is right now)
SET CURRENT=%cd%
REM Path to the OpenAssemblyCreator
SET OPEN_ASSEM=%CURRENT%\..\PoE2Mods.pw\OpenAssemblyCreator\bin\Release\OpenAssemblyCreator.exe
REM The path to the ..\Pillars of Eternity II Deadfire\PillarsOfEternityII_Data\Managed directory - can be absolute or relative
SET SRC=%1
REM The path to where you want to save the now opened assembly - say "C:\Assembly-CSharp.dll" - can be either absolute or relative
SET TGT="E:\Users\Spil\Documents\Programming\Deadfire\PoE2Mods\Packages\Assembly-CSharp.dll"

REM Go to the directory where the Assembly-CSharp.dll file is found
cd /D %SRC%

REM Call the OpenAssemblyCreator.exe on the Assembly-CSharp.dll file 
%OPEN_ASSEM% Assembly-CSharp.dll %TGT%
@ECHO OFF
REM Make a backup of the original file, so it can be accessed later, in case something goes wrong during testing
::: Begin set date

for /f "tokens=1-4 delims=/-. " %%i in ('date /t') do (call :set_date %%i %%j %%k %%l)
goto :end_set_date

:set_date
if "%1:~0,1%" gtr "9" shift
for /f "skip=1 tokens=2-4 delims=(-)" %%m in ('echo,^|date') do (set %%m=%1&set %%n=%2&set %%o=%3)
goto :eof

:end_set_date
::: End set date

REM Set time
For /f "tokens=1-3 delims=/:" %%a in ("%TIME%") do (set mytime=%%a_%%b_%%c)
For /f "tokens=1 delims=," %%a in ("%mytime%") do (set local_time=%%a)

set datetimef=%yy%_%mm%_%dd%_%local_time%
set datetimef=%datetimef: =0%

REM Perform the actual copying
@ECHO ON
COPY Assembly-CSharp.dll %TGT%_%datetimef%
@ECHO OFF

REM Go back to the original directory
cd /D %CURRENT%
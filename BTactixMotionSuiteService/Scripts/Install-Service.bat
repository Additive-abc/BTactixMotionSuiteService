@echo off
REM ---------------------------------------------------
REM Install BTactix Motion Windows Service
REM ---------------------------------------------------

SET SERVICE_NAME=BTactixMotionService
SET DISPLAY_NAME=B-Tactix Motion Service

REM Get the folder of the batch file
SET SCRIPT_DIR=%~dp0
REM Parent folder contains the EXE and DLLs
SET BUILD_DIR=%SCRIPT_DIR%..

REM Find the EXE automatically
FOR %%f IN ("%BUILD_DIR%\*.exe") DO SET SERVICE_EXE=%%f

IF NOT EXIST "%SERVICE_EXE%" (
    ECHO ERROR: Could not find EXE in %BUILD_DIR%
    PAUSE
    EXIT /B 1
)

ECHO Installing service from: %SERVICE_EXE%
REM Remove old service if exists
sc query "%SERVICE_NAME%" >nul 2>&1
IF %ERRORLEVEL%==0 (
    ECHO Service exists. Stopping and deleting...
    sc stop "%SERVICE_NAME%"
    sc delete "%SERVICE_NAME%"
    TIMEOUT /T 2 /NOBREAK >nul
)

REM Create the service
sc create "%SERVICE_NAME%" binPath= "\"%SERVICE_EXE%\"" DisplayName= "%DISPLAY_NAME%" start= auto
ECHO Service installed.

REM Start the service
sc start "%SERVICE_NAME%"
ECHO Service started.
PAUSE

@echo off
REM ---------------------------------------------------
REM Uninstall BTactix Motion Windows Service
REM ---------------------------------------------------

SET SERVICE_NAME=BTactixMotionService

sc query "%SERVICE_NAME%" >nul 2>&1
IF %ERRORLEVEL%==0 (
    ECHO Stopping service...
    sc stop "%SERVICE_NAME%"
    ECHO Deleting service...
    sc delete "%SERVICE_NAME%"
    ECHO Service removed.
) ELSE (
    ECHO Service does not exist.
)
PAUSE

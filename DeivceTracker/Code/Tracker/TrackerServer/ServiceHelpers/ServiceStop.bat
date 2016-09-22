@echo off
set ServiceName=TrackerService
set ServiceFileName=TrackerServer
Title Installing %ServiceName% on %computername%

echo ===================================================
echo Running windows service - %ServiceName%
echo ===================================================
echo.
net stop %ServiceFileName%
echo ---------------------------------------------------
echo.
pause
@echo off
set ServiceName=TrackerService
set ServiceFileName=TrackerServer
Title Installing %ServiceName% on %computername%

echo ===================================================
echo Installing windows service - %ServiceName%
echo ===================================================
echo.

REM The following directory is for .NET 4.0
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%

echo.
echo ---------------------------------------------------
InstallUtil /i %~dp0%ServiceFileName%.exe
echo ---------------------------------------------------
echo.
pause
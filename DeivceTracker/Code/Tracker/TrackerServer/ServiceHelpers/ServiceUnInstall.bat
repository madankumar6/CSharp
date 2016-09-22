@echo off
set ServiceFileName=TrackerServer
Title Uninstalling %ServiceFileName% on %computername%

echo ===================================================
echo Uninstalling windows service - %ServiceFileName%
echo ===================================================
echo.

REM The following directory is for .NET 4.0
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%

echo.
echo ---------------------------------------------------
InstallUtil /u %~dp0%ServiceFileName%.exe
echo ---------------------------------------------------
echo.
pause
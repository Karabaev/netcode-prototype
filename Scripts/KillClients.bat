@echo off
setlocal

echo Kill launched processes %%i
taskkill /F /IM Motk.Client.exe

endlocal
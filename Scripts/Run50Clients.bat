@echo off
setlocal

for /L %%i in (1,1,50) do (
    echo Launch %%i
    start "" "../../Builds/Motk.Client/Motk.Client.exe" "-batchmode" "-nographics"
    timeout /t 1 /nobreak
)

timeout /t 300 /nobreak

echo Kill launched processes %%i
taskkill /F /IM Motk.Client.exe

endlocal
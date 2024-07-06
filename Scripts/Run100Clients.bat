@echo off
setlocal

for /L %%i in (1,1,100) do (
    echo Launch %%i
    start "test" "E:\Projects\Builds\Motk.Client\Motk.exe" "-batchmode" "-nographics"
    timeout /t 1 /nobreak
)

timeout /t 120 /nobreak

echo Kill launched processes %%i
taskkill /F /IM Motk.exe

endlocal
set FOLDER=%HOMEDRIVE%\inetpub\wwwroot

rem Grant full control to the folder
icacls %FOLDER% /grant %USERNAME%:F

if exist %FOLDER% (
  rd /s /q "%FOLDER%"
)

mkdir %FOLDER%

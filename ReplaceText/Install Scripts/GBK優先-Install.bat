@ECHO oFF

echo 複製執行檔到 %windir%\
copy ReplaceText.* %windir%\

echo.
echo 套用 GBK優先-Everyhing to UTF8 - 安裝.reg
regedit /S "GBK優先-Everyhing to UTF8 - 安裝.reg"

echo.
echo 套用 GBK優先-Everyhing to UTF8 (test) - 安裝.reg
regedit /S "GBK優先-Everyhing to UTF8 (test) - 安裝.reg"

echo.
echo 安裝完成

pause
:timeout /t 3
@ECHO oFF

echo 刪除機碼

echo.
echo 套用 Everyhing to UTF8 - 解除.reg
regedit /S "Everyhing to UTF8 - 解除.reg"

echo.
echo 套用 Everyhing to UTF8 (test) - 解除.reg
regedit /S "Everyhing to UTF8 (test) - 解除.reg"

echo.
echo 刪除 %windir%\ReplaceText.*

del /q %windir%\ReplaceText.*

echo.
echo 解除安裝完成

pause
:timeout /t 5
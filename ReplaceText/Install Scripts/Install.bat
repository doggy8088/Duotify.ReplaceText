@ECHO oFF

echo �ƻs�����ɨ� %windir%\
copy ReplaceText.* %windir%\

echo.
echo �M�� Everyhing to UTF8 - �w��.reg
regedit /S "Everyhing to UTF8 - �w��.reg"

echo.
echo �M�� Everyhing to UTF8 (test) - �w��.reg
regedit /S "Everyhing to UTF8 (test) - �w��.reg"

echo.
echo �w�˧���

pause
:timeout /t 3
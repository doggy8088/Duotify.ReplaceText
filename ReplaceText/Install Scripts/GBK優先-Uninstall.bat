@ECHO oFF

echo �R�����X

echo.
echo �M�� GBK�u��-Everyhing to UTF8 - �Ѱ�.reg
regedit /S "GBK�u��-Everyhing to UTF8 - �Ѱ�.reg"

echo.
echo �M�� GBK�u��-Everyhing to UTF8 (test) - �Ѱ�.reg
regedit /S "GBK�u��-Everyhing to UTF8 (test) - �Ѱ�.reg"

echo.
echo �R�� %windir%\ReplaceText.*

del /q %windir%\ReplaceText.*

echo.
echo �Ѱ��w�˧���

pause
:timeout /t 5
@echo off

REM Run
REM for /f %%f IN ( 'dir /b /s /A:-D-L-S-H' ) DO ReplaceText.exe %%f 

REM Run & Show Full Path
REM for /f %%f IN ( 'dir /b /s /A:-D-L-S-H' ) DO ReplaceText.exe %%f /FullPath

REM Test Run
REM for /f %%f IN ( 'dir /b /s /A:-D-L-S-H' ) DO ReplaceText.exe %%f /T 

REM Test Run & Show Full Path
REM for /f %%f IN ( 'dir /b /s /A:-D-L-S-H' ) DO ReplaceText.exe %%f /T /FullPath

REM Test Run & Show Full Path & 一併修正文字檔案 (*.txt, *.csv)
for /f %%f IN ( 'dir /b /s /A:-D-L-S-H' ) DO ReplaceText.exe %%f /T /FullPath /M


REM Test Run & Verbose Output
REM for /f %%f IN ( 'dir /b /s /A:-D-L-S-H' ) DO ReplaceText.exe %%f /T /V

pause
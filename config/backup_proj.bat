cd D:\
D:
set DATA_YMD=%date:~0,4%%date:~5,2%%date:~8,2%
xcopy S1 S1_运维备份\%DATA_YMD%\
xcopy S2 S2_运维备份\%DATA_YMD%\
pause
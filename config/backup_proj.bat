cd D:\
D:
set DATA_YMD=%date:~0,4%%date:~5,2%%date:~8,2%
xcopy S1 S1_��ά����\%DATA_YMD%\
xcopy S2 S2_��ά����\%DATA_YMD%\
pause
cd "C:\Program Files\SmartQuant Ltd\OpenQuant 2014"
C:

start OpenQuant.exe --file="D:\GitHub\S1\S1.sln" --id=100 --run
ping 127.0.0.1 -n 20
start OpenQuant.exe --file="D:\GitHub\S2\S2.sln" --id=200 --run
ping 127.0.0.1 -n 20

pause

# QuantBox.APIProvider
OpenQuant2014的行情交易插件，使用XAPI统一接口

## 特殊功能
命令启停OQ功能

1. 启动OQ，并打开指定策略，并运行
通过**命令行**传入参数
- --file: 策略绝对路径。如果空格必须用引号
- --run: 运行策略。否则只打开策略
- --id: 识别码。用于监控剪贴板时过滤命令

```
cd "C:\Program Files\SmartQuant Ltd\OpenQuant 2014"
C:
start OpenQuant.exe --file="D:\Users\Kan\Documents\OpenQuant 2014\Solutions\SMACrossover\SMACrossover.sln" --id=100 --run
```

2. 停止策略，并退出程序
通过**剪贴板**传入参数。
- --id: 识别码。用于监控剪贴板时过滤命令
- --stop: 停止策略。必须先停止才能退出
- --exit: 退出程序。
```
echo --id=100 --stop --exit | clip
```
只要向剪贴板复制`--id=100 --stop --exit`即可，这个复制可以手工实现，也可以灵活使用管道符|将echo的回显重定向到剪贴板clip
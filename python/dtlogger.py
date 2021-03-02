#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
pip install spyne

<targets>
    <target type='WebService'
        name='ws'
        url='http://127.0.0.1:8000/logme'
        protocol='HttpPost'
        encoding='UTF-8'>
        <parameter name='logger' type='System.String' layout='${logger}'/>
        <parameter name='level' type='System.String' layout='${level}'/>
        <parameter name='message' type='System.String' layout='${message}'/>
        <parameter name='title' type='System.String' layout='${event-context:item=title}'/>
    </target>
</targets>
<rules>
    <logger name="*" minlevel="Info" writeTo="ws" />
</rules>


protected NLog.Logger log_wx = NLog.LogManager.GetLogger("wx");

log_wx.Warn("策略启动。");

"""
from spyne import Application, rpc, ServiceBase, String
from spyne.protocol.http import HttpRpc
from spyne.protocol.json import JsonDocument
from spyne.server.wsgi import WsgiApplication

from DtalkRobot import *
from BatchDelayQueue import DelayQueue

# 99simnow
webhook_a99ctp = "https://oapi.dingtalk.com/robot/send?access_token=11111"
# 98实
webhook_a98ctp = "https://oapi.dingtalk.com/robot/send?access_token=11111"
# S01策略
webhook_s01 = "https://oapi.dingtalk.com/robot/send?access_token=11111"

# 不明的消息都发到模拟平台上
webhook = webhook_a99ctp

ip = '0.0.0.0'
port = 8000


def send_msgs(logger, level, title, message):
    # 利用logger,可以将消息发向不同的机器人
    if logger == 'ALL':
        _webhook = webhook
    elif logger == 'A98CTP.A':
        _webhook = webhook_a98ctp
    elif logger == 'A98CTP.P':
        _webhook = webhook_a98ctp
    elif logger == 'S01':
        _webhook = webhook_s01
    else:
        _webhook = webhook

    if title is None or len(title) == 0:
        robot.sendText_webhook(_webhook, message)
    else:
        msg = f"##### {title}\n{message}"
        robot.sendMarkdown_webhook(_webhook, title, msg)


def process_log_msg(tuples):
    msgs = {}
    for tp in tuples:
        logger, level, title, message = tp
        key = (logger, level, title)
        lst = msgs.get(key, [])
        lst.append(message)
        msgs[key] = lst
    print("定时处理", len(msgs))
    for k, v in msgs.items():
        # 这里不能太长，否则会被截断
        send_msgs(k[0], k[1], k[2], '\n'.join(v))


class LogService(ServiceBase):
    @rpc(String, String, String, String, _returns=String)
    def logme(ctx, logger, level, title, message):
        delayQueue.add((logger, level, title, message))
        return 'OK'


delayQueue = DelayQueue(10, process_log_msg)

application = Application([LogService],
                          tns='kan.logger.wechat',
                          in_protocol=HttpRpc(validator='soft'),
                          out_protocol=JsonDocument()
                          )

if __name__ == '__main__':
    # You can use any Wsgi server. Here, we chose
    # Python's built-in wsgi server but you're not
    # supposed to use it in production.
    from wsgiref.simple_server import make_server

    wsgi_app = WsgiApplication(application)
    server = make_server(ip, port, wsgi_app)

    robot = DtalkRobot(webhook)
    robot.sendText_webhook(webhook, "启动钉钉日志服务成功")

    # 不退出
    server.serve_forever()

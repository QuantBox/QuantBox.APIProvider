#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
钉钉群自定义机器人
author：疯狂的技术宅
github：https://github.com/magician000

学习python时做的练习，纯粹为了娱乐
如果存在bug请自行修改，不提供任何支持

官方文档
https://open-doc.dingtalk.com/docs/doc.htm?spm=a219a.7629140.0.0.z5MWoh&treeId=257&articleId=105735&docType=1

这个接口的消息格式命名风格不统一，坑爹呢？
所以不要迷信大公司就怎样规范。
"""

import urllib.parse
import urllib.request
import json
import time

# 代理，一般不使用
if False:
    proxy_support = urllib.request.ProxyHandler({'http': '192.168.1.60:808', 'https': '192.168.1.60:808'})
    # proxy_support = urllib.request.ProxyHandler({'sock5': '192.168.1.60:1080'})
    opener = urllib.request.build_opener(proxy_support)
    urllib.request.install_opener(opener)


# 自定义机器人的封装类
class DtalkRobot(object):
    def __init__(self, webhook):
        super(DtalkRobot, self).__init__()
        self.webhook = webhook

    # text类型
    def sendText_webhook(self, webhook, msg, isAtAll=False, atMobiles=[]):
        data = {"msgtype": "text", "text": {"content": msg}, "at": {"atMobiles": atMobiles, "isAtAll": isAtAll}}
        return self.post_webhook(webhook, data)

    # markdown类型
    def sendMarkdown_webhook(self, webhook, title, text):
        data = {"msgtype": "markdown", "markdown": {"title": title, "text": text}}
        return self.post_webhook(webhook, data)

    # link类型
    def sendLink(self, title, text, messageUrl, picUrl=""):
        data = {"msgtype": "link",
                "link": {"text": text, "title": title, "picUrl": picUrl, "messageUrl": messageUrl}}
        return self.post(data)

    # ActionCard类型
    def sendActionCard(self, actionCard):
        data = actionCard.getData()
        return self.post(data)

    # FeedCard类型
    def sendFeedCard(self, links):
        data = {"feedCard": {"links": links}, "msgtype": "feedCard"}
        return self.post(data)

    def post(self, data):
        post_data = json.JSONEncoder().encode(data).encode(encoding='UTF8')

        req = urllib.request.Request(self.webhook, post_data)
        req.add_header('Content-Type', 'application/json')
        content = urllib.request.urlopen(req).read().decode('UTF-8')
        return content

    def post_webhook(self, webhook, data):
        post_data = json.JSONEncoder().encode(data).encode(encoding='UTF8')

        req = urllib.request.Request(webhook, post_data)
        req.add_header('Content-Type', 'application/json')
        content = urllib.request.urlopen(req).read().decode('UTF-8')
        # print(content)
        return content


# ActionCard类型消息结构
class ActionCard(object):
    """docstring for ActionCard"""
    title = ""
    text = ""
    singleTitle = ""
    singleURL = ""
    btnOrientation = 0
    hideAvatar = 0
    btns = []

    def __init__(self, arg=""):
        super(ActionCard, self).__init__()
        self.arg = arg

    def putBtn(self, title, actionURL):
        self.btns.append({"title": title, "actionURL": actionURL})

    def getData(self):
        data = {"actionCard": {"title": self.title, "text": self.text, "hideAvatar": self.hideAvatar,
                               "btnOrientation": self.btnOrientation, "singleTitle": self.singleTitle,
                               "singleURL": self.singleURL, "btns": self.btns}, "msgtype": "actionCard"}
        return data


# FeedCard类型消息格式
class FeedLink(object):
    """docstring for FeedLink"""
    title = ""
    picUrl = ""
    messageUrl = ""

    def __init__(self, arg=""):
        super(FeedLink, self).__init__()
        self.arg = arg

    def getData(self):
        data = {"title": self.title, "picURL": self.picUrl, "messageURL": self.messageUrl}
        return data


if __name__ == "__main__":
    webhook = "https://oapi.dingtalk.com/robot/send?access_token=72f6252257f43f402f743b7d698fc21d21939717783a20689e1c1292c20d1903"
    robot = DtalkRobot(webhook)

    # print(robot.sendText("xxxx：[" + time.strftime('%Y-%m-%d %H:%M:%S', time.localtime(time.time())) + "]", False,
    #                      ["13912345678 "]))

    print(robot.sendMarkdown('报单回报', """# LAST_HEDGE (OI805)
1. 21:00:00.224/6298/0|-2/
1. 21:00:01.412/6296/0
## LAST_HEDGE (TA805)
1. 21:00:00.224/5692/0|-6/
1. 21:00:01.427/5690/0
### LAST_HEDGE (TA805)
- 21:00:00.224/5692/0|-6/
- 21:00:01.427/5690/0
#### LAST_HEDGE (TA805)
- 21:00:00.224/5692/0|-6/
- 21:00:01.427/5690/0
##### LAST_HEDGE (TA805)
- 21:00:00.224/5692/0|-6/
- 21:00:01.427/5690/0
###### LAST_HEDGE (i1805)
- 21:00:00.427/514.5/0|1/
- 21:00:01.412/514/0"""))

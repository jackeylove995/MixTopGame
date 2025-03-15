#!/usr/bin/env python
# -*- encoding:utf-8 -*-

import sys
import requests

JOB_URL = sys.argv[1]
JOB_NAME = sys.argv[2]
isFinish = "构建完成"
url = 'https://open.feishu.cn/open-apis/bot/v2/hook/e4e2c941-79d9-41df-b16d-ce3e37501fab'
method = 'post'
headers = {
    'Content-Type': 'application/json'
}
json = {
    "msg_type": "interactive",
    "card": {
        "config": {
            "wide_screen_mode": True,
            "enable_forward": True
        },
        "elements": [{
            "tag": "div",
            "text": {
                "content": "Hello feishu",
                "tag": "lark_md"
            }
        }, {
            "actions": [{
                "tag": "button",
                "text": {
                    "content": "查看报告",
                    "tag": "lark_md"
                },
                "url": JOB_URL,
                "type": "default",
                "value": {}
            }],
            "tag": "action"
        }],
        "header": {
            "title": {
                "content": JOB_NAME + " "+isFinish+"",
                "tag": "plain_text"
            }
        }
    }
}
requests.request(method=method, url=url, headers=headers, json=json)


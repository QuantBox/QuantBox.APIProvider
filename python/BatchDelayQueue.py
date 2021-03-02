"""
延迟队列
"""
import threading
import time


class BatchQueue:
    def __init__(self):
        self.buf = []

    def add_one(self, obj):
        self.buf.append(obj)

    def get_all(self):
        _buf = self.buf.copy()
        self.buf = []
        return _buf


class DelayQueue:

    def __init__(self, interval, process_func):
        self.queue = BatchQueue()
        self.interval = interval
        self.process_func = process_func
        # 启动定时器
        threading.Timer(self.interval, self.process).start()

    def add(self, obj):
        self.queue.add_one(obj)

    def process(self):
        buf = self.queue.get_all()
        if len(buf) > 0:
            self.process_func(buf)
        # 定时器需要再激活才可使用
        threading.Timer(self.interval, self.process).start()


if __name__ == '__main__':

    def process(buf):
        print(buf)


    def thread_fun(dq):
        for i in range(1000):
            dq.add(i)
            time.sleep(0.01)


    dq = DelayQueue(5, process)
    threading.Thread(target=thread_fun, args=(dq,)).start()
    threading.Thread(target=thread_fun, args=(dq,)).start()
    threading.Thread(target=thread_fun, args=(dq,)).start()
    input()

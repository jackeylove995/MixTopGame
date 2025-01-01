using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    public class Clock : MonoBehaviour
    {
        public class Task
        {
            public enum Status
            {
                Running,
                Pause,
                Stop
            }

            public Status status;
            public float begin;
            public float end;
            public float interval;
            public Action<float> eachInterCall;
            public float cur;
            public int callTime;

            public float absInterval;

            public Task(float begin, float end, float interval, Action<float> eachInterCall)
            {
                this.cur = begin;
                this.begin = begin;
                this.end = end;
                this.interval = interval;
                this.eachInterCall = eachInterCall;
                absInterval = Mathf.Abs(interval);
            }
        }

        public static List<Task> tasks = new List<Task>();

        public static void Init()
        {
            GameObject go = new GameObject("Clock");
            DontDestroyOnLoad(go);
            go.AddComponent<Clock>();
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        public static void StartTimer(float begin, float end, float interval, Action<float> eachInterCall)
        {
            Task task = new Task(begin, end, interval, eachInterCall);
            tasks.Add(task);
        }

        void Update()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                UpdateTask(tasks[i]);

                if (tasks[i].status == Task.Status.Stop)
                {
                    tasks.RemoveAt(i);
                    i--;
                }
            }
        }

        void UpdateTask(Task task)
        {
            switch (task.status)
            {
                case Task.Status.Running:
                    if ((task.interval > 0 && task.cur >= task.end)
                     || (task.interval < 0 && task.cur <= task.end))
                    {
                        task.eachInterCall?.Invoke(task.end);
                        task.status = Task.Status.Stop;
                        return;
                    }
                    float absDis = Mathf.Abs(task.cur - task.begin);
                    float intDis = task.callTime * task.absInterval;
                    if (absDis >= intDis)
                    {
                        int runToInt = task.callTime + 1;
                        while (runToInt * task.absInterval < absDis)
                        {
                            runToInt++;
                        }
                        for (int i = task.callTime; i < runToInt; i++)
                        {
                            task.eachInterCall?.Invoke(task.begin + i * task.interval);
                        }

                        task.callTime = runToInt;
                    }

                    if (task.interval > 0)
                    {
                        task.cur += Time.deltaTime;
                    }
                    else
                    {
                        task.cur -= Time.deltaTime;
                    }

                    break;
                case Task.Status.Pause:
                    break;
                case Task.Status.Stop:
                    break;
            }
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    public class Clock : MonoBehaviour
    {
        public static Clock Instance;

        public abstract class Task
        {
            public enum Status
            {
                Running,
                Pause,
                Stop
            }

            public Status status;
            public string name;
            public Coroutine coroutine;

            public void Name(string name)
            {
                this.name = name;
            }

        }

        public class TimerTask : Task
        {
            public float begin;
            public float end;
            public float interval;
            public Action<float> eachInterCall;
            public float cur;
            public int callTime;

            public float absInterval;

            public TimerTask(float begin, float end, float interval, Action<float> eachInterCall)
            {
                this.cur = begin;
                this.begin = begin;
                this.end = end;
                this.interval = interval;
                this.eachInterCall = eachInterCall;
                absInterval = Mathf.Abs(interval);
            }
        }

        public class FixTimeTask : Task
        {
            public int executeCount;
            public float interval;
            public bool fromNowOn;
            public Action<int> action;
        }

        public class DelayTask : Task
        {
            public float delay;
            public Action action;
        }

        public static Dictionary<string, Task> tasks = new Dictionary<string, Task>();
        public static List<Task> noNameTasks = new List<Task>();

        #region Init 初始化
        public static void Init()
        {
            GameObject go = new GameObject("Clock");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<Clock>();
        }
        #endregion

        #region SetName 为任务设置名称
        private static string taskNameTemp;
        public static Clock Name(string name)
        {
            taskNameTemp = name;
            return Instance;
        }
        #endregion

        #region Add or Remove Task
        private static void AddTask(Task task)
        {
            task.name = taskNameTemp;
            if (string.IsNullOrEmpty(task.name))
            {
                noNameTasks.Add(task);
            }
            else
            {
                tasks[task.name] = task;
            }
            taskNameTemp = string.Empty;
        }

        private static void RemoveTask(Task task)
        {
            if (task.name.Equals(string.Empty))
            {
                noNameTasks.Remove(task);
            }
            else
            {
                tasks[task.name] = null;
            }
            task = null;
        }
        #endregion

        #region Timer 倒计时
        /// <summary>
        /// 启动计时器
        /// </summary>
        public static void StartTimer(float begin, float end, float interval, Action<float> eachInterCall)
        {
            TimerTask task = new TimerTask(begin, end, interval, eachInterCall);
            task.coroutine = Instance.StartCoroutine(IETimer(task));
            AddTask(task);
        }
        static IEnumerator IETimer(TimerTask timerTask)
        {
            while (true)
            {
                UpdateTimerTask(timerTask);
                yield return null;
            }
        }
        static void UpdateTimerTask(TimerTask task)
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
        #endregion

        #region DelayCall 延时函数
        public static void DelayCall(float delay, Action action)
        {
            DelayTask task = new DelayTask();
            task.delay = delay;
            task.action = action;
            task.coroutine = Instance.StartCoroutine(IEDelayCall(task));
            AddTask(task);
        }
        static IEnumerator IEDelayCall(DelayTask delayTask)
        {
            yield return new WaitForSeconds(delayTask.delay);
            delayTask.action();
            RemoveTask(delayTask);
        }
        #endregion

        #region DelayFrameCall 等待几帧执行函数
        public static void DelayFramesCall(int frameCount, Action action)
        {
            Instance.StartCoroutine(IEDelayFramesCall(frameCount, action));
        }

        static IEnumerator IEDelayFramesCall(int frameCount, Action action)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }
            action();
        }
        #endregion

        #region FixTimeCall 每隔固定时间执行函数
        /// <summary>
        /// 固定时间轮播任务
        /// </summary>
        /// <param name="interval">每隔多久</param>
        /// <param name="fromNowOn">从现在开始吗</param>
        /// <param name="action"></param>
        public static void FixTimeCall(float interval, bool fromNowOn, Action<int> action)
        {
            FixTimeTask task = new FixTimeTask();
            task.interval = interval;
            task.fromNowOn = fromNowOn;
            task.action = action;
            task.coroutine = Instance.StartCoroutine(IEFixTimeCall(task));
            AddTask(task);
        }

        static IEnumerator IEFixTimeCall(FixTimeTask task)
        {
            if (task.fromNowOn)
            {
                task.executeCount++;
                task.action(task.executeCount);
                yield return null;
            }
            WaitForSeconds waitForSeconds = new WaitForSeconds(task.interval);
            while (true)
            {
                yield return waitForSeconds;
                task.executeCount++;
                task.action(task.executeCount);
            }
        }

        #endregion

        #region 停止一个任务
        public static void StopTask(string name)
        {
            Task task = tasks[name];
            Instance.StopCoroutine(task.coroutine);
            tasks.Remove(name);
        }

        #endregion
    }
}


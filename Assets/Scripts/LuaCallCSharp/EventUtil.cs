using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace MTG
{
    
    /// <summary>
    /// lua发布的消息，Unity也能够接收到
    /// </summary>
    public static class EventUtil
    {
        public class Receiver
        {
            public string receiverName;
            public Action<LuaTable> action;

            public Receiver(string  receiverName, Action<LuaTable> action)
            {
                this.receiverName = receiverName;
                this.action = action;
            }
        }

        public static Dictionary<string, List<Receiver>> EventMap
            = new Dictionary<string, List<Receiver>>();

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="mapParam">参数，类型为Dictionary<string, object>，Lua中直接传table</param>
        public static void Push(string eventName, LuaTable mapParam = null)
        {
            if (EventMap.ContainsKey(eventName))
            {
                var receivers = EventMap[eventName];
                for (int i = receivers.Count - 1 ;  i > -1 ; i--)
                {
                    try
                    {
                        if (receivers[i].receiver == null) 
                        {
                            receivers.RemoveAt(i);
                            continue;
                        }
                        receivers[i].action.Invoke(mapParam);
                    }
                    catch (Exception e)
                    {
                        receivers.RemoveAt(i);
                        Debug.LogFormat($"[EventUtil] event execute error, event name: {eventName}, so remove it. \n{e.Message}");
                    }
                }
                if (receivers.Count == 0)
                {
                    EventMap.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">回调函数</param>
        public static void Receive(string receiverName, string eventName, Action<LuaTable> action)
        {
            List<Receiver> receivers;
            if (EventMap.ContainsKey(eventName))
            {
                receivers = EventMap[eventName];

                for(int i = receivers.Count - 1 ;i > -1 ;i--)
                {
                    if (receivers[i].receiver == receiver)
                    {
                        Debug.Log($"[EventUtil] {eventName} has received double in one receiver,  receiver type is {receiver.GetType().ToString()}, so remove old one");
                        receivers.RemoveAt(i);
                    }
                }
            }
            else
            {
                receivers = new List<Receiver>();
                EventMap[eventName] = receivers;
            }

           receivers.Add(new Receiver(receiver, action));
        }
    }
}


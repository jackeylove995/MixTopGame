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
        public static Dictionary<string, List<Action<LuaTable>>> EventMap
            = new Dictionary<string, List<Action<LuaTable>>>();

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="mapParam">参数，类型为Dictionary<string, object>，Lua中直接传table</param>
        public static void Push(string eventName, LuaTable mapParam = null)
        {
            if (EventMap.ContainsKey(eventName))
            {
                var actions = EventMap[eventName];
                for (int i = 0; i < actions.Count; i++)
                {
                    try
                    {
                        actions[i].Invoke(mapParam);
                    }
                    catch (Exception e)
                    {
                        actions.RemoveAt(i);
                        i--;
                        Debug.LogFormat($"event execute error, event name: {eventName}, so remove it. \n{e.Message}");
                    }
                }
                if (actions.Count == 0)
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
        public static void Receive(string eventName, Action<LuaTable> action)
        {
            if (EventMap.ContainsKey(eventName))
            {
                EventMap[eventName].Add(action);
            }
            else
            {
                EventMap[eventName] = new List<Action<LuaTable>>
                {
                    action
                };
            }          
        }
    }
}


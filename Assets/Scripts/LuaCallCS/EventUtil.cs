using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class EventUtil
    {
        public static Dictionary<string, List<Action<LuaTable>>> EventMap = new Dictionary<string, List<Action<LuaTable>>>();

        public static void Push(string eventName, LuaTable luaTable = null)
        {
            if (EventMap.ContainsKey(eventName))
            {
                List<Action<LuaTable>> actions = EventMap[eventName];
                for (int i = 0; i < actions.Count; i++)
                {
                    try
                    {
                        actions[i].Invoke(luaTable);
                    }
                    catch (Exception e)
                    {
                        actions.RemoveAt(i);
                        i--;
                        Debug.LogError("Event execute error , name: "
                        + eventName + "\n"
                        + e.Message + "\n"
                        + e.StackTrace);
                    }
                }
                if (actions.Count == 0)
                {
                    EventMap.Remove(eventName);
                }
            }
        }

        public static void Receive(string eventName, Action<LuaTable> action)
        {
            if (EventMap.ContainsKey(eventName))
            {
                EventMap[eventName].Add(action);
            }
            List<Action<LuaTable>> actions = new List<Action<LuaTable>>();
            actions.Add(action);
            EventMap[eventName] = actions;
        }
    }
}


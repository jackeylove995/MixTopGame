using System;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class MonoUtil
    {
        public static void AddUpdate(Action action)
        {
            MonoManager.Instance.AddUpdate(action);
        }

        public static void RemoveUpdate(Action action)
        {
            MonoManager.Instance.RemoveUpdate(action);
        }

    }
}


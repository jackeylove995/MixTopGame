using System;

namespace MTG
{
    public static class MonoUtil 
    {
        public static void Init()
        {
            var instance = MonoManager.Instance;
        }

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


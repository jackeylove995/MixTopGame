using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class GOLoader
    {     
        public static void LoadFullPanel(string module, string name, Action<GameObject> onCreate)
        {
            string path = string.Format($"Assets/HotFixAssets/{module}/Prefabs/{name}.prefab");
            Addressables.InstantiateAsync(path, UIPanelSetting.FullbackPanelContainor).Completed += (handle) =>
            {
                onCreate?.Invoke(handle.Result);
            };
        }

        public static void LoadPopPanel(string module, string name, Action<GameObject> onCreate)
        {
            string path = string.Format($"Assets/HotFixAssets/{module}/Prefabs/{name}.prefab");
            Addressables.InstantiateAsync(path, UIPanelSetting.PopPanelContainor).Completed += (handle) =>
            {
                onCreate?.Invoke(handle.Result);
            };
        }
    }
}


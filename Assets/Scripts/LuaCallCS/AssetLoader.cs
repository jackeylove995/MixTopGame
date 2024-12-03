using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class AssetLoader
    {     
        public static void LoadGameObject(string address, Transform parent, Action<GameObject> onCreate)
        {
            Addressables.InstantiateAsync(address + ".prefab", parent).Completed += (handle) =>
            {
                onCreate?.Invoke(handle.Result);
            };
        }
    }
}


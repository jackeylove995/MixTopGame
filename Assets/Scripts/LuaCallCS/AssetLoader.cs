using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;
using XLuaTest;

namespace MTG
{
    [LuaCallCSharp]
    public static class AssetLoader
    {
        public static void LoadGameObject(string address, Transform parent, Action<GameObject, LuaTable> onCreate)
        {
            Addressables.InstantiateAsync(address, parent).Completed += (handle) =>
            {
                GameObject gameObject = handle.Result;
                onCreate?.Invoke(gameObject, gameObject.GetComponent<LuaBehaviour>().scriptEnv);
            };
        }
    }
}


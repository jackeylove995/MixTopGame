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
            Action<GameObject, LuaTable> back = onCreate;
            Addressables.InstantiateAsync(address, parent).Completed += (handle) =>
            {
                GameObject gameObject = handle.Result;
                back?.Invoke(gameObject, gameObject.GetComponent<LuaBehaviour>().scriptEnv);
            };
        }

        public static void DestroyGameObject(UnityEngine.Object obj)
        {
            if(obj is Component component)
            {
                GameObject.Destroy(component.gameObject);
            }
        }
    }
}


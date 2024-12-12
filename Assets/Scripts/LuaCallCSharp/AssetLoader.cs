using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

namespace MTG
{
    public static class AssetLoader
    {
        public static void LoadGameObjectAsync(string address, Transform parent, Action<LuaTable> onCreate)
        {
            Action<LuaTable> back = onCreate;
            Addressables.InstantiateAsync(address, parent).Completed += (handle) =>
            {
                back?.Invoke(handle.Result.GetComponent<LuaBehaviour>().scriptTable);
            };
        }

        public static LuaTable LoadGameObjectSync(string address, Transform parent)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            handle.WaitForCompletion();
            GameObject go = GameObject.Instantiate(handle.Result);
            go.transform.SetParent(parent);
            return go.GetComponent<LuaBehaviour>().scriptTable;
        }

        public static void DestroyGameObject(UnityEngine.Object obj)
        {
            if (obj is Component component)
            {
                GameObject.Destroy(component.gameObject);
            }
        }
    }
}


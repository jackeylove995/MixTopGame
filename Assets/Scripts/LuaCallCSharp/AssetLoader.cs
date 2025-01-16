using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using XLua;

namespace MTG
{
    public static class AssetLoader
    {
        public static void LoadGameObjectAsync(
            string address,
            Transform parent = null,
            Action<LuaTable> onCreate = null
        )
        {
            Addressables.InstantiateAsync(address, parent).Completed += (handle) =>
            {
                onCreate?.Invoke(handle.Result.GetComponent<LuaBehaviour>().scriptTable);
            };
        }

        public static LuaTable LoadGameObjectSync(string address, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(address, parent);
            handle.WaitForCompletion();    
            return handle.Result.GetComponent<LuaBehaviour>().scriptTable;
        }

        public static void LoadSpriteAtlasAsync(string address, Action<SpriteAtlas> onComplete)
        {
            Addressables.LoadAssetAsync<SpriteAtlas>(address).Completed += (handle) =>
            {
                onComplete.Invoke(handle.Result);
            };
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

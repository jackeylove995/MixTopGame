using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;
using static XLua.LuaEnv;
using ZLuaFramework;
using System.IO;

namespace MTG
{
    public class LuaInitTask : AppInitTask
    {

        public override void OnAllTasksInitSuccessfully()
        {
            LuaBehaviour.luaEnv.DoString("require 'main'");
        }

        public override IEnumerator DOTask()
        {           
            LuaBehaviour.luaEnv.AddLoader(GetLoader());     
            yield return null;
        }

        public CustomLoader GetLoader()
        {
#if UNITY_EDITOR

            string luaPathPre = "Assets/AssetsHotFix/Lua/";
            CustomLoader fileLoader = (ref string address) =>
            {
                string filePath = luaPathPre + address.Replace(".", "/") + ".lua";
                return File.ReadAllBytes(filePath);
            };
            return fileLoader;

#endif

            CustomLoader addressableLoader = (ref string address) =>
            {
                address = "Lua/" + address.Replace(".", "/") + ".lua";
                var handle = Addressables.LoadAssetAsync<TextAsset>(address);
                handle.WaitForCompletion();
                byte[] luaTextBytes = System.Text.Encoding.UTF8.GetBytes(handle.Result.text);
                Addressables.Release(handle);
                return luaTextBytes;
            };
            return addressableLoader;
        }
    }
}


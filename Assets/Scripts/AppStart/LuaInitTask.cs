using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;
using static XLua.LuaEnv;
using ZLuaFramework;

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
            CustomLoader customLoader = (ref string address) =>
            {
                address = "Lua/" + address.Replace(".", "/") + ".lua";
                var handle = Addressables.LoadAssetAsync<TextAsset>(address);
                handle.WaitForCompletion();
                byte[] luaTextBytes = System.Text.Encoding.UTF8.GetBytes(handle.Result.text);
                Addressables.Release(handle);
                return luaTextBytes;
            };

            LuaBehaviour.luaEnv.AddLoader(customLoader);     

            yield return null;
        }
    }
}


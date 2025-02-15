using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static XLua.LuaEnv;

namespace MTG
{
    public class LuaInitTask : AppInitTask
    {
        public string MainLuaPath = "Lua/AAAInit/main.lua";

        private string mainTxt;

        public override void OnAllTasksInitSuccessfully()
        {
            LuaBehaviour.luaEnv.DoString(mainTxt);
        }

        public override IEnumerator DOTask()
        {
            bool taskOver = false;
            CustomLoader customLoader = (ref string address) =>
            {
                var handle = Addressables.LoadAssetAsync<TextAsset>(address);
                handle.WaitForCompletion();
                byte[] luaTextBytes = System.Text.Encoding.UTF8.GetBytes(handle.Result.text);
                Addressables.Release(handle);
                return luaTextBytes;
            };

            LuaBehaviour.luaEnv.AddLoader(customLoader);

            Addressables.LoadAssetAsync<TextAsset>(MainLuaPath).Completed += (handle) =>
            {
                mainTxt = handle.Result.text;
                Addressables.Release(handle);
                taskOver = true;
            };

            yield return new WaitUntil(() => taskOver);
        }
    }
}


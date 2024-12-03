using System.Collections;
using System.Collections.Generic;
using MTG;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using XLua;
using XLuaTest;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        TaskLua();
        //StartCoroutine(TaskDoUpdateAddressadble());
    }

    public void TaskLua()
    {       
        LuaEnv luaEnv = LuaBehaviour.luaEnv;
        Addressables.LoadAssetAsync<TextAsset>("AAAInit/Lua/init.lua").Completed += (handle)=>
        {
            string initCode = handle.Result.text;
            Addressables.Release(handle);
            luaEnv.DoString(initCode);
        };
    }

    IEnumerator TaskDoUpdateAddressadble()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // 检测更新
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;
        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            OnError("CheckForCatalogUpdates Error\n" +  checkHandle.OperationException.ToString());
            yield break;
        }

        if (checkHandle.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, true);
            yield return updateHandle;

            if (updateHandle.Status != AsyncOperationStatus.Succeeded)
            {
                OnError("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                yield break;
            }

            // 更新列表迭代器
            List<IResourceLocator> locators = updateHandle.Result;
            foreach (var locator in locators)
            {
                List<object> keys = new List<object>();
                keys.AddRange(locator.Keys);
                // 获取待下载的文件总大小
                var sizeHandle = Addressables.GetDownloadSizeAsync(keys.GetEnumerator());
                yield return sizeHandle;
                if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    OnError("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                    yield break;
                }

                long totalDownloadSize = sizeHandle.Result;
                NormalDebug("download size : " + totalDownloadSize);
                if (totalDownloadSize > 0)
                {
                    // 下载
                    var downloadHandle = Addressables.DownloadDependenciesAsync(keys, true);
                    while (!downloadHandle.IsDone)
                    {
                        if (downloadHandle.Status == AsyncOperationStatus.Failed)
                        {
                            OnError("DownloadDependenciesAsync Error\n"  + downloadHandle.OperationException.ToString());
                            yield break;
                        }
                        // 下载进度
                        float percentage = downloadHandle.PercentComplete;
                        NormalDebug(string.Format($"has download: {percentage}"));
                        
                        yield return null;
                    }
                    if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
      
                        NormalDebug("down over");
                    }
                }
            }
        }
        else
        {
            NormalDebug("check no update");
        }

        TaskLua();
    }

    // 异常提示
    private void OnError(string msg)
    {
        DebugUtil.ShowDebugMes(string.Format($"\n{msg}\n retry!"));
    }

    private void NormalDebug(string msg)
    {
        DebugUtil.ShowDebugMes(msg);
    }
}

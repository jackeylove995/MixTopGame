using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace MTG
{
    public class AutoMarkAddress : AssetPostprocessor
    {
        static AddressableAssetSettings settings;

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {

            // 获取当前Addressable设置
            if (settings == null)
            {
                settings = AddressableAssetSettingsDefaultObject.Settings;
            }

            if (settings == null || settings.groups == null || settings.groups.Count == 0)
            {
                Debug.LogError("先创建一个默认addressable group，才可以使用自动标记address功能");
                return;
            }
            if (!settings.BuildRemoteCatalog)
            {
                Debug.LogWarning("先设置AddressableAssetSettings的Build Remote Catalog为true, 并配置Build和RemotePath才可以使用远程加载，RemoteLoadPath为搭建的CDN资源存放地址");
                return;
            }

            foreach (string path in movedFromAssetPaths)
            {
                Debug.Log("move from " + path);
            }
            foreach (string path in importedAssets)
            {
                Debug.Log("import " + path);
                AddNewEntry(path);
            }
            foreach (string path in deletedAssets)
            {
                Debug.Log("delete " + path);
            }
            foreach (string path in movedAssets)
            {
                Debug.Log("move to " + path);
                AddNewEntry(path);
            }

            DeleteUnusedGroups();

            GenerateCodeMap(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
        }

        static void AddNewEntry(string path)
        {
            GetAssetAddressPath(path, out string address, out string module);
            if (address == string.Empty || module == string.Empty)
            {
                return;
            }
            AddressableAssetGroup group = GetOrCreateGroup(module);

            foreach (var asset in group.entries)
            {
                if (asset.address == path)
                {
                    return;
                }
            }
            Debug.Log("Create Address:" + address);
            settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group).address = address;
        }

        static AddressableAssetGroup GetOrCreateGroup(string module)
        {

            foreach (var group in settings.groups)
            {
                if (group.Name == module)
                {
                    return group;
                }
            }

            List<AddressableAssetGroupSchema> schemas = new List<AddressableAssetGroupSchema>();

            BundledAssetGroupSchema contentUpdate = ScriptableObject.CreateInstance<BundledAssetGroupSchema>(); ;
            // 设置 Build Path 和 Load Path
            contentUpdate.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteBuildPath);
            contentUpdate.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteLoadPath);

            schemas.Add(contentUpdate);
            schemas.Add(ScriptableObject.CreateInstance<ContentUpdateGroupSchema>());
            return settings.CreateGroup(module, false, false, false, schemas);
        }

        /// <summary>
        /// 删除无数据组
        /// </summary>
        static void DeleteUnusedGroups()
        {
            List<AddressableAssetGroup> deleteGroups = settings.groups.Where(a => a.entries.Count == 0 && !a.Default).ToList();

            foreach (var group in deleteGroups)
            {
                settings.RemoveGroup(group);
            }
        }

        /// <summary>
        ///  形如Assets/HotFixAssets/a/b/c...格式，获取a/b/c...和a
        /// </summary>
        /// <param name="input">原始地址</param>
        /// <param name="address">去除前缀简化地址</param>
        /// <param name="module">所属模块</param>
        static void GetAssetAddressPath(string input, out string address, out string module)
        {
            string pathPrefix = "Assets/HotFixAssets/";
            if (input.Contains(pathPrefix) && input.Contains('.'))
            {
                address = input.Replace(pathPrefix, string.Empty);
                module = address.Split('/')[0];
            }
            else
            {
                address = string.Empty;
                module = string.Empty;
            }

        }

        /// <summary>
        /// 生成全局代码映射，映射完不用手写地址
        /// 映射规则：
        /// key : 统一为 文件名_后缀，整个项目不要使用同名资源
        /// value : lua文件为路径用.分割，用以require，预制体路径用/分割，用来Addressables.LoadAsset
        /// </summary>
        static void GenerateCodeMap(string[] imports, string[] deletes, string[] moved, string[] moveFrom)
        {
            if (imports.Length == 0
            && deletes.Length == 0
            && moved.Length == 0
            && moveFrom.Length == 0)
            {
                return;
            }

            string addressMapLuaPath = Path.Combine(PathSetting.CodeAddressMapPath, "AddressMap.lua");

            List<string> content = new List<string>();
            content.Add("--生成全局代码映射，不用手写地址");
            content.Add("--映射规则：");
            content.Add("--key : 统一为 文件名_后缀，整个项目不要使用同名资源");
            content.Add("--value : lua文件为路径用.分割，用以require，预制体路径用/分割，用来Addressables.LoadAsset");

            List<string> subContent = new List<string>();

            bool refresh = false;
            if (!Directory.Exists(PathSetting.CodeAddressMapPath))
            {
                refresh = true;
                Directory.CreateDirectory(PathSetting.CodeAddressMapPath);
            }

            try
            {
                //还没这个文件会报错，如果还没这个文件的话直接就用默认值空列表
                subContent = File.ReadAllLines(addressMapLuaPath).ToList();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message + " Create AddressMap.lua");
            }

            Func<string, string> getPathKey = (path) =>
            {
                string[] addressFilesName = path.Replace("Assets/HotFixAssets/", "").Split('/');
                string key = addressFilesName[addressFilesName.Length - 1].Replace('.', '_');
                return key;
            };

            Func<string, string> getSpecialPath = (path) =>
            {
                if (path.EndsWith(".lua"))
                {
                    path = path.Replace(".lua", "").Replace("/", ".");
                }
                else
                {
                    path = path.Replace("Assets/HotFixAssets/", "");
                }
                return path;
            };

            bool haveChange = false;
            Action<string> deletePath = (deletePath) =>
            {
                for (int i = 0; i < subContent.Count; i++)
                {
                    if (subContent[i].Contains(deletePath))
                    {
                        subContent.RemoveAt(i);
                        haveChange = true;
                        i--;
                    }
                }
            };

            Action<string> importPath = (importPath) =>
            {
                if (importPath.Contains("Assets/HotFixAssets"))
                {
                    string key = getPathKey(importPath);
                    string value = getSpecialPath(importPath);
                    //不用format节省时间
                    string entryLine = key + " = " + "\"" + value + "\"";
                    subContent.Add(entryLine);
                    haveChange = true;
                }
            };

            foreach (var path in imports)
            {
                importPath(path);
            }
            foreach (var path in moved)
            {
                importPath(path);
            }
            foreach (var path in deletes)
            {
                deletePath(path);
            }
            foreach (var path in moveFrom)
            {
                deletePath(path);
            }

            if (haveChange)
            {
                content.AddRange(subContent);
                File.WriteAllLines(Path.Combine(PathSetting.CodeAddressMapPath, "AddressMap.lua"), content);
                Debug.Log("AddressMap Generate!");
            }

            if (refresh)
            {
                new DirectoryInfo(PathSetting.CodeAddressMapPath).Refresh();
            }

            
        }
    }
}


using System.Collections.Generic;
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
                //Debug.Log("move from " + path);
            }
            foreach (string path in importedAssets)
            {
                //Debug.Log("import " + path);
                AddNewEntry(path);
            }
            foreach (string path in deletedAssets)
            {
                //Debug.Log("delete " + path);
            }
            foreach (string path in movedAssets)
            {
                //Debug.Log("move to " + path);
                AddNewEntry(path);
            }

            DeleteUnusedGroups();
        }


        static void AddNewEntry(string path)
        {
            GetAssetModuleAndNameByPath(path, out var module, out var name);
            if (module == string.Empty || name == string.Empty)
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
            settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group).address = path.Replace("Assets/HotFixAssets/", "");
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
        /// 形如Assets/HotFixAssets/a/b/c格式，获取a和c，此项目中分别为Module和AssetName
        /// </summary>
        /// <param name="input"></param>
        /// <param name="module"></param>
        /// <param name="name"></param>
        static void GetAssetModuleAndNameByPath(string input, out string module, out string name)
        {
            module = string.Empty;
            name = string.Empty;
            string pathPrefix = "Assets/HotFixAssets/";
            if (input.Contains(pathPrefix))
            {
                input = input.Replace(pathPrefix, string.Empty);
                string[] abc = input.Split('/');
                if (abc.Length == 3)
                {
                    module = abc[0];
                    name = abc[2];
                }

            }
        }
    }
}


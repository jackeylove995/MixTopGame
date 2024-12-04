using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace MTG
{
    public class AutoMarkAddress : AssetPostprocessor
    {

        public enum ChangeAddressType
        {
            NoChange = 0,
            NewAddress = 1,
            DeleteAddress = 2,
            RemoveUnusedGroup = 4
        }

        static AddressableAssetSettings settings;
        static string SomeAddressChanged;
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

            SomeAddressChanged = string.Empty;

            ///移动a -> a/b,触发movedFromAssetPaths ： path为Assets/...a
            ///                 movedAssets         :   path为Assets/...a/b
            ///修改或导入触发 importedAssets : path 为Assets/下目标文件路径带文件后缀
            ///删除触发deletedAssets : path 为Assets/下目标文件路径带文件后缀
            ///删除资源时Addresable自动解除标记
            ///增加时手动打新标记
            ///移动时手动打新标记，Addresable会自动移除旧标记
            foreach (string path in importedAssets)
            {
                if (!path.Contains("AddressMap/AddressMap.lua"))
                {
                    AddNewEntry(path);
                }
            }
            foreach (string path in movedAssets)
            {
                AddNewEntry(path);
            }

            if (deletedAssets != null && deletedAssets.Length > 0)
            {
                foreach (var path in deletedAssets)
                {
                    if (IsAValidAddressPath(path))
                    {
                        SomeAddressChangedBy(ChangeAddressType.DeleteAddress);
                        break;
                    }
                }

            }

            //删除group数据为空的无用组
            DeleteUnusedGroups();

            if (SomeAddressChanged != string.Empty)
            {
                GenerateCodeMap();
            }
        }


        static void AddNewEntry(string path)
        {
            if (!IsAValidAddressPath(path))
            {
                return;
            }
            string module = path.Split('/')[2];
            AddressableAssetGroup group = GetGroup(module, true);

            foreach (var asset in group.entries)
            {
                if (asset.address == path)
                {
                    return;
                }
            }
            Debug.Log("Create Address:" + path);
            SomeAddressChangedBy(ChangeAddressType.NewAddress);
            settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group).address = path;
        }

        static AddressableAssetGroup GetGroup(string module, bool createNewGroupIfNull)
        {
            foreach (var group in settings.groups)
            {
                if (group.Name == module)
                {
                    return group;
                }
            }
            if (!createNewGroupIfNull)
            {
                return null;
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
        public static void DeleteUnusedGroups()
        {
            List<AddressableAssetGroup> emptyGroups = settings.groups.Where(a => a.entries.Count == 0 && !a.Default).ToList();
            if (emptyGroups.Count > 0)
            {
                SomeAddressChangedBy(ChangeAddressType.RemoveUnusedGroup);
            }
            foreach (var group in emptyGroups)
            {
                settings.RemoveGroup(group);
            }
        }

        static bool IsAValidAddressPath(string path)
        {
            ///位于热更文件夹下
            ///包含.，表明不是一个文件夹
            ///路径分割完>3，表明不是HotFixAssets的直系文件，因为只有一个文件夹表明所属module
            if (path.Contains("Assets/HotFixAssets")
             && path.Contains('.')
             && path.Split('/').Length > 3)
            {
                return true;
            }
            return false;
        }

        static void SomeAddressChangedBy(ChangeAddressType changeAddressType)
        {
            if (SomeAddressChanged.Contains(changeAddressType.ToString()))
            {
                return;
            }
            SomeAddressChanged += " " + changeAddressType.ToString();
        }
        /// <summary>
        /// Auto Generate
        /// 生成全局资源映射，映射完不用手写地址
        /// 映射规则：
        /// key : 统一为 文件名_后缀，整个项目不要使用同名资源
        /// value : lua文件为路径用.分割，用以require，预制体路径用/分割，用来Addressables.LoadAsset
        /// </summary>
        public static void GenerateCodeMap()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("--Auto Generate");
            content.AppendLine("--生成全局资源映射，不用手写地址");
            content.AppendLine("--映射规则：");
            content.AppendLine("--key : 统一为 文件名_后缀，整个项目不要使用同名资源");
            content.AppendLine("--value : lua文件为路径用.分割，用以require，预制体路径用/分割，用来Addressables.LoadAsset");

            if (!Directory.Exists(PathSetting.CodeAddressMapPath))
            {
                Directory.CreateDirectory(PathSetting.CodeAddressMapPath);
                File.WriteAllText(Path.Combine(PathSetting.CodeAddressMapPath, "AddressMap.lua"), content.ToString());
                AddNewEntry("Assets/HotFixAssets/AddressMap/AddressMap.lua");
            }

            foreach (var group in settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    string[] addressFilesName = entry.address.Split('/');
                    string key = addressFilesName[addressFilesName.Length - 1].Replace('.', '_');
                    string address = entry.address;
                    if (address.EndsWith(".lua"))
                    {
                        address = address.Replace(".lua", "").Replace("/", ".");
                    }
                    string entryLine = key + " = " + "\"" + address + "\"";
                    content.AppendLine(entryLine);
                }
            }

            File.WriteAllText(Path.Combine(PathSetting.CodeAddressMapPath, "AddressMap.lua"), content.ToString());
            Debug.Log("AddressMap Generate! By " + SomeAddressChanged);
            new DirectoryInfo(PathSetting.CodeAddressMapPath).Refresh();
        }
    }
}


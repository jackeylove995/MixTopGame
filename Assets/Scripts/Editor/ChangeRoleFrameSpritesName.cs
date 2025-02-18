using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace MTG
{
    /// <summary>
    /// when import role frame animation sprites, change its name standard
    /// </summary>
    public class ChangeRoleFrameSpritesName
    {
        /// <summary>
        /// 检测名称是否包含key，如果包含，则名为value的anim就会呗重命名
        /// </summary>
        static Dictionary<string, string> nameMap = new Dictionary<string, string>()
        {
            { "idle,Idle,IDLE" , "idle" },
            { "run,walk,Run,Walk,RUN,WALK" , "run"},
            { "hurt,Hurt,HURT" , "hurt"}
        };


        public static void OnPostprocessAllAssets(
           string[] importedAssets,
           string[] deletedAssets,
           string[] movedAssets,
           string[] movedFromAssetPaths)
        {
            foreach (string asset in importedAssets)
            {
                RenameFileIfShould(asset);
            }

            foreach (string asset in movedAssets)
            {
                RenameFileIfShould(asset);
            }
        }
        
        static void RenameFileIfShould(string assetPath)
        {
            if(assetPath.StartsWith(ZLuaFramework.PathSetting.RoleFrameAnimationSpritesPath))
            {
                if(IsAValidRolePath(assetPath, out string animName, out int animIndex))
                {
                    AssetDatabase.RenameAsset(assetPath, animName + "_" + animIndex);
                }
            }
        }

        static bool IsAValidRolePath(string path, out string animName, out int animIndex)
        {
            string name = Path.GetFileNameWithoutExtension(path);

            foreach (var animNameKV in nameMap)
            {
                foreach (var filter in animNameKV.Key.Split(','))
                {
                    if (name.Contains(filter))
                    {
                        animName = animNameKV.Value;
                        // 使用正则表达式提取字符串中的数字
                        // 使用正则表达式进行匹配
                        Match match = Regex.Match(name, @"\d+");
                        if (match.Success)
                        {
                            // 如果匹配成功，提取数字并转换为整数
                            int.TryParse(match.Value, out animIndex);
                            return true;
                        }
                        else
                        {
                            Debug.LogError("the anim sprite you import has no index number! which is " + name);
                        }
                    }
                }
            }

            animName = string.Empty;
            animIndex = -1;
            return false;
        }

    }
}


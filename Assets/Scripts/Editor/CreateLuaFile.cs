using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MTG
{
    /// <summary>
    /// you can create lua file by right-click -> CreateLua , the file default context is LuaTemplate.lua
    /// </summary>
    public class CreateLuaFile : EditorWindow
    {
        static string LuaTemplatePath = Path.Combine(
            Application.dataPath,
            "AssetsDevelop",
            "Lua",
            "LuaTemplate.lua"
        );

        [MenuItem("Assets/Create Lua")]
        public static void CreateLuaScript()
        {
            // 获取选中对象的相对路径
            string selectedObjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!string.IsNullOrEmpty(selectedObjectPath))
            {
                // 获取选中对象的绝对路径
                string absolutePath = Path.Combine(
                    Application.dataPath,
                    selectedObjectPath.Substring("Assets/".Length),
                    "NewLua.lua"
                );
                string templateDes = File.ReadAllText(LuaTemplatePath)
                    .Replace("create-time", DateTime.Now.ToString());
                ;
                File.WriteAllText(absolutePath, templateDes);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("No object selected or the selected object is not an asset.");
            }
        }
    }
}


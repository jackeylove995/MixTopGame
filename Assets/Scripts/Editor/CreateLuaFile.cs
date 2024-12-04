using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Runtime.InteropServices;

public class CreateLuaFile : EditorWindow
{
    static string LuaTemplatePath = Path.Combine(Application.dataPath, "DevelopAssets", "Lua","LuaTemplate.lua");

    [MenuItem("Assets/Create Lua")]
    public static void CreateLuaScript()
    {
        // 获取选中对象的相对路径
        string selectedObjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (!string.IsNullOrEmpty(selectedObjectPath))
        {
            // 获取选中对象的绝对路径
            string absolutePath = Path.Combine(Application.dataPath, selectedObjectPath.Substring("Assets/".Length), "NewLua.lua");            
            string templateDes = File.ReadAllText(LuaTemplatePath).Replace("create-time", DateTime.Now.ToString());;
            File.WriteAllText(absolutePath, templateDes);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("No object selected or the selected object is not an asset.");
        }
    }
}
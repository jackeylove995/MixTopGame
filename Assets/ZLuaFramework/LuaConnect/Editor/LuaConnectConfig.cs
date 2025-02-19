using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZLuaFramework
{
    [CreateAssetMenu(fileName = "LuaConnectConfig", menuName = "ZLuaFrameworkConfigs/LuaConnectConfig",order = 1)]
    public class LuaConnectConfig : ScriptableObject
    {
        private static LuaConnectConfig self;
        public static LuaConnectConfig GetAsset()
        {
            if(self == null)
            {
                self = AssetDatabase.LoadAssetAtPath<LuaConnectConfig>("Assets/ZLuaFramework/LuaConnect/LuaConnectConfig.asset");
            }
            return self;
        }


        public Object ExcelFolder;
        public string ExcelFolderPath = "No object selected";

        public Object OutputLuaFolder;
        public string OutputLuaFolderPath = "No object selected";


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LuaConnectConfig))]
    public class LuaConnectConfigEditor : Editor
    {
        private SerializedObject serializedObject;
        private SerializedProperty ExcelFolder, ExcelFolderPath, OutputLuaFolder, OutputLuaFolderPath;
        private GUIStyle title;
        private GUIStyle titleStyle()
        {
            if (title == null)
            {
                title = new GUIStyle(EditorStyles.label);
                title.fontSize = 16;
                title.fontStyle = FontStyle.Bold;
            }
            return title;
        }
        private void OnEnable()
        {
            serializedObject = new SerializedObject(target);
            ExcelFolder = serializedObject.FindProperty("ExcelFolder");
            ExcelFolderPath = serializedObject.FindProperty("ExcelFolderPath");
            OutputLuaFolder = serializedObject.FindProperty("OutputLuaFolder");
            OutputLuaFolderPath = serializedObject.FindProperty("OutputLuaFolderPath");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("LuaConnectConfig", titleStyle());

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert all excel files in the ExcelFolder to lua files in the OutputLuaFolder when there are changes.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(ExcelFolder);
            if (ExcelFolder.objectReferenceValue != null)
            {
                ExcelFolderPath.stringValue = AssetDatabase.GetAssetPath(ExcelFolder.objectReferenceValue);
                EditorGUILayout.LabelField("path:" + ExcelFolderPath.stringValue);
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(OutputLuaFolder);
            if (OutputLuaFolder.objectReferenceValue != null)
            {
                OutputLuaFolderPath.stringValue = AssetDatabase.GetAssetPath(OutputLuaFolder.objectReferenceValue);
                EditorGUILayout.LabelField("path:" + OutputLuaFolderPath.stringValue);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}


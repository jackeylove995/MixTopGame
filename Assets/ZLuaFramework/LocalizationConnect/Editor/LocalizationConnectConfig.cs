using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    [CreateAssetMenu(fileName = "LocalizationConnectConfig", menuName = "ZLuaFrameworkConfigs/LocalizationConnectConfig", order = 1)]
    public class LocalizationConnectConfig : ScriptableObject
    {
        private static LocalizationConnectConfig self;
        public static LocalizationConnectConfig GetAsset()
        {
            if (self == null)
            {
                self = AssetDatabase.LoadAssetAtPath<LocalizationConnectConfig>("Assets/ZLuaFramework/LocalizationConnect/LocalizationConnectConfig.asset");
            }
            return self;
        }


        public Object Excel;
        public string ExcelPath = "No object selected";

        public Object OutputLuaKeyFolder;
        public string OutputLuaKeyFolderPath = "No object selected";
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LocalizationConnectConfig))]
    public class LocalizationConnectConfigEditor : Editor
    {
        private SerializedObject serializedObject;
        private SerializedProperty Excel, ExcelPath, OutputLuaKeyFolder, OutputLuaKeyFolderPath;
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
            Excel = serializedObject.FindProperty("Excel");
            ExcelPath = serializedObject.FindProperty("ExcelPath");
            OutputLuaKeyFolder = serializedObject.FindProperty("OutputLuaKeyFolder");
            OutputLuaKeyFolderPath = serializedObject.FindProperty("OutputLuaKeyFolderPath");      
        }
        

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("AddressableConnectConfig", titleStyle());

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert Excel content to localization entries when Excel change.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(Excel);
            if(Excel.objectReferenceValue != null)
            {
                ExcelPath.stringValue = AssetDatabase.GetAssetPath(Excel.objectReferenceValue);
                EditorGUILayout.LabelField("path:" + ExcelPath.stringValue);
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Lua Support: Automatically generate localization keys to lua file in OutputLuaKeyFolder after excel change.(Optional)", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(OutputLuaKeyFolder);
            if (OutputLuaKeyFolder.objectReferenceValue != null)
            {
                OutputLuaKeyFolderPath.stringValue = AssetDatabase.GetAssetPath(OutputLuaKeyFolder.objectReferenceValue);
                EditorGUILayout.LabelField("path:" + OutputLuaKeyFolderPath.stringValue);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}


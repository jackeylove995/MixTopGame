using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    [CreateAssetMenu(fileName = "LocalizationConnectConfig", menuName = "ZLuaFrameworkConfigs/LocalizationConnectConfig", order = 1)]
    public class LocalizationConnectConfig : ScriptableObject
    {
        private static LocalizationConnectConfig self;
        public static LocalizationConnectConfig Instance
        {
            get
            {
                if (self == null)
                {
                    string fileName = "LocalizationConnectConfig";
                    string[] guids = AssetDatabase.FindAssets(fileName);
                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path.Contains(fileName) && path.EndsWith(".asset"))
                        {
                            self = AssetDatabase.LoadAssetAtPath<LocalizationConnectConfig>(path);
                        }
                    }
                }
                return self;
            }           
        }

        public AssetWithPath ExcelFile;

        public AssetWithPath OutputLuaKeyFile;

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LocalizationConnectConfig))]
    public class LocalizationConnectConfigEditor : Editor
    {
        private SerializedObject csharpObject;
        private SerializedProperty ExcelFile, OutputLuaKeyFile;

        private void OnEnable()
        {
            csharpObject = new SerializedObject(target);
            ExcelFile = csharpObject.FindProperty("ExcelFile");
            OutputLuaKeyFile = csharpObject.FindProperty("OutputLuaKeyFile");   
        }
        

        public override void OnInspectorGUI()
        {
            csharpObject.Update();
            EditorGUILayout.LabelField("AddressableConnectConfig", EditorDrawer.ConnectTitleStyle);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert Excel content to localization entries when Excel change.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(ExcelFile);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert localization keys in file in OutputLuaKeyFile after excel change.(Optional)", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(OutputLuaKeyFile);

            csharpObject.ApplyModifiedProperties();
        }
    }
#endif
}


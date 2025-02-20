using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    [CreateAssetMenu(fileName = "LuaConnectConfig", menuName = "ZLuaFrameworkConfigs/LuaConnectConfig",order = 1)]
    public class LuaConnectConfig : ScriptableObject
    {
        private static LuaConnectConfig self;
        public static LuaConnectConfig Instance
        {
            get
            {
                if (self == null)
                {
                    string fileName = "LuaConnectConfig";
                    string[] guids = AssetDatabase.FindAssets(fileName); 
                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path.Contains(fileName) && path.EndsWith(".asset"))
                        {
                            self = AssetDatabase.LoadAssetAtPath<LuaConnectConfig>(path);
                        }                     
                    }
                }
                return self;
            }          
        }

        public AssetWithPath ExcelFolder;

        public AssetWithPath OutputLuaFolder;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LuaConnectConfig))]
    public class LuaConnectConfigEditor : Editor
    {
        private SerializedObject serializedObject;
        private SerializedProperty ExcelFolder, OutputLuaFolder;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(target);
            ExcelFolder = serializedObject.FindProperty("ExcelFolder");
            OutputLuaFolder = serializedObject.FindProperty("OutputLuaFolder");
            var s =  LuaConnectConfig.Instance;
        }


        public override void OnInspectorGUI()
        {           
            serializedObject.Update();
            EditorGUILayout.LabelField("LuaConnectConfig", EditorDrawer.ConnectTitleStyle);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert all excel files in the ExcelFolder to lua files in the OutputLuaFolder when there are changes.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(ExcelFolder);
            EditorGUILayout.PropertyField(OutputLuaFolder);        
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}


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
        private SerializedObject csharpObject;
        private SerializedProperty ExcelFolder, OutputLuaFolder;

        private void OnEnable()
        {
            csharpObject = new SerializedObject(target);
            ExcelFolder = csharpObject.FindProperty("ExcelFolder");
            OutputLuaFolder = csharpObject.FindProperty("OutputLuaFolder");
            var s =  LuaConnectConfig.Instance;
        }


        public override void OnInspectorGUI()
        {           
            csharpObject.Update();
            EditorGUILayout.LabelField("LuaConnectConfig", EditorDrawer.ConnectTitleStyle);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert all excel files in the ExcelFolder to lua files in the OutputLuaFolder when there are changes.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(ExcelFolder);
            EditorGUILayout.PropertyField(OutputLuaFolder);        
            csharpObject.ApplyModifiedProperties();
        }
    }
#endif
}


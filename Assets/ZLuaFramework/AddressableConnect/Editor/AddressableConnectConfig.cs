using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    [CreateAssetMenu(fileName = "AddressableConnectConfig", menuName = "ZLuaFrameworkConfigs/AddressableConnectConfig", order = 1)]
    public class AddressableConnectConfig : ScriptableObject
    {
        private static AddressableConnectConfig self;
        public static AddressableConnectConfig Instance
        {
            get
            {
                if (self == null)
                {
                    string fileName = "AddressableConnectConfig";
                    string[] guids = AssetDatabase.FindAssets(fileName);
                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path.Contains(fileName) && path.EndsWith(".asset"))
                        {
                            self = AssetDatabase.LoadAssetAtPath<AddressableConnectConfig>(path);
                        }
                    }
                }
                return self;
            }           
        }

        public AssetWithPath HoffixFolder;

        public AssetWithPath AddressKeyMapFile;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AddressableConnectConfig))]
    public class AddressableConnectConfigEditor : Editor
    {
        private SerializedObject serializedObject;
        private SerializedProperty HoffixFolder, AddressKeyMapFile;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(target);
            HoffixFolder = serializedObject.FindProperty("HoffixFolder");
            AddressKeyMapFile = serializedObject.FindProperty("AddressKeyMapFile");    
        }
        

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("AddressableConnectConfig", EditorDrawer.ConnectTitleStyle);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically mark addressable when file in HoffixFolder.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(HoffixFolder);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert all addressable keys to AddressKeyMapFile.(Optional)", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(AddressKeyMapFile);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}


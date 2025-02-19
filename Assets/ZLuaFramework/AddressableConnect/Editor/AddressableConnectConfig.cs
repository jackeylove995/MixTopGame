using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ZLuaFramework
{
    [CreateAssetMenu(fileName = "AddressableConnectConfig", menuName = "ZLuaFrameworkConfigs/AddressableConnectConfig", order = 1)]
    public class AddressableConnectConfig : ScriptableObject
    {
        private static AddressableConnectConfig self;
        public static AddressableConnectConfig GetAsset()
        {
            if (self == null)
            {
                self = AssetDatabase.LoadAssetAtPath<AddressableConnectConfig>("Assets/ZLuaFramework/AddressableConnect/AddressableConnectConfig.asset");
            }
            return self;
        }

        public Object HoffixFolder;
        public string HoffixFolderPath = "No object selected";

        public Object AddressKeyMapFolder;
        public string AddressKeyMapFolderPath = "No object selected";
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AddressableConnectConfig))]
    public class CustomDataObjectEditor : Editor
    {
        private SerializedObject serializedObject;
        private SerializedProperty HoffixFolder, HoffixFolderPath, AddressKeyMapFolder, AddressKeyMapFolderPath;
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
            HoffixFolder = serializedObject.FindProperty("HoffixFolder");
            HoffixFolderPath = serializedObject.FindProperty("HoffixFolderPath");
            AddressKeyMapFolder = serializedObject.FindProperty("AddressKeyMapFolder");
            AddressKeyMapFolderPath = serializedObject.FindProperty("AddressKeyMapFolderPath");      
        }
        

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("AddressableConnectConfig", titleStyle());

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically mark addressable when file in HoffixFolder.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(HoffixFolder);
            if(HoffixFolder.objectReferenceValue != null)
            {
                HoffixFolderPath.stringValue = AssetDatabase.GetAssetPath(HoffixFolder.objectReferenceValue);
                EditorGUILayout.LabelField("path:" + HoffixFolderPath.stringValue);
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Lua Support: All addressable keys will be read to lua file in AddressKeyMapFolder.(Optional)", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(AddressKeyMapFolder);
            if (AddressKeyMapFolder.objectReferenceValue != null)
            {
                AddressKeyMapFolderPath.stringValue = AssetDatabase.GetAssetPath(AddressKeyMapFolder.objectReferenceValue);
                EditorGUILayout.LabelField("path:" + AddressKeyMapFolderPath.stringValue);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}


using NUnit.Framework;
using System.Collections.Generic;
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

        public AssetWithPath[] UnpackToMultiGroups;

        public AssetWithPath AddressKeyMapFile;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AddressableConnectConfig))]
    public class AddressableConnectConfigEditor : Editor
    {
        private SerializedObject csharpObject;
        private SerializedProperty HoffixFolder, UnpackToMultiGroups, AddressKeyMapFile;

        private void OnEnable()
        {
            csharpObject = new SerializedObject(target);
            HoffixFolder = csharpObject.FindProperty("HoffixFolder");
            AddressKeyMapFile = csharpObject.FindProperty("AddressKeyMapFile");
            UnpackToMultiGroups = csharpObject.FindProperty("UnpackToMultiGroups");
        }
        

        public override void OnInspectorGUI()
        {
            csharpObject.Update();
            EditorGUILayout.LabelField("AddressableConnectConfig", EditorDrawer.ConnectTitleStyle);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically mark addressable when file in HoffixFolder.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(HoffixFolder);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Children folders in below folders will be marked as single module.(Optional)", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(UnpackToMultiGroups, true);


            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Automatically convert all addressable keys to AddressKeyMapFile.(Optional)", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(AddressKeyMapFile);

            csharpObject.ApplyModifiedProperties();
        }
    }
#endif
}


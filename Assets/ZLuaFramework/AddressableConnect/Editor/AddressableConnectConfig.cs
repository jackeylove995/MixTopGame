using UnityEditor;
using UnityEngine;

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

#if UNITY_EDITOR
        [CustomEditor(typeof(AddressableConnectConfig))]
        public class CustomDataObjectEditor : Editor
        {
            private AddressableConnectConfig dataObject;
            private GUIStyle title;
            private bool init;

            private void Init()
            {
                if (init) return;
                init = true;
                dataObject = (AddressableConnectConfig)target;
                title = new GUIStyle(EditorStyles.label);
                title.fontSize = 16;
                title.fontStyle = FontStyle.Bold;
            }

            public override void OnInspectorGUI()
            {
                Init();
                EditorGUILayout.LabelField("AddressableConnectConfig", title);

                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField("热更文件夹，其中所有资源会自动打Address标记.");
                dataObject.HoffixFolder = EditorGUILayout.ObjectField("HoffixFolder:", dataObject.HoffixFolder, typeof(Object), false);
                // 读取对象路径
                dataObject.HoffixFolderPath = dataObject.HoffixFolder != null ? AssetDatabase.GetAssetPath(dataObject.HoffixFolder) : "No object selected";
                EditorGUILayout.LabelField("path:" + dataObject.HoffixFolderPath);

                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField("自动打Address标记输出的lua文件配置,在Lua中作为全局变量可用.(可选)");
                dataObject.AddressKeyMapFolder = EditorGUILayout.ObjectField("AddressKeyMapFolder:", dataObject.AddressKeyMapFolder, typeof(Object), false);
                // 读取对象路径
                dataObject.AddressKeyMapFolderPath = dataObject.AddressKeyMapFolder != null ? AssetDatabase.GetAssetPath(dataObject.AddressKeyMapFolder) : "No object selected";
                EditorGUILayout.LabelField("path:" + dataObject.AddressKeyMapFolderPath);
            }
        }
#endif
    }
}


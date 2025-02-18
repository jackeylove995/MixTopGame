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


#if UNITY_EDITOR
        [CustomEditor(typeof(LocalizationConnectConfig))]
        public class CustomDataObjectEditor : Editor
        {
            private LocalizationConnectConfig dataObject;
            private GUIStyle title;
            private bool init;

            private void Init()
            {
                if (init) return;
                init = true;
                dataObject = (LocalizationConnectConfig)target;
                title = new GUIStyle(EditorStyles.label);
                title.fontSize = 16;
                title.fontStyle = FontStyle.Bold;
            }

            public override void OnInspectorGUI()
            {
                Init();
                EditorGUILayout.LabelField("LocalizationConnectConfig", title);

                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField("多语言Excel表格.");
                dataObject.Excel = EditorGUILayout.ObjectField("Excel:", dataObject.Excel, typeof(Object), false);
                // 读取对象路径
                dataObject.ExcelPath = dataObject.Excel != null ? AssetDatabase.GetAssetPath(dataObject.Excel) : "No object selected";
                EditorGUILayout.LabelField("path:" + dataObject.ExcelPath);

                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField("存放Excel表格转化为Lua的文件夹.");
                dataObject.OutputLuaKeyFolder = EditorGUILayout.ObjectField("OutputLuaFolder:", dataObject.OutputLuaKeyFolder, typeof(Object), false);
                dataObject.OutputLuaKeyFolderPath = dataObject.OutputLuaKeyFolder != null ? AssetDatabase.GetAssetPath(dataObject.OutputLuaKeyFolder) : "No object selected";
                EditorGUILayout.LabelField("path:" + dataObject.OutputLuaKeyFolderPath);
            }
        }
#endif
    }
}


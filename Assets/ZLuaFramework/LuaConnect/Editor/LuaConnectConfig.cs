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


#if UNITY_EDITOR
        [CustomEditor(typeof(LuaConnectConfig))]
        public class CustomDataObjectEditor : Editor
        {
            private LuaConnectConfig dataObject;
            private GUIStyle title;
            private bool init;
           
            private void Init()
            {
                if (init) return;
                init = true;
                dataObject = (LuaConnectConfig)target;
                title = new GUIStyle(EditorStyles.label);     
                title.fontSize = 16;
                title.fontStyle = FontStyle.Bold;
            }

            public override void OnInspectorGUI()
            {
                Init();
                EditorGUILayout.LabelField("LuaConnectConfig" , title);

                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField("���Excel�����ļ���.");             
                dataObject.ExcelFolder = EditorGUILayout.ObjectField("ExcelFolder:", dataObject.ExcelFolder, typeof(Object), false);
                // ��ȡ����·��
                dataObject.ExcelFolderPath = dataObject.ExcelFolder != null ? AssetDatabase.GetAssetPath(dataObject.ExcelFolder) : "No object selected";
                EditorGUILayout.LabelField("path:" + dataObject.ExcelFolderPath);

                EditorGUILayout.Space(20);

                EditorGUILayout.LabelField("���Excel����ļ���������Excel���ת��ΪLua���ļ���.");
                dataObject.OutputLuaFolder = EditorGUILayout.ObjectField("OutputLuaFolder:", dataObject.OutputLuaFolder, typeof(Object), false);
                dataObject.OutputLuaFolderPath = dataObject.ExcelFolder != null ? AssetDatabase.GetAssetPath(dataObject.OutputLuaFolder) : "No object selected";
                EditorGUILayout.LabelField("path:" + dataObject.OutputLuaFolderPath);
            }
        }
#endif
    }
}


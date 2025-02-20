using OfficeOpenXml.Drawing.Chart;
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZLuaFramework
{
    public class EditorDrawer
    {
        private static GUIStyle connectTitleStyle;
        public static GUIStyle ConnectTitleStyle
        {
            get
            {
                if (connectTitleStyle == null)
                {
                    connectTitleStyle = new GUIStyle(EditorStyles.label);
                    connectTitleStyle.fontSize = 16;
                    connectTitleStyle.fontStyle = FontStyle.Bold;
                }

                return connectTitleStyle;
            }
        }
    }

    [Serializable]
    public class AssetWithPath
    {
        public Object asset;
        public string assetPath;
    }

    [CustomPropertyDrawer(typeof(AssetWithPath))]
    public class AssetWithPathInspector : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string name = property.displayName;
            var asset = property.FindPropertyRelative("asset");
            var assetPath = property.FindPropertyRelative("assetPath");
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.ObjectField(position, asset, new GUIContent(name));

            if (asset.objectReferenceValue != null)
            {
                assetPath.stringValue = AssetDatabase.GetAssetPath(asset.objectReferenceValue);                          
            }
        }      
    }
}


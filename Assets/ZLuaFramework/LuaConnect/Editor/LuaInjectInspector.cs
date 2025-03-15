using UnityEditor;
using UnityEngine;

namespace ZLuaFramework
{
    [CustomPropertyDrawer(typeof(Injection))]
    public class LuaInjectInspector : PropertyDrawer
    {
        private const string NO_OBJECT = "No Object";
        private static readonly GenericMenu.MenuFunction2 s_OnItemClick = OnItemClick;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var nameProperty = property.FindPropertyRelative("name");
            var valueProperty = property.FindPropertyRelative("value");
            var height = EditorGUI.GetPropertyHeight(nameProperty) + EditorGUI.GetPropertyHeight(valueProperty) +
                         EditorGUIUtility.standardVerticalSpacing;
            if (valueProperty.propertyType == SerializedPropertyType.ObjectReference)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameProperty = property.FindPropertyRelative("name");
            var valueProperty = property.FindPropertyRelative("value");

            position.height = EditorGUI.GetPropertyHeight(nameProperty);
            EditorGUI.PropertyField(position, nameProperty);

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUI.GetPropertyHeight(valueProperty);
            DrawValueProperty(position,nameProperty, valueProperty);

           /* if (valueProperty.objectReferenceValue != null && !nameProperty.stringValue.Equals(valueProperty.objectReferenceValue.name))
            {
                nameProperty.stringValue = valueProperty.objectReferenceValue.name;
                nameProperty.serializedObject.ApplyModifiedProperties();
            }*/
        }

        protected void DrawValueProperty(Rect position, SerializedProperty name, SerializedProperty value)
        {
            EditorGUI.PropertyField(position, value);

            if (value.propertyType != SerializedPropertyType.ObjectReference)
                return;
            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;
            position.xMin += EditorGUIUtility.labelWidth;
            using (new EditorGUI.DisabledScope(!value.objectReferenceValue))
            {
                var displayText = NO_OBJECT;
                if (value.objectReferenceValue)
                    displayText = value.objectReferenceValue.GetType().Name;
                if (GUI.Button(position, displayText, EditorStyles.popup))
                    BuildPopupList(name, value).DropDown(position);
            }
        }

        private static GenericMenu BuildPopupList(SerializedProperty name, SerializedProperty value)
        {
            var menu = new GenericMenu();
            var target = value.objectReferenceValue;
            if (target is Component c)
                target = c.gameObject;
            if (target is GameObject gameObject)
            {
                menu.AddItem(EditorGUIUtility.TrTextContent(target.GetType().Name),
                    value.objectReferenceValue == target, s_OnItemClick,
                    new LuaInjectionObjectMenuEvent(name,value, target));
                var components = gameObject.GetComponents<Component>();
                foreach (var component in components)
                    menu.AddItem(EditorGUIUtility.TrTextContent(component.GetType().Name),
                        value.objectReferenceValue == component, s_OnItemClick,
                        new LuaInjectionObjectMenuEvent(name,value, component));
            }
            else if (EditorUtility.IsPersistent(target))
            {
                var path = AssetDatabase.GetAssetPath(target);
                var mainAsset = AssetDatabase.LoadMainAssetAtPath(path);
                menu.AddItem(EditorGUIUtility.TrTextContent(mainAsset.GetType().Name),
                    value.objectReferenceValue == mainAsset, s_OnItemClick,
                    new LuaInjectionObjectMenuEvent(name, value, mainAsset));
                var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                foreach (var asset in assets)
                    menu.AddItem(EditorGUIUtility.TrTextContent($"{asset.GetType().Name} ({asset.name})"),
                        value.objectReferenceValue == asset, OnItemClick,
                        new LuaInjectionObjectMenuEvent(name, value, asset));
            }
            else menu.AddDisabledItem(EditorGUIUtility.TrTextContent(target.GetType().Name));

            return menu;
        }

        private static void OnItemClick(object obj)
        {
            var evt = obj as LuaInjectionObjectMenuEvent;
            evt?.Apply();
        }

        internal sealed class LuaInjectionObjectMenuEvent
        {
            private readonly SerializedProperty m_nameProperty;
            private readonly SerializedProperty m_valueProperty;
            private readonly Object m_Target;

            public LuaInjectionObjectMenuEvent(SerializedProperty nameProperty, SerializedProperty valueProperty,  Object target)
            {
                m_nameProperty = nameProperty;
                m_valueProperty = valueProperty;
                m_Target = target;
            }

            public void Apply()
            {
               
                m_valueProperty.objectReferenceValue = m_Target;
                m_nameProperty.stringValue = m_valueProperty.objectReferenceValue.name;

                //m_nameProperty.serializedObject.ApplyModifiedProperties();
                m_valueProperty.serializedObject.ApplyModifiedProperties();          
            }
        }
    }
}


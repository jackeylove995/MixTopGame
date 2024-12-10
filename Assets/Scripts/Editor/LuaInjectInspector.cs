using UnityEditor;
using UnityEngine;

namespace MTG
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
            DrawValueProperty(position, valueProperty);

            if (valueProperty.objectReferenceValue != null && nameProperty.stringValue.Equals(string.Empty))
            {
                nameProperty.stringValue = valueProperty.objectReferenceValue.name;
                nameProperty.serializedObject.ApplyModifiedProperties();

            }
        }

        protected void DrawValueProperty(Rect position, SerializedProperty property)
        {
            EditorGUI.PropertyField(position, property);

            if (property.propertyType != SerializedPropertyType.ObjectReference)
                return;
            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;
            position.xMin += EditorGUIUtility.labelWidth;
            using (new EditorGUI.DisabledScope(!property.objectReferenceValue))
            {
                var displayText = NO_OBJECT;
                if (property.objectReferenceValue)
                    displayText = property.objectReferenceValue.GetType().Name;
                if (GUI.Button(position, displayText, EditorStyles.popup))
                    BuildPopupList(property).DropDown(position);
            }
        }

        private static GenericMenu BuildPopupList(SerializedProperty property)
        {
            var menu = new GenericMenu();
            var target = property.objectReferenceValue;
            if (target is Component c)
                target = c.gameObject;
            if (target is GameObject gameObject)
            {
                menu.AddItem(EditorGUIUtility.TrTextContent(target.GetType().Name),
                    property.objectReferenceValue == target, s_OnItemClick,
                    new LuaInjectionObjectMenuEvent(property, target));
                var components = gameObject.GetComponents<Component>();
                foreach (var component in components)
                    menu.AddItem(EditorGUIUtility.TrTextContent(component.GetType().Name),
                        property.objectReferenceValue == component, s_OnItemClick,
                        new LuaInjectionObjectMenuEvent(property, component));
            }
            else if (EditorUtility.IsPersistent(target))
            {
                var path = AssetDatabase.GetAssetPath(target);
                var mainAsset = AssetDatabase.LoadMainAssetAtPath(path);
                menu.AddItem(EditorGUIUtility.TrTextContent(mainAsset.GetType().Name),
                    property.objectReferenceValue == mainAsset, s_OnItemClick,
                    new LuaInjectionObjectMenuEvent(property, mainAsset));
                var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                foreach (var asset in assets)
                    menu.AddItem(EditorGUIUtility.TrTextContent($"{asset.GetType().Name} ({asset.name})"),
                        property.objectReferenceValue == asset, OnItemClick,
                        new LuaInjectionObjectMenuEvent(property, asset));
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
            private readonly SerializedProperty m_Property;
            private readonly Object m_Target;

            public LuaInjectionObjectMenuEvent(SerializedProperty property, Object target)
            {
                m_Property = property;
                m_Target = target;
            }

            public void Apply()
            {
                m_Property.objectReferenceValue = m_Target;
                m_Property.serializedObject.ApplyModifiedProperties();          
            }
        }
    }
}


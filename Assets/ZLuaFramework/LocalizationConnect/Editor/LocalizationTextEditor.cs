using TMPro;
using UnityEditor;

namespace ZLuaFramework
{
    [CustomEditor(typeof(LocalizationText))]
    public class LocalizationTextEditor : Editor
    {
        private SerializedObject serializedObject;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            if (serializedObject.FindProperty("text").objectReferenceValue == null)
            {
                var text = ((LocalizationText)target).GetComponent<TextMeshProUGUI>();

                serializedObject.FindProperty("text").objectReferenceValue = text;
                serializedObject.ApplyModifiedProperties();
            }
            DrawDefaultInspector();
        }

    }
}


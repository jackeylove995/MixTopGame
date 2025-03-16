using TMPro;
using UnityEditor;

namespace ZLuaFramework
{
    [CustomEditor(typeof(LocalizationText))]
    public class LocalizationTextEditor : Editor
    {
        private SerializedObject csharpObject;

        private void OnEnable()
        {
            csharpObject = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            if (csharpObject.FindProperty("text").objectReferenceValue == null)
            {
                var text = ((LocalizationText)target).GetComponent<TextMeshProUGUI>();

                csharpObject.FindProperty("text").objectReferenceValue = text;
                csharpObject.ApplyModifiedProperties();
            }
            DrawDefaultInspector();
        }

    }
}


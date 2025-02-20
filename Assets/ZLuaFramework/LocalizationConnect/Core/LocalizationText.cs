
using TMPro;
using UnityEngine.Localization.Components;

namespace ZLuaFramework
{
    public class LocalizationText : LocalizeStringEvent
    {
        public TextMeshProUGUI text;

        protected override void UpdateString(string value)
        {
            text.text = value;
        }
    }
}




using TMPro;
using UnityEngine;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class DebugUtil
    {
        public static TextMeshProUGUI debugText;
        public static TextMeshProUGUI DebugText
        {
            get
            {
                if (debugText == null)
                {
                    debugText = GameObject.Find("DebugText").GetComponent<TextMeshProUGUI>();
                }
                return debugText;
            }
        }

        public static void ShowDebugMes(string mes)
        {
            if (DebugText.text != string.Empty)
            {
                DebugText.text = DebugText.text + "\n" + mes;
            }
            else
            {
                DebugText.text = mes;
            }

        }

        public static void Log(string mes)
        {
            Debug.Log("LUA:" + mes);
        }

        public static void LogFormat(string mes, params object[] args)
        {
            Debug.LogFormat("LUA:" + mes, args);
        }

        public static void LogError(string mes)
        {
            Debug.LogError("LUA:" + mes);
        }
    }
}


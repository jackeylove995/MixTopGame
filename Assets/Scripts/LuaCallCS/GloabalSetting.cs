using UnityEngine;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class GlobalSetting
    {
        public static Transform UIRoot;
        public static Transform FullScreenPanelContainor;
        public static Transform PopupPanelContainor;

        public static void InitSetting()
        {
            UIRoot = GameObject.Find("UIRoot").transform;
            FullScreenPanelContainor = GameObject.Find("FullScreenPanelContainor").transform;
            PopupPanelContainor = GameObject.Find("PopupPanelContainor").transform;
        }

    }
}


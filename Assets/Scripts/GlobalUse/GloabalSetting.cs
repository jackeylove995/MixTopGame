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
        public static Transform Sprite3DContainor;

        public static Transform TMainCamera;
        public static void InitSetting()
        {
            UIRoot = GameObject.Find("UIRoot").transform;
            FullScreenPanelContainor = GameObject.Find("FullScreenPanelContainor").transform;
            PopupPanelContainor = GameObject.Find("PopupPanelContainor").transform;
            Sprite3DContainor = GameObject.Find("Sprite3DContainor").transform;
            TMainCamera = Camera.main.transform;
        }

    }
}


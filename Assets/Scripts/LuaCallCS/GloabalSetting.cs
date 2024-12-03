using UnityEngine;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class GlobalSetting
    {
        public static Transform UIRoot;
        public static Transform FullbackPanelContainor;
        public static Transform PopPanelContainor;

        public static void InitSetting()
        {
            UIRoot = GameObject.Find("UIRoot").transform;
            FullbackPanelContainor = GameObject.Find("FullbackPanelContainor").transform;
            PopPanelContainor = GameObject.Find("PopPanelContainor").transform;
        }

    }
}


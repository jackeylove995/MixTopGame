using UnityEngine;

namespace MTG
{
    public static class UIPanelSetting
    {
        private static Transform uiRoot;
        public static Transform UIRoot
        {
            get
            {
                if (null == uiRoot)
                {
                    uiRoot = GameObject.Find("UIRoot").transform;
                }
                return uiRoot;
            }
        }

        private static Transform fullbackPanelContainor;
        public static Transform FullbackPanelContainor
        {
            get
            {
                if (null == fullbackPanelContainor)
                {
                    fullbackPanelContainor = GameObject.Find("FullbackPanelContainor").transform;
                }
                return fullbackPanelContainor;
            }
        }

        private static Transform popPanelContainor;
        public static Transform PopPanelContainor
        {
            get
            {
                if (null == popPanelContainor)
                {
                    popPanelContainor = GameObject.Find("PopPanelContainor").transform;
                }
                return popPanelContainor;
            }
        }

    }
}


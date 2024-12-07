
using UnityEngine;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class UIUtil
    {
        public static void Size(GameObject gameObject, float width, float height)
        {
            RectTransform rttransform = gameObject.transform as RectTransform;
            rttransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rttransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static void Size(Transform transform, float width, float height)
        {
            RectTransform rttransform = transform as RectTransform;
            rttransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rttransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}

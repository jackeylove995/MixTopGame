
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace MTG
{
    [LuaCallCSharp]
    public static class UnityUtil
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

        public static void LocalMove(Transform transform, float x, float y, float z)
        {
            transform.localPosition +=  new Vector3(x, y, z);
        }

        public static void SetLocalPostion(Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        public static void SetLocalPostion(Transform transform, float x, float y)
        {
            transform.localPosition = new Vector3(x, y);
        }

        public static void SetLocalZ(Transform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;
        }
    }
}


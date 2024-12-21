using UnityEngine;

namespace MTG
{
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
            transform.localPosition += new Vector3(x, y, z);
        }

        public static void SetLocalPosition(Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        public static void SetLocalPosition(Transform transform, Vector3 pos)
        {
            transform.localPosition = pos;
        }

        public static void SetLocalPosition(Transform transform, float x, float y)
        {
            transform.localPosition = new Vector3(x, y);
        }

        public static void SetPosition(Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }
        public static void SetLocalZ(Transform transform, float z)
        {
            var pos = transform.localPosition;
            pos.z = z;
            transform.localPosition = pos;
        }

        public static void SetZ(Transform transform, float z)
        {
            var pos = transform.position;
            pos.z = z;
            transform.position = pos;
        }
    }
}


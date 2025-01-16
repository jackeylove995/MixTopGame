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

        public static void BackMove(Transform mover, Transform two)
        {
            mover.position += (mover.position - two.position).normalized;
        }   

        public static void MoveToTargetBySpeed(Transform mover, Transform target, float speed)
        {
            Vector3 normal = (target.position - mover.position).normalized;
            mover.position += normal * speed;
        }
    }
}


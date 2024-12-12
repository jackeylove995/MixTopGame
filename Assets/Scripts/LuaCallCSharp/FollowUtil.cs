using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    /// <summary>
    /// 跟随工具
    /// </summary>
    public static class FollowUtil
    {
        /// <summary>
        /// used in MonoManager.fixedUpdate
        /// </summary>
        public static Dictionary<Transform, Transform> followMap
                  = new Dictionary<Transform, Transform>();
        public static void FollowTargetXY(Transform source, Transform target)
        {
            followMap[source] = target;
        }

        public static void UnFollow(Transform source)
        {
            if(followMap.ContainsKey(source))
            {
                followMap.Remove(source);
            }
        }
    }
}


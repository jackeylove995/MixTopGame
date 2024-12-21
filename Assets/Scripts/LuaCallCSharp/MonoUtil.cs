using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    public class MonoUtil : MonoBehaviour
    {
        public static void Init()
        {
            GameObject go = new GameObject("MonoUtil");
            DontDestroyOnLoad(go);
            go.AddComponent<MonoUtil>();
            mUpdateMap = new Dictionary<string, Action>();
        }

        private static Dictionary<string, Action> mUpdateMap;
        private static Action mUpdate;

        public static void AddUpdate(string name, Action action)
        {
            if(mUpdateMap.ContainsKey(name))
            {
                Debug.LogError("There already a same key in update, which name is " + name);
                return;
            }
            mUpdate += action;
            mUpdateMap.Add(name, action);
        }

        public static void RemoveUpdate(string name)
        {
            if (mUpdateMap.TryGetValue(name, out Action action))
            {
                mUpdate -= action;
                mUpdateMap.Remove(name);
            }

        }

        void Update()
        {
            mUpdate?.Invoke();
        }

        void FixedUpdate()
        {
            foreach (var fow in FollowUtil.followMap)
            {
                var z = fow.Key.position.z;
                var newPos = fow.Value.position;
                newPos.z = z;
                fow.Key.position = newPos;
            }
        }
    }
}


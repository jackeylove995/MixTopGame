using System;
using UnityEngine;

namespace MTG
{
    public class MonoManager : MonoBehaviour
    {
        private static MonoManager instance;
        public static MonoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("MonoManager");
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<MonoManager>();
                }
                return instance;
            }
        }

        private Action mUpdate;

        public void AddUpdate(Action action)
        {
            mUpdate += action;
        }

        public void RemoveUpdate(Action action)
        {
            mUpdate -= action;
        }

        void Update()
        {
            mUpdate?.Invoke();
        }

        void FixedUpdate()
        {
            foreach(var fow in FollowUtil.followMap)
            {
                var z = fow.Key.position.z;
                var newPos = fow.Value.position;
                newPos.z = z;
                fow.Key.position = newPos;
            }
        }
    }
}


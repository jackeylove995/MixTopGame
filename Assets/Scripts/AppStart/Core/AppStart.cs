using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MTG
{
    public class AppStart : MonoBehaviour
    {
        private List<AppInitTask> mAppInitTasks;

        void Awake()
        {
            InvokeGameStart();
        }

        public void InvokeGameStart()
        {
            StartCoroutine(ExecuteStartTasks());
        }

        public IEnumerator ExecuteStartTasks()
        {
            Debug.Log("App Start Init ...");

            mAppInitTasks = GetComponentsInChildren<AppInitTask>().Where(x => x.enabled).ToList();
            foreach (var task in mAppInitTasks)
            {
                yield return task.DOTask();
                Debug.Log("Task:" + task.GetType());
            }

            Debug.Log("App Init Successfully!");

            mAppInitTasks.ForEach(task => task.OnAllTasksInitSuccessfully());

            Destroy(gameObject);
        }
    }
}


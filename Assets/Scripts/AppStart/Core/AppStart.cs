using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    public class AppStart : MonoBehaviour
    {
        public enum TaskType
        {
            Lua
        }

        public Dictionary<TaskType, Type> Map = new Dictionary<TaskType, Type>()
        {
            { TaskType.Lua, typeof(LuaInitTask) }
        };


        public List<TaskType> AppInitTasks;
        
        void Awake()
        {
            Init();
        }

        public void Init()
        {
            transform.RotateAround(transform.position, Vector3.back, 0.025f);
            StartCoroutine(ExecuteStartTasks());
        }

        public IEnumerator ExecuteStartTasks()
        {
            Debug.Log("App Start Init ...");
            
            List<IAppInitTask> tasks = new List<IAppInitTask>();
            foreach(var taskType in AppInitTasks)
            {
                IAppInitTask task = Activator.CreateInstance(Map[taskType]) as IAppInitTask;
                tasks.Add(task);
                yield return task.DOTask();
                Debug.Log("Task:" + task.GetType());
            }
            Debug.Log("App Init Successfully!");
            tasks.ForEach( task => task.OnAllTasksInitSuccessfully());
            Destroy(gameObject);
        }
    }
}


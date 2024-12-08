using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Assertions.Must;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MTG
{
    public class GameManager : MonoBehaviour
    {
        public List<ITask> StartGameTasks = new List<ITask>();
        private void Awake()
        {
            StartGameTasks.Add(new LuaInitTask());
            StartCoroutine(ExecuteStartTasks());
        }

        public IEnumerator ExecuteStartTasks()
        {
            foreach (ITask task in StartGameTasks)
            {
                Debug.Log("Task: " + task.GetType() + " Start");
                yield return task.DOTask();
            }
        }
    }

}

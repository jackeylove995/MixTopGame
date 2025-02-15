using System.Collections;
using UnityEngine;

namespace MTG
{
    public abstract class AppInitTask : MonoBehaviour
    {
        public bool enabled = true;
        public abstract IEnumerator DOTask();
        public virtual void OnAllTasksInitSuccessfully() {}
    }
}


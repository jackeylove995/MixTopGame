using System.Collections;
using UnityEngine;

namespace MTG
{
    public abstract class AppInitTask : MonoBehaviour
    {
        public new bool enabled = true;
        public abstract IEnumerator DOTask();
        public virtual void OnAllTasksInitSuccessfully() {}
    }
}


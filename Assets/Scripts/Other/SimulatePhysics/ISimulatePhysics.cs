using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    public interface ISimulatePhysics 
    {
        public bool CheckColliderEnter();

        public void OnColliderEnter(ISimulatePhysics other);
    }
}


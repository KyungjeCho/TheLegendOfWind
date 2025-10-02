using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum BTNodeState
    {
        None = -1,
        Success = 0,
        Failure = 1,
        Running = 2
    }

    public abstract class BTNode
    {
        protected BTNodeState state;

        public BTNodeState State => state;

        public abstract BTNodeState Evaluate(float deltaTime);
    }
}

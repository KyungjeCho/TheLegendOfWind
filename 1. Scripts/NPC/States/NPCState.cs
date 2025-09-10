using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class NPCState : ScriptableObject
    {
        public virtual void OnInitialize()
        {

        }
        public virtual void OnStartState(Transform tr)
        {

        }
        public virtual void Act(NPCStateMachine stateMachine)
        {
            
        }
        public virtual void OnEndState(Transform tr)
        {

        }
    }
}
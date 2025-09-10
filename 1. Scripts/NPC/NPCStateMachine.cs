using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class NPCStateMachine
    {
        public NPCState currentState;

        public Transform myTransform;
        public NPCStateMachine(NPCState defaultState, Transform tr)
        {
            currentState = defaultState;   
            myTransform = tr;
        }
        public void ChangeState(NPCState state)
        {
            currentState.OnEndState(myTransform);
            currentState = state;
            currentState.OnStartState(myTransform);
        }
    }
}
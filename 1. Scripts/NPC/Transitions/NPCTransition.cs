using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "NPC Transition", menuName = "ScriptableObjects/NPC/Transitions/NPC Transition")]
    public class NPCTransition : ScriptableObject 
    {
        public NPCState nextState;

        public void Transit(NPCStateMachine stateMachine)
        {
            stateMachine.ChangeState(nextState);
        }
    }
}
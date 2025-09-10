using KJ.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "NPC Dialog State", menuName = "ScriptableObjects/NPC/States/NPC Dialog State")]
    public class NPCDialogState : NPCState
    {
        public string dialogId;
        public NPCTransition nextTransition;

        public override void Act(NPCStateMachine stateMachine)
        {
            Debug.Log(this.GetType().Name);
            DialogManager.Instance.StartDialog(dialogId);

            nextTransition.Transit(stateMachine);
        }
    }
}
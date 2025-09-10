using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "NPC Quest State", menuName = "ScriptableObjects/NPC/States/NPC Quest State")]
    public class NPCQuestState : NPCState
    {
        public string preAcceptDialog;
        public string inProgressDialog;
        public string completeDialog;

        public QuestSO questSO;
        public NPCTransition nextTransition;

        public Transform myTr;
        public override void OnStartState(Transform tr)
        {
            myTr = tr;
            MultiDialogNonPlayerController controller = tr.GetComponent<MultiDialogNonPlayerController>();
            controller?.ShowQuestionMark1();

            GameEvent.OnQuestCompleted += OnCompleted;
        }
        public override void Act(NPCStateMachine stateMachine)
        {
            if (!QuestManager.Instance.IsCurrentQuest(questSO) && !QuestManager.Instance.IsCompletedQuest(questSO))
            {
                DialogManager.Instance.StartDialog(preAcceptDialog);
                return;
            }
            if (QuestManager.Instance.IsCurrentQuest(questSO))
            {
                if (QuestManager.Instance.CompleteQuest(questSO))
                {
                    DialogManager.Instance.StartDialog(completeDialog);
                    nextTransition.Transit(stateMachine);
                    return;
                }

                DialogManager.Instance.StartDialog(inProgressDialog);
                return;
            }
        }
        public override void OnEndState(Transform tr)
        {
            MultiDialogNonPlayerController controller = tr.GetComponent<MultiDialogNonPlayerController>();
            controller?.HideQuestionMark();
        }
        public void OnCompleted(QuestSO questSO)
        {
            if (this.questSO != questSO) 
            { 
                return; 
            }
            MultiDialogNonPlayerController controller = myTr.GetComponent<MultiDialogNonPlayerController>();
            controller?.ShowQuestionMark2();
        }
    }

}

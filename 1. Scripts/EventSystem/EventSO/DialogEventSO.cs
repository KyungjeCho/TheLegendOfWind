using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Dialog Event", menuName = "ScriptableObjects/Events/Dialog Event")]
    public class DialogEventSO : BaseEventSO
    {
        public string dialogId;

        public override bool DoEvent(BaseTrigger trigger)
        {
            DialogManager.Instance.StartDialog(dialogId);
            return true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DialogEventObject : EventObject
    {
        public string dialogId;

        public override void OnEventEnter()
        {
            DialogManager.Instance.StartDialog(dialogId);
        }

        public override void OnEventExit()
        {
            throw new System.NotImplementedException();
        }
    }
}

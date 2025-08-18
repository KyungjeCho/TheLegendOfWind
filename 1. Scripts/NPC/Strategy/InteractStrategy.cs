using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KJ.CameraControl;

namespace KJ
{
    public interface IInteractable
    {
        void Interact();
    }

    public class DialogInteract : IInteractable
    {
        [SerializeField]
        private string dialogId;

        public void SetDialogId(string dialogId) { this.dialogId = dialogId; }

        public DialogInteract(string dialogId)
        {
            this.dialogId = dialogId;
        }

        public void Interact()
        {
            DialogManager.Instance.StartDialog(dialogId);
        }
    }

    public class StoreInteract : IInteractable
    {
        // todo : 상점 인터렉트 용 UI 설계 후 로직 구현
        public void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}

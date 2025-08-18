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
        // todo : ���� ���ͷ�Ʈ �� UI ���� �� ���� ����
        public void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}

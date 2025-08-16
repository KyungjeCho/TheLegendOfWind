using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIDialogPanelController : MonoBehaviour
    {
        [SerializeField]
        private Text nameText = null;
        [SerializeField]
        private Text dialogText = null;

        private DialogObject currentDialog = null;

        public void ShowDialog(DialogObject dialogObject)
        {
            if (dialogObject == null)
            {
                return;
            }
            nameText.text = dialogObject.speaker;
            dialogText.text = dialogObject.dialog;

            currentDialog = dialogObject;
        }
        public void ShowNextDialog()
        {
            if (currentDialog == null || currentDialog.nextId == null)
            {
                return;
            }
            DialogObject dialogObject = DialogManager.Instance.GetDialogObject(currentDialog.nextId);
            ShowDialog(dialogObject);
        }
    }
}

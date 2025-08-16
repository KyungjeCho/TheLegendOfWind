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
        [SerializeField]
        private GameObject choicesPanel = null;
        [SerializeField]
        private Text choices1Txt = null;
        [SerializeField]
        private Text choices2Txt = null;
        [SerializeField]
        private Text nextTxt = null;

        private DialogObject currentDialog = null;

        public void ShowDialog(DialogObject dialogObject)
        {
            if (dialogObject == null)
            {
                return;
            }
            nameText.text = dialogObject.speaker;
            dialogText.text = dialogObject.dialog;

            if (dialogObject.choices == string.Empty)
            {
                choicesPanel.SetActive(false);
                nextTxt.gameObject.SetActive(true);
            }
            else
            {
                choicesPanel.SetActive(true);
                nextTxt.gameObject.SetActive(false);

                var choices = dialogObject.choices.Split('|');
                choices1Txt.text = choices[0] + " [F]";
                choices2Txt.text = choices[1] + " [G]";
            }
            
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

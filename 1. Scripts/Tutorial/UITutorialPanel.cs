using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UITutorialPanel : MonoBehaviour
    {
        public Text descriptionText;
        public SoundList openSound;
        public SoundList closeSound;

        private bool isPanelOpen = false;

        private void Update()
        {
            if (isPanelOpen && InputManager.Instance.InteractButton.IsButtonPressed)
            {
                ClosePanel();
            }
        }
        public void SetDescription(TutorialList tutorial)
        {
            string description = DataManager.TutorialData.GetCopy((int)tutorial).description;

            descriptionText.text = description;

            DataManager.TutorialData.data[(int)tutorial].isCleared = true;
            DataManager.TutorialData.SaveData();
        }

        public void OpenPanel(TutorialList tutorial)
        {
            if (DataManager.TutorialData.data[(int)tutorial].isCleared != true)
            {
                gameObject.SetActive(true);

                SetDescription(tutorial);
                InputManager.GetOrCreateInstance().ChangeDialogStrategy();
                SoundManager.GetOrCreateInstance().PlayUISound((int)openSound);
                isPanelOpen = true;
            }
        }

        public void ClosePanel()
        {
            InputManager.GetOrCreateInstance().ChangeNormalStrategy();
            SoundManager.GetOrCreateInstance().PlayUISound((int)openSound);
            gameObject.SetActive(false);
        }
    }
}
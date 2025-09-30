using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UISkillDescriptionPanel : MonoBehaviour
    {
        public Image iconImg;
        public Text titleText;
        public Text descriptionText;

        public Action<BaseSkill> onClosed;

        private BaseSkill skill;
        private bool isOpened = false;

        private void Update()
        {
            if (isOpened)
            {
                if (InputManager.Instance.InteractButton.IsButtonPressed)
                {
                    ClosePanel();
                    InputManager.Instance.InteractButton.IsButtonPressed = false;
                }
            }
        }
        public void OpenPanel(BaseSkill skill, Action<BaseSkill> onClosed)
        {
            if (iconImg == null || titleText == null || descriptionText == null)
            {
                Debug.LogError("Set UI");
                return;
            }
            this.skill = skill;
            titleText.text = skill.SkillName;
            descriptionText.text = skill.SkillDescription;
            this.onClosed = onClosed;
            isOpened = true;
            gameObject.SetActive(isOpened);
        }
        public void ClosePanel()
        {
            isOpened = false;
            gameObject.SetActive(isOpened);
            onClosed?.Invoke(skill);
        }
    }
}
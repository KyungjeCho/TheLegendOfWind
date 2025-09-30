using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KJ
{
    public class BookController : InteractComponent
    {
        public UnlockList unlockList;
        public BaseSkill skill;
        public UISkillDescriptionPanel skillDescriptionPanel;
        public UnityEvent onUIDismissed;

        public override void Interact()
        {
            if (skillDescriptionPanel == null || skill == null)
                return;

            skillDescriptionPanel.OpenPanel(skill, OnUIDismissed);
            InputManager.Instance.ChangeDialogStrategy();
        }

        private void OnUIDismissed(BaseSkill skill)
        {
            DataManager.UnlockData.SetIsUnlocked(unlockList, true);
            InputManager.Instance.ChangeNormalStrategy();
            onUIDismissed?.Invoke();
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIQuestInfo : MonoBehaviour
    {
        [SerializeField]
        private GameObject questInfoPanel;

        [NonSerialized]
        public QuestSO questSO;

        public void SetQuestSO(QuestSO questSO) { this.questSO = questSO;}

        public void OpenUIQuestInfo(QuestSO questSO)
        {
            questInfoPanel.SetActive(true);
            SetQuestSO(questSO);
            InputManager.Instance.ChangeDialogStrategy();
            EventBusSystem.Publish(EventBusType.STOP);
            questInfoPanel.transform.GetChild(0).GetComponent<Text>().text = questSO.questName;
            questInfoPanel.transform.GetChild(1).GetComponent<Text>().text = questSO.questDescription;
            questInfoPanel.transform.GetChild(2).GetComponent<Text>().text = questSO.rewards.ToString();
        }

        public void AcceptQuest()
        {
            QuestManager.Instance.AddQuest(questSO);
            questInfoPanel.SetActive(false);

            questSO = null;
            InputManager.Instance.ChangeNormalStrategy();
            EventBusSystem.Publish(EventBusType.START);
        }

        public void DenyQuest()
        {
            questSO = null;
            questInfoPanel.SetActive(false);
            InputManager.Instance.ChangeNormalStrategy();
            EventBusSystem.Publish(EventBusType.START);
        }
    }
}
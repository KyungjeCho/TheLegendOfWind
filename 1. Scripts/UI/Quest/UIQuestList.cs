using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KJ
{
    public class UIQuestList : MonoBehaviour
    {
        [SerializeField]
        private GameObject slotPrefab;

        [SerializeField]
        private Transform contentTr;

        [SerializeField]
        private Text questNameText;
        [SerializeField]
        private Text questContentText;

        private Dictionary<GameObject, QuestSlot> slotUIs;

        private void Awake()
        {
            slotUIs= new Dictionary<GameObject, QuestSlot>();
        }
        private void OnEnable()
        {
            slotUIs.Clear();

            CreateSlots();

            foreach (var questSlot in slotUIs.Values)
            {
                questSlot.OnPostUpdate += OnPostUpdate;
                questSlot.UpdateSlot(questSlot.quest);
            }
        }

        private void OnDisable()
        {
            foreach(var go in slotUIs.Keys)
            {
                Destroy(go);
            }
            slotUIs.Clear();
        }
        public void CreateSlots()
        {
            // Quest Manager ->  Current Quest List 
            for (int i = 0; i < QuestManager.Instance.CurrentQuests.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, contentTr);

                AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

                QuestSlot newSlot = new QuestSlot(QuestManager.Instance.CurrentQuests[i]);
                newSlot.slotUI = go;
                slotUIs.Add(go, newSlot);
                go.name += ": " + i;
            }
        }

        public void OnPostUpdate(QuestSlot slot)
        {
            slot.slotUI.transform.GetComponentInChildren<Text>().text = slot.QuestSO.questName;
        }
        private void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (!trigger)
            {
                Debug.LogWarning("No EventTrigger component found!");
                return;
            }
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        private void OnClick(GameObject go, PointerEventData data)
        {
            QuestSlot slot = slotUIs[go];
            
            questNameText.text = slot.QuestSO.questName;
            questContentText.text = "";
            foreach (Requirement requirement in slot.Quest.Requirements)
            {
                questContentText.text += requirement.ToString() + "\n";
            }
        }

        private void OnLeftClick()
        {

        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class QuestSlot
    {
        public Quest quest;

        public QuestSO QuestSO => quest.QuestSO;

        [NonSerialized]
        public GameObject slotUI;
        [NonSerialized]
        public Action<QuestSlot> OnPreUpdate;
        [NonSerialized]
        public Action<QuestSlot> OnPostUpdate;


        public Quest Quest => quest;

        public QuestSlot(Quest quest)
        {
            UpdateSlot(quest);
        }

        public void UpdateSlot(Quest quest)
        {
            if (OnPreUpdate != null)
            {
                OnPreUpdate.Invoke(this);
            }
            this.quest = quest;
            if (OnPostUpdate != null)
            {
                OnPostUpdate.Invoke(this);
            }
        }
    }

}

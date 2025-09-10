using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum QuestState
    {
        NotStarted,
        InProgress,
        Completed,
        Rewarded
    }
    [Serializable]
    public class Quest
    {
        private QuestSO questSO;
        private QuestState state = QuestState.NotStarted;
        private int completedQuestAmount = 0;
        private int requireQuestAmount = 0;
        private List<Requirement> requirements = new List<Requirement>();

        public bool IsCompleted => completedQuestAmount >= requireQuestAmount;
        public QuestSO QuestSO => questSO;

        public List<Requirement> Requirements => requirements;

        public Quest(QuestSO questSO)
        {
            this.questSO = questSO;
            state = QuestState.InProgress;
            requireQuestAmount = this.questSO.requirements.Count;

            foreach(var requirement in this.questSO.requirements)
            {
                Requirement r = requirement.CreateRequirement(this);
                requirements.Add(r);
            }
        }

        public void CompleteRequirement()
        {
            completedQuestAmount += 1;
            if (completedQuestAmount >= requireQuestAmount)
            { 
                state = QuestState.Completed;
                GameEvent.PublishQuestCompleted(questSO);
            }
        }
    }
}

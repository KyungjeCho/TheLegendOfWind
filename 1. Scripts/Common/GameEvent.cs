using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public static class GameEvent
    {
        public static event Action<MonsterList> OnMonsterKilled;
        public static event Action<ItemSO, int> OnItemChanged;
        public static event Action<string> OnDialogExit;
        public static event Action<QuestSO> OnQuestCompleted;

        public static void PublishMonsterKilled(MonsterList monsterList)
        {
            OnMonsterKilled?.Invoke(monsterList);
        }
        public static void PublishQuestCompleted(QuestSO questSO)
        {
            OnQuestCompleted?.Invoke(questSO);
        }
    }
}

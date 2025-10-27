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
        //public static event Action<string> OnDialogExit;
        public static event Action<QuestSO> OnQuestCompleted;
        public static event Action<UnlockList> OnUnlocked;

        public static void PublishMonsterKilled(MonsterList monsterList)
        {
            OnMonsterKilled?.Invoke(monsterList);
        }
        public static void PublishQuestCompleted(QuestSO questSO)
        {
            OnQuestCompleted?.Invoke(questSO);
        }
        public static void PublishItemChanged(ItemSO itemSO, int amount)
        {
            OnItemChanged?.Invoke(itemSO, amount);
        }
        public static void PublishUnlocked(UnlockList unlockedList)
        {
            OnUnlocked?.Invoke(unlockedList);
        }
    }
}

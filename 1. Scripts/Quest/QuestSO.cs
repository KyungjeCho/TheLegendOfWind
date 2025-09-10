using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum QuestType
    {
        None = -1,
        MainQuest,
        SubQuest
    }

    [Serializable]
    public class ItemReward
    {
        public int amount;
        public ItemSO itemSO;
    }

    [Serializable]
    public class Reward
    {
        public int gold;
        public int exp;
        public List<ItemReward> items;

        public override string ToString()
        {
            string rewardText = string.Empty;
            rewardText += "보상\n";
            rewardText += gold + " 골드\n";
            rewardText += exp + " 경험치\n";
            foreach (ItemReward i in items)
            {
                rewardText += i.itemSO.itemName + " " + i.amount + "\n";
            }
            return rewardText;
        }
    }
    [CreateAssetMenu(fileName = "Quest SO", menuName = "ScriptableObjects/Quest SO")]
    public class QuestSO : ScriptableObject
    {
        public string questName;
        [TextArea(15, 20)]
        public string questDescription;
        public NPCList npcId;
        public QuestType questType;
        public int requireLV;
        public QuestSO requireQuest;
        public Reward rewards;
        public List<BaseRequirementSO> requirements;
        public BaseEventSO acceptEvent;
        public BaseEventSO denyEvent;
    }
}

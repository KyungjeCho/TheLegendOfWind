using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public abstract class Requirement 
    {
        private Quest quest;

        public Quest Quest
        { 
            get { return quest; } 
            set { quest = value; }
        }

        public abstract bool IsCompleted();
    }

    public class HuntingRequirement : Requirement
    {
        private MonsterList targetMonster;
        private int targetCount;
        public int currentCount;

        public int TargetCount => targetCount;
        public MonsterList TargetMonster => targetMonster;

        public HuntingRequirement(HuntingRequirementSO huntingRequirementSO)
        {
            targetMonster = huntingRequirementSO.targetMonster;
            targetCount = huntingRequirementSO.requireCount;
            currentCount = 0;

            GameEvent.OnMonsterKilled += KilledMonster;
        }
        ~HuntingRequirement()
        {
            GameEvent.OnMonsterKilled -= KilledMonster;
        }
        public void KilledMonster(MonsterList targetMonster)
        {
            if (targetMonster == this.targetMonster)
            {
                currentCount++;
            }
            if (IsCompleted())
            {
                Quest.CompleteRequirement();
                GameEvent.OnMonsterKilled -= KilledMonster;
            }
        }

        public override bool IsCompleted()
        {
            return targetCount <= currentCount;
        }
        public override string ToString()
        {
            return DataManager.MonsterData.names[(int)targetMonster] + ": " + currentCount + " / " + targetCount;
        }
    }
    public class CollectingRequirement : Requirement
    {
        private ItemSO targetItem;
        private int targetCount;
        public int currentCount;

        public int TargetCount => targetCount;
        public ItemSO TargetItem => targetItem;
        
        public CollectingRequirement(CollectingRequirementSO collectingRequirementSO)
        {
            targetItem = collectingRequirementSO.targetItemSO;
            targetCount = collectingRequirementSO.requireCount;
            currentCount = 0;

            GameEvent.OnItemChanged += OnItemChanged;
        }
        ~CollectingRequirement()
        {
            GameEvent.OnItemChanged -= OnItemChanged;
        }

        public void OnItemChanged(ItemSO itemSO, int amount)
        {
            if (itemSO.itemName == targetItem.itemName)
            {
                currentCount = amount;
            }

            if (IsCompleted())
            {
                GameEvent.OnItemChanged -= OnItemChanged;
            }
        }
        public override bool IsCompleted()
        {
            return targetCount <= currentCount;
        }

        public override string ToString()
        {
            return targetItem.name + ": " + currentCount + " / " + targetCount;
        }
    }
}

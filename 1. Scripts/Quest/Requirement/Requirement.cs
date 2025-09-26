using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public abstract class Requirement 
    {
        public Quest quest;

        public abstract bool IsCompleted();
    }

    public class HuntingRequirement : Requirement
    {
        private MonsterList targetMonster;
        private int targetCount;
        public int currentCount;

        public int TargetCount => targetCount;
        public MonsterList TargetMonster => targetMonster;

        public HuntingRequirement(HuntingRequirementSO huntingRequirementSO, Quest q)
        {
            quest = q;
            targetMonster = huntingRequirementSO.targetMonster;
            targetCount = huntingRequirementSO.requireCount;
            currentCount = 0;

            GameEvent.OnMonsterKilled += KilledMonster;
            LoadTargetCount();
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
                SaveTargetCount();
            }
            if (IsCompleted())
            {
                quest.CompleteRequirement();
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

        public void SaveTargetCount()
        {
            string filePath = "Assets/9. Resources/Resources/Data";
            string fileName = quest.QuestSO.name + targetMonster.ToString();

            string path = Path.Combine(filePath, fileName);
            File.WriteAllText(path, currentCount.ToString());
        }
        public void LoadTargetCount()
        {
            string filePath = "Assets/9. Resources/Resources/Data";
            string fileName = quest.QuestSO.name + targetMonster.ToString();

            string path = Path.Combine(filePath, fileName);

            if(!File.Exists(path))
            {
                return;
            }
            try
            {
                string content = File.ReadAllText(path);
                currentCount = int.Parse(content);
            }
            catch (Exception e1)
            {
                Debug.Log("Load Error : " + e1.Message);
            }
        }
    }
    public class CollectingRequirement : Requirement
    {
        private ItemSO targetItem;
        private int targetCount;
        public int currentCount;

        public int TargetCount => targetCount;
        public ItemSO TargetItem => targetItem;
        
        public CollectingRequirement(CollectingRequirementSO collectingRequirementSO, Quest q)
        {
            quest = q;
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
            if (itemSO == null)
            {
                return;
            }
            if (itemSO.itemName == targetItem.itemName)
            {
                currentCount = amount;
            }

            if (IsCompleted())
            {
                quest.CompleteRequirement();
                GameEvent.OnItemChanged -= OnItemChanged;
            }
        }
        public override bool IsCompleted()
        {
            return targetCount <= currentCount;
        }

        public override string ToString()
        {
            return targetItem.itemName + ": " + currentCount + " / " + targetCount;
        }
    }
}

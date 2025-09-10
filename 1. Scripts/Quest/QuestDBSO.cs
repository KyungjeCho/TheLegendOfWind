using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Quest DB", menuName = "ScriptableObjects/Quest DB")]
    public class QuestDBSO : ScriptableObject
    {
        public List<QuestSO> container;

        public QuestSO SearchQuestSO(string questName)
        {
            foreach(QuestSO item in container)
            {
                if (item.questName.Equals(questName))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    //[CreateAssetMenu(fileName = "Quest Log", menuName = "ScriptableObjects/Quest/Quest Log")]
    //public class QuestLogSO : MonoBehaviour
    //{
    //    [SerializeField]
    //    private QuestDBSO db;

    //    [SerializeField]
    //    private List<Quest> quests = new List<Quest>();

    //    [SerializeField]
    //    private QuestLogSO completedLogSO;

    //    public bool IsInQuests(QuestSO questSO)
    //    {
    //        return quests.Exists(q => q.QuestSO == questSO);
    //    }

    //    public bool AddQuest(QuestSO questSO)
    //    {
    //        if (quests.Exists(q => q.QuestSO == questSO))
    //        {
    //            return false;
    //        }
    //        if (completedLogSO != null && completedLogSO.IsInQuests(questSO))
    //        {
    //            return false;
    //        }

    //        quests.Add(new Quest(questSO));
    //        return true;
    //    }
    //}
}

using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuest : MonoBehaviour
{
    public QuestSO questSO;
    public UIQuestInfo questInfo;
    // Start is called before the first frame update
    void Start()
    {
        questInfo.OpenUIQuestInfo(questSO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

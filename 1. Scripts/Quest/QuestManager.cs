using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class QuestManager : SingletonMonoBehaviour<QuestManager>
    {
        [SerializeField]
        private QuestDBSO db;

        private string currentQuestsJsonName = "currentQuestData.txt";
        private string completedQuestsJsonName = "completedQuestData.txt";
        private string jsonPath = "Assets/9. Resources/Resources/Data";

        private Quest[] currentQuests;
        //private List<Quest> currentQuests = new List<Quest>();
        private List<string> completedQuests = new List<string>();

        public Quest[] CurrentQuests => currentQuests;
        //public List<Quest> CurrentQuests => currentQuests;
        public List<string> CompletedQuests => completedQuests;

        private void Start()
        {
            currentQuests = new Quest[0];
            LoadText();
        }
        private void Update()
        {
            //int idx = 0;
            //while (idx < currentQuests.Count)
            //{
            //    if (currentQuests[idx].IsCompleted)
            //    {
            //        completedQuests.Add(currentQuests[idx].QuestSO.questName);
            //        currentQuests.RemoveAt(idx);
            //    }
            //    else
            //        idx++;
            //}
        }
        //private void OnDestroy()
        //{
        //    SaveText();
        //}
        public QuestSO GetQuestSO(string questName)
        {
            return db.SearchQuestSO(questName);
        }
        public Quest GetQuest(QuestSO questSO)
        {
            foreach(Quest quest in currentQuests)
            {
                if (quest.QuestSO == questSO)
                {
                    return quest;
                }
            }
            return null;
        }
        public bool IsCurrentQuest(QuestSO questSO)
        {
            foreach (Quest quest in currentQuests)
            {
                if (quest.QuestSO == questSO) return true;
            }
            return false;
        }
        public bool IsCompletedQuest(QuestSO questSO)
        {
            foreach(string quest in completedQuests)
            {
                if (quest == questSO.questName) return true;
            }
            return false;
        }

        public bool AddQuest(QuestSO questSO)
        {
            if (questSO == null || IsCurrentQuest(questSO) || IsCompletedQuest(questSO))
                return false;
            Quest newQuest = new Quest(questSO);
            currentQuests = ArrayHelper.Add<Quest>(newQuest, currentQuests);
            SaveText();
            return true;
        }
        
        public bool CompleteQuest(QuestSO questSO)
        {
            if (!IsCurrentQuest(questSO) || IsCompletedQuest(questSO))
                return false;

            for (int i = 0; i < currentQuests.Length; i++)
            { 
                if (currentQuests[i].QuestSO == questSO)
                {
                    if (currentQuests[i].IsCompleted)
                    {
                        completedQuests.Add(currentQuests[i].QuestSO.name);
                        currentQuests = ArrayHelper.Remove<Quest>(i, currentQuests);
                        SaveText();
                        return true;
                    }
                }
            }

            //foreach (Quest quest in currentQuests)
            //{
            //    if (quest.QuestSO == questSO)
            //    {
            //        if (quest.IsCompleted)
            //        {
            //            completedQuests.Add(quest.QuestSO.questName);
            //            ArrayHelper.Remove<Quest>(quest, currentQuests);
            //            currentQuests.Remove(quest);
            //            SaveJson();
            //            return true;
            //        }
            //    }
            //}
            return false;
        }

        public void SaveText()
        {
            string content = "";
            string path = Path.Combine(jsonPath, currentQuestsJsonName);
            foreach (Quest q in currentQuests)
            {
                content += q.QuestSO.questName + "\n";
            }
            File.WriteAllText(path, content);

            content = "";
            path = Path.Combine(jsonPath, completedQuestsJsonName);
            foreach (string s in completedQuests)
            {
                content = s + "\n";
            }
            File.WriteAllText(path, content);
        }

        public void LoadText()
        {
            string path = Path.Combine(jsonPath, currentQuestsJsonName);
            try
            {
                string content = File.ReadAllText(path);
                Debug.Log(content);
                string[] splitContent = content.Split('\n');
                for (int i = 0; i < splitContent.Length; i++)
                {
                    Quest q = new Quest(db.SearchQuestSO(splitContent[i]));
                    currentQuests = ArrayHelper.Add<Quest>(q, currentQuests);
                }
            }
            catch  (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }

            path = Path.Combine(jsonPath, completedQuestsJsonName);

            try
            {
                string content = File.ReadAllText(path);
                string[] splitContent = content.Split('\n');
                for (int i = 0; i < splitContent.Length; i++)
                {
                    completedQuests.Add(splitContent[i]);
                }
            }
            catch (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }
        }

        public void ResetQuests()
        {
            completedQuests.Clear();
            currentQuests = new Quest[0];
            SaveText();
        }
    }
}
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

        private string currentQuestsJsonName = "currentQuestData.json";
        private string completedQuestsJsonName = "completedQuestData.json";
        private string jsonPath = "Assets/9. Resources/Resources/Data";

        private Quest[] currentQuests = new Quest[0];
        //private List<Quest> currentQuests = new List<Quest>();
        private List<string> completedQuests = new List<string>();

        public Quest[] CurrentQuests => currentQuests;
        //public List<Quest> CurrentQuests => currentQuests;
        public List<string> CompletedQuests => completedQuests;

        private void Start()
        {
            LoadJson();
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
        private void OnDestroy()
        {
            SaveJson();
        }
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
            SaveJson();
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
                        SaveJson();
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

        public void SaveJson()
        {
            JsonWrapData<Quest> wrapper = new JsonWrapData<Quest>(currentQuests, new string[0]); 

            string path = Path.Combine(jsonPath, currentQuestsJsonName);
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(path, json);

            path = Path.Combine(jsonPath, completedQuestsJsonName);
            json = JsonUtility.ToJson(completedQuests, true);
            File.WriteAllText(path, json);
        }

        public void LoadJson()
        {
            string path = Path.Combine(jsonPath, currentQuestsJsonName);

            try
            {
                string json = File.ReadAllText(path);
                JsonWrapData<Quest> wrapper = JsonUtility.FromJson<JsonWrapData<Quest>>(json);
                currentQuests = wrapper.data;
            }
            catch  (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }

            path = Path.Combine(jsonPath, completedQuestsJsonName);

            try
            {
                string json = File.ReadAllText(path);
                completedQuests = JsonUtility.FromJson<List<string>>(json);
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
            SaveJson();
        }
    }
}
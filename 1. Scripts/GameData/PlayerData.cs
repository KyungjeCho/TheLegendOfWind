using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class PlayerStat
    {
        public int level            = 1;
        public float hp             = 0f;
        public float mana           = 0f;
        public float stemina        = 0f;
        public float exp            = 0f;
        public int gold             = 0;
        public int remainSkillPoint = 0; // 필요할까? 
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public const string dataDirectory = "Assets/9. Resources/Resources/Data";

        public PlayerStat stat;

        [HideInInspector]
        public string jsonFilePath = "";
        [HideInInspector]
        public string jsonFileName = "playerStat.json";
        
        public void SaveData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);
            string json = JsonUtility.ToJson(stat, true);
            File.WriteAllText(path, json);
        }
        public void LoadData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);
            try
            {
                string jsonData = File.ReadAllText(path);
                PlayerStat jsonPlayerStatData = JsonUtility.FromJson<PlayerStat>(jsonData);
                stat = jsonPlayerStatData;
            }
            catch (Exception e1)
            {
                stat.level = 1;
                PlayerLVStat levelStat = DataManager.PlayerLVData.GetCopyFromLevel(stat.level);
                stat.hp = levelStat.maxHp;
                stat.mana = levelStat.maxMana;
                Debug.Log("Error reading the file: " + e1.Message);
            }
        }

        public static void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Hello World");
        }
    }

}


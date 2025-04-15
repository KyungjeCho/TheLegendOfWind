using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class MonsterData : BaseData
    {
        public MonsterStat[] monsterStats = null;

        public string jsonFilePath = "";
        public string jsonFileName = "monsterData.json";

        public void SaveData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);
            JsonWrapData<MonsterStat> jsonWrapData = new JsonWrapData<MonsterStat>(monsterStats, names);
            string json = JsonUtility.ToJson(jsonWrapData, true);
            File.WriteAllText(path, json);
        }

        public void LoadData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);

            try
            {
                string jsonData = File.ReadAllText(path);

                JsonWrapData<MonsterStat> jsonWrapData = JsonUtility.FromJson<JsonWrapData<MonsterStat>>(jsonData);

                monsterStats = jsonWrapData.data;
                names = jsonWrapData.names;
            }
            catch (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }
        }

        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                monsterStats = new MonsterStat[] { new MonsterStat() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                monsterStats = ArrayHelper.Add(new MonsterStat(), monsterStats);
            }
            return GetDataCount();
        }
        public override void RemoveData(int index)
        {
            if (names.Length == 0)
            {
                names = null;
                monsterStats = null;
                return;
            }

            names = ArrayHelper.Remove(index, names);
            monsterStats = ArrayHelper.Remove(index, monsterStats);
        }

        public MonsterStat GetCopy(int index)
        {
            if (index < 0 || index >= monsterStats.Length)
                return null;

            MonsterStat copy = new MonsterStat();
            MonsterStat original = monsterStats[index];
            copy.monsterType    = original.monsterType; 
            copy.maxHP          = original.maxHP;
            copy.attack         = original.attack;
            copy.defense        = original.defense;
            copy.attackRange    = original.attackRange;
            copy.speed          = original.speed;
            copy.dropExp        = original.dropExp;
            return copy;
        }
    }
}


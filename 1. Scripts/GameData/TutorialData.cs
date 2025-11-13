using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class TutorialData : BaseData<TutorialClip>
    {
        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                data = new TutorialClip[] { new TutorialClip() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                data = ArrayHelper.Add(new TutorialClip(), data);
            }
            return GetDataCount();
        }
        public override void RemoveData(int index)
        {
            if (names.Length == 0)
            {
                names = null;
                data = null;
                return;
            }

            names = ArrayHelper.Remove(index, names);
            data = ArrayHelper.Remove(index, data);
        }

        public TutorialClip GetCopy(int index)
        {
            if (index < 0 || index >= data.Length)
                return null;

            TutorialClip copy = new TutorialClip();
            TutorialClip original = data[index];
            copy.description = original.description;
            copy.isCleared = original.isCleared;
            return copy;
        }

        public override void LoadData()
        {
            string path = Path.Combine(Application.persistentDataPath, jsonFilePath, jsonFileName);

            try
            {
                string jsonData = File.ReadAllText(path);

                JsonWrapData<TutorialClip> jsonWrapData = JsonUtility.FromJson<JsonWrapData<TutorialClip>>(jsonData);

                data = jsonWrapData.data;
                names = jsonWrapData.names;
            }
            catch (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }
            //base.LoadData();
        }
        public override void SaveData()
        {
            string path = Path.Combine(Application.persistentDataPath, jsonFilePath, jsonFileName);
            JsonWrapData<TutorialClip> jsonWrapData = new JsonWrapData<TutorialClip>(data, names);
            string json = JsonUtility.ToJson(jsonWrapData, true);
            File.WriteAllText(path, json);
        }
    }
}
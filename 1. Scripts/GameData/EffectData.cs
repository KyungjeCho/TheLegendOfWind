using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace KJ
{
    public class EffectData : BaseData
    {
        public EffectClip[] effectClips = null;

        public string jsonFilePath = "";
        public string jsonFileName = "effectData.json";

        public void SaveData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);
            JsonWrapData<EffectClip> jsonWrapData = new JsonWrapData<EffectClip>(effectClips, names);
            string json = JsonUtility.ToJson(jsonWrapData, true);
            File.WriteAllText(path, json);
        }

        public void LoadData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);

            try
            {
                string jsonData = File.ReadAllText(path);

                JsonWrapData<EffectClip> jsonSoundData = JsonUtility.FromJson<JsonWrapData<EffectClip>>(jsonData);

                effectClips = jsonSoundData.data;
                names = jsonSoundData.names;
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
                effectClips = new EffectClip[] { new EffectClip() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                effectClips = ArrayHelper.Add(new EffectClip(), effectClips);
            }
            return GetDataCount();
        }
        public override void RemoveData(int index)
        {
            if (names.Length == 0)
            {
                names = null;
                effectClips = null;
                return;
            }

            names = ArrayHelper.Remove(index, names);
            effectClips = ArrayHelper.Remove(index, effectClips);
        }

        public EffectClip GetCopy(int index)
        {
            if (index < 0 || index >= effectClips.Length)
                return null;

            EffectClip copy = new EffectClip();
            EffectClip original = effectClips[index];
            copy.clipPath = original.clipPath;
            copy.clipName = original.clipName;
            copy.PreLoad();
            return copy;
        }
    }

}

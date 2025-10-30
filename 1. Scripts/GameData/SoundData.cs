using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class JsonSoundData
    {
        public SoundClip[] clips;
        public string[] names;

        public JsonSoundData(SoundClip[] clips, string[] names)
        {
            this.clips = clips;
            this.names = names;
        }
    }

    public class SoundData : BaseData
    {
        public SoundClip[] soundClips = null;

        private string jsonFilePath = "";
        private string jsonFileName = "soundData.json";
        private string dataPath = "Data/soundData";

        public void SaveData()
        {
            string path = Path.Combine(dataDirectory, jsonFileName);
            JsonSoundData jsonSoundData = new JsonSoundData(soundClips, names);
            string json = JsonUtility.ToJson(jsonSoundData, true);
            File.WriteAllText(path, json);
        }

        public void LoadData()
        {
            jsonFilePath = Application.dataPath + dataDirectory;
            TextAsset asset = (TextAsset) Resources.Load(dataPath, typeof(TextAsset));
            if (asset == null || asset.text == null)
            {
                this.AddData("NewSound");
                return;
            }
            JsonSoundData jsonSounData = JsonUtility.FromJson<JsonSoundData>(asset.text);
            soundClips = jsonSounData.clips;
            names = jsonSounData.names;

            //string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);

            //try
            //{
            //    string jsonData = File.ReadAllText(path);

            //    JsonSoundData jsonSoundData = JsonUtility.FromJson<JsonSoundData>(jsonData);

            //    soundClips = jsonSoundData.clips;
            //    names = jsonSoundData.names;
            //}
            //catch (Exception e1)
            //{
            //    Debug.Log("Error reading the file: " + e1.Message);
            //}
        }

        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                soundClips = new SoundClip[] { new SoundClip() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                soundClips = ArrayHelper.Add(new SoundClip(), soundClips);
            }
            return GetDataCount();
        }
        public override void RemoveData(int index) 
        {
            if (names.Length == 0)
            {
                names = null;
                soundClips = null;
                return;
            }

            names = ArrayHelper.Remove(index, names);
            soundClips = ArrayHelper.Remove(index, soundClips);
        }

        public SoundClip GetCopy(int index)
        {
            if (index < 0 || index >= soundClips.Length)
                return null;

            SoundClip copy      = new SoundClip();
            SoundClip original  = soundClips[index];
            copy.clipPath       = original.clipPath;
            copy.clipName       = original.clipName;
            copy.isLoop         = original.isLoop;
            copy.minDistance = original.minDistance;   
            copy.maxDistance = original.maxDistance;   
            copy.playType = original.playType;
            copy.PreLoad();
            return copy;
        }
    }
}


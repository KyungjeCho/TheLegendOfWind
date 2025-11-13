using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class RespawnData : BaseData<RespawnClip>
    {
        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                data = new RespawnClip[] { new RespawnClip() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                data = ArrayHelper.Add(new RespawnClip(), data);
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
        public RespawnClip GetCopy(int index)
        {
            if (index < 0 || index >= data.Length)
                return null;

            RespawnClip copy = new RespawnClip();
            RespawnClip original = data[index];
            copy.posX = original.posX;
            copy.posY = original.posY;
            copy.posZ = original.posZ;
            copy.rotX = original.rotX;
            copy.rotY = original.rotY;
            copy.rotZ = original.rotZ;
            return copy;
        }
        public RespawnClip GetCopyByName(string name)
        {
            int index = -1;
            for (int i = 0; i < names.Length; i++)
            {
                if (name.Equals(names[i]))
                {
                    index = i; break;
                }
            }

            return index == -1 ? null : GetCopy(index);
        }
        public bool SetData(int index, RespawnClip copy)
        {
            if (copy == null) return false;

            data[index].posX = copy.posX;
            data[index].posY = copy.posY;
            data[index].posZ = copy.posZ;
            data[index].rotX = copy.rotX;
            data[index].rotY = copy.rotY;
            data[index].rotZ = copy.rotZ;
            return true;
        }
        public bool SetData(string name, RespawnClip copy)
        {
            int index = -1;
            for (int i = 0; i < names.Length; i++)
            {
                if (name.Equals(names[i]))
                {
                    index = i; break;
                }
            }

            return SetData(index, copy);
        }

        public override void LoadData()
        {
            string path = Path.Combine(Application.persistentDataPath, jsonFilePath, jsonFileName);

            try
            {
                string jsonData = File.ReadAllText(path);

                JsonWrapData<RespawnClip> jsonWrapData = JsonUtility.FromJson<JsonWrapData<RespawnClip>>(jsonData);

                data = jsonWrapData.data;
                names = jsonWrapData.names;
            }
            catch (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }
        }
        public override void SaveData()
        {
            string path = Path.Combine(Application.persistentDataPath, jsonFilePath, jsonFileName);
            JsonWrapData<RespawnClip> jsonWrapData = new JsonWrapData<RespawnClip>(data, names);
            string json = JsonUtility.ToJson(jsonWrapData, true);
            File.WriteAllText(path, json);
        }
    }
}


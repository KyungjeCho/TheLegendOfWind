using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 데이터 기본 클래스
    /// 
    /// </summary>
    public class BaseData : ScriptableObject
    {
        public const string dataDirectory = "Assets/9. Resources/Resources/Data";
        public string[] names = null;

        public int GetDataCount() => names != null ? names.Length : 0;

        public string[] GetNameList(bool indexing)
        {
            if (names == null)
                return new string[0];

            if (indexing)
                return names
                    .Select((x, i) => i + ". " + x).ToArray();
            else
                return names;
        }

        public virtual int AddData(string name) { return GetDataCount(); }
        public virtual void RemoveData(int index) { }
    }

    public class BaseData<T> : ScriptableObject where T : class
    {
        public const string dataDirectory = "Assets/9. Resources/Resources/Data";
        public string[] names = null;
        public T[] data = null;

        public string jsonFilePath = "";
        public string jsonFileName = typeof(T).ToString() + ".json";
        public int GetDataCount() => names != null ? names.Length : 0;

        public string[] GetNameList(bool indexing)
        {
            if (names == null)
            {
                return new string[0];
            }

            if (indexing)
            {
                return names.Select((x, i) => i + ". " + x).ToArray();
            }
            else
            {
                return names;
            }
        }

        public void SetJsonFilePath(string filePath) => jsonFilePath = filePath;
        public void SetJsonFileName(string fileName) => jsonFileName = fileName;

        public void SetJsonFilePathAndName(string filePath, string fileName)
        {
            SetJsonFilePath(filePath);
            SetJsonFileName(fileName);
        }

        public virtual int AddData(string name) 
        {
            return GetDataCount(); 
        }

        public virtual void RemoveData(int index) { }

        public virtual void SaveData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);
            JsonWrapData<T> jsonWrapData = new JsonWrapData<T>(data, names);
            string json = JsonUtility.ToJson(jsonWrapData, true);
            File.WriteAllText(path, json);
        }

        public virtual void LoadData()
        {
            string path = Path.Combine(dataDirectory, jsonFilePath, jsonFileName);

            try
            {
                string jsonData = File.ReadAllText(path);

                JsonWrapData<T> jsonWrapData = JsonUtility.FromJson<JsonWrapData<T>>(jsonData);

                data = jsonWrapData.data;
                names = jsonWrapData.names;
            }
            catch (Exception e1)
            {
                Debug.Log("Error reading the file: " + e1.Message);
            }
        }
    }
}


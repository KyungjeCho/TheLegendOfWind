using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Dialog DB", menuName = "ScriptableObjects/Dialog DB")]
    public class DialogDB : ScriptableObject
    {
        [SerializeField]
        private IDictionary<string, DialogObject> container = new Dictionary<string, DialogObject>();
        private string fileName = "dialogData.csv";
        private string path = "9. Resources/Resources/Data/";

        public IDictionary<string, DialogObject> Container => container;

        public void LoadCSV()
        {
            string fullPath = path + fileName;

            StreamReader reader = new StreamReader(fullPath);

            container.Clear();

            while (true)
            {
                string line = reader.ReadLine();

                if (line == null)
                {
                    break;
                }

                var splitData = line.Split(',');

                DialogObject dialogObject = new DialogObject();
                dialogObject.id = splitData[0];
                dialogObject.speaker = splitData[1];
                dialogObject.dialog = splitData[2];
                dialogObject.nextId = splitData[3];
                dialogObject.choices = splitData[4];
                dialogObject.choicesNextId = splitData[5];
                dialogObject.trigger = splitData[6];

                container.Add(dialogObject.id, dialogObject);
            }

            Debug.Log(container.Count);
        }
    }
}
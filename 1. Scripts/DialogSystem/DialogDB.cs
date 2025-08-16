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
        private List<DialogObject> container = new List<DialogObject>();
        private string fileName = "dialogData.csv";
        private string path = "Assets/9. Resources/Resources/Data/";

        public List<DialogObject> Container => container;

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
                dialogObject.nextId = splitData[3] != "None" ? splitData[3] : null;
                dialogObject.choices = splitData[4] != "None" ? splitData[4] : null;
                dialogObject.choicesNextId = splitData[5] != "None" ? splitData[5] : null;
                dialogObject.trigger = splitData[6] != "None" ? splitData[6] : null;

                container.Add(dialogObject);
            }
        }
    }
}
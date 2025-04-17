using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace  KJ
{
    public class DataManagementHelper
    {
        public static string GetPath(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            string[] pathNode = path.Split('/');
            bool findResource = false;
            for (int i = 0; i < pathNode.Length - 1; i++)
            {
                if (findResource == false)
                {
                    if (pathNode[i] == "Resources")
                    {
                        findResource = true;
                        path = string.Empty;
                    }
                }
                else
                {
                    path += pathNode[i] + "/";
                }
            }

            return path;
        }

        public static void CreateEnumList(string enumName, StringBuilder data)
        {
            string templateFilePath = "Assets/1. Scripts/Editor/EnumTemplate.txt";

            string template = File.ReadAllText(templateFilePath);

            template = template.Replace("$DATA$", data.ToString());
            template = template.Replace("$ENUM$", enumName);
            string folderPath = "Assets/1. Scripts/GameData/";
            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);

            string filePath = folderPath + enumName + ".cs";
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.WriteAllText(filePath, template);
        }

        public static void CreateEnumStructure(string enumName, BaseData data)
        {
            if (data == null)
            {
                return;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.names.Length; i++)
            {
                builder.AppendLine("    " + data.names[i] + " = " + i.ToString() + ",");
            }
            DataManagementHelper.CreateEnumList(enumName, builder);
        }

        public static void CreateEnumStructure(string enumName, string[] names)
        {
            if (names == null)
            {
                return;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                builder.AppendLine("    " + names[i] + " = " + i.ToString() + ",");
            }
            DataManagementHelper.CreateEnumList(enumName, builder);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    public class NPCTool : EditorWindow
    {
        private int selection = 0;
        private static NPCData data;
        private GameObject npcSource;

        private static NPCTool window;
        [MenuItem("Tools/NPC Tool")]
        private static void Init()
        {
            data = CreateInstance<NPCData>();
            data.LoadData();

            window = GetWindow<NPCTool>();
            window.Show();
            window.titleContent = new GUIContent("NPC Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<NPCData>();
                    data.LoadData();
                }
                return;
            }

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    // List
                    EditorGUILayout.BeginVertical(GUILayout.Width(300));
                    {
                        EditorGUILayout.Separator();
                        EditorGUILayout.BeginVertical("box");
                        {
                            EditorGUILayout.BeginScrollView(Vector2.zero);
                            {
                                if (data.GetDataCount() > 0)
                                {
                                    selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
                                }
                            }
                            EditorGUILayout.EndScrollView();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndVertical();

                    // Inspection
                    EditorGUILayout.BeginVertical();
                    {
                        npcSource = null;
                        EditorGUILayout.BeginScrollView(Vector2.zero);
                        {
                            if (data.GetDataCount() > 0)
                            {
                                EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(450));
                                data.names[selection] = EditorGUILayout.TextField("Name", data.names[selection], GUILayout.Width(450));
                                if (npcSource == null && data.data[selection].clipName != string.Empty)
                                {
                                    data.data[selection].PreLoad();
                                    npcSource = Resources.Load(data.data[selection].clipPath + data.data[selection].clipName) as GameObject;
                                }
                                npcSource = (GameObject)EditorGUILayout.ObjectField("NPC Clip", npcSource, typeof(GameObject), false, GUILayout.Width(450));

                                if (npcSource != null)
                                {
                                    data.data[selection].clipPath = DataManagementHelper.GetPath(npcSource);
                                    data.data[selection].clipName = npcSource.name;

                                    Debug.Log(data.data[selection].clipPath);
                                    Debug.Log(data.data[selection].clipName);
                                }
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();

                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            // footer
            Object source = npcSource;
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add"))
                {
                    data.AddData("New");
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                if (GUILayout.Button("Remove"))
                {
                    if (data.GetDataCount() > 0)
                    {
                        source = null;
                        data.RemoveData(selection);
                    }
                }

                if (selection > data.GetDataCount() - 1)
                    selection = data.GetDataCount() - 1;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save"))
                {
                    data.SaveData();
                    DataManagementHelper.CreateEnumStructure("NPCList", data.names);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
                if (GUILayout.Button("Load"))
                {
                    data = CreateInstance<NPCData>();
                    data.LoadData();
                    selection = 0;
                    npcSource = null;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}

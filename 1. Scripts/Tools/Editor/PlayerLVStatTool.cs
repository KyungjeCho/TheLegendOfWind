using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    public class PlayerLVStatTool : EditorWindow
    {
        private int selection = 0;
        private static PlayerLVData data;

        private static PlayerLVStatTool window;
        private GUILayoutOption fieldLayoutOption = GUILayout.Width(450);

        [MenuItem("Tools/Player Level Stat Tool")]
        public static void Init()
        {
            data = CreateInstance<PlayerLVData>();
            data.LoadData();

            window = GetWindow<PlayerLVStatTool>();
            window.Show();
            window.titleContent = new GUIContent("Player Level Stat Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<PlayerLVData>();
                    data.LoadData();
                }
                return;
            }

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    // List GUI
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

                    // Inspection GUI
                    EditorGUILayout.BeginVertical();
                    {

                        EditorGUILayout.BeginScrollView(Vector2.zero);
                        {
                            if (data.GetDataCount() > 0)
                            {
                                EditorGUILayout.LabelField("ID", selection.ToString(), fieldLayoutOption);
                                GUI.enabled = false;
                                data.names[selection] = EditorGUILayout.TextField("Name", "Level_" + (selection + 1).ToString(), fieldLayoutOption);
                                GUI.enabled = true; // 원상복구
                                data.data[selection].level = EditorGUILayout.IntField("Level", data.data[selection].level, fieldLayoutOption);
                                data.data[selection].maxHp = EditorGUILayout.FloatField("Max HP", data.data[selection].maxHp, fieldLayoutOption);
                                data.data[selection].maxMana = EditorGUILayout.FloatField("Max MANA", data.data[selection].maxMana, fieldLayoutOption);
                                data.data[selection].maxStemina = EditorGUILayout.FloatField("Max STEMINA", data.data[selection].maxStemina, fieldLayoutOption);
                                data.data[selection].attack = EditorGUILayout.FloatField("Attack", data.data[selection].attack, fieldLayoutOption);
                                data.data[selection].defense = EditorGUILayout.FloatField("Defense", data.data[selection].defense, fieldLayoutOption);
                                data.data[selection].exp = EditorGUILayout.FloatField("EXP", data.data[selection].exp, fieldLayoutOption);
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Add"))
                    {
                        data.AddData("New");
                        selection = data.GetDataCount() - 1;
                    }
                    if (GUILayout.Button("Remove"))
                    {
                        if (data.GetDataCount() > 0)
                        {
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
                        DataManagementHelper.CreateEnumStructure("PlayerLVList", data.names);
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                    if (GUILayout.Button("Load"))
                    {
                        data = CreateInstance<PlayerLVData>();
                        data.LoadData();
                        selection = 0;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace KJ
{
   
    public class MonsterStatTool : EditorWindow
    {
        private int selection = 0;
        private static MonsterData data;

        private static MonsterStatTool window;

        [MenuItem("Tools/Monster Stat Tool")]
        public static void Init()
        {
            data = CreateInstance<MonsterData>();
            data.LoadData();

            window = GetWindow<MonsterStatTool>();
            window.Show();
            window.titleContent = new GUIContent("Monster Stat Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<MonsterData>();
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
                                EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(450));
                                data.names[selection]                       = EditorGUILayout.TextField("Name", data.names[selection], GUILayout.Width(450));
                                data.monsterStats[selection].monsterType    = (MonsterType)EditorGUILayout.EnumPopup("Monster Type", data.monsterStats[selection].monsterType, GUILayout.Width(450));
                                data.monsterStats[selection].maxHP          = EditorGUILayout.FloatField("Max HP", data.monsterStats[selection].maxHP, GUILayout.Width(450));
                                if (data.monsterStats[selection].maxHP < 0)
                                {
                                    data.monsterStats[selection].maxHP = 0f;
                                }
                                data.monsterStats[selection].attack         = EditorGUILayout.FloatField("Attack", data.monsterStats[selection].attack, GUILayout.Width(450));
                                if (data.monsterStats[selection].attack < 0)
                                {
                                    data.monsterStats[selection].attack = 0f;
                                }
                                data.monsterStats[selection].defense        = EditorGUILayout.FloatField("Defense", data.monsterStats[selection].defense, GUILayout.Width(450));
                                if (data.monsterStats[selection].defense < 0)
                                {
                                    data.monsterStats[selection].defense = 0f;
                                }
                                data.monsterStats[selection].attackRange    = EditorGUILayout.FloatField("Attack Range", data.monsterStats[selection].attackRange, GUILayout.Width(450));
                                if (data.monsterStats[selection].attackRange < 0)
                                {
                                    data.monsterStats[selection].attackRange = 0f;
                                }
                                data.monsterStats[selection].speed          = EditorGUILayout.FloatField("Speed", data.monsterStats[selection].speed, GUILayout.Width(450));
                                if (data.monsterStats[selection].speed < 0)
                                {
                                    data.monsterStats[selection].speed = 0f;
                                }
                                data.monsterStats[selection].dropExp        = EditorGUILayout.FloatField("Drop Exp", data.monsterStats[selection].dropExp, GUILayout.Width(450));
                                if (data.monsterStats[selection].dropExp < 0)
                                {
                                    data.monsterStats[selection].dropExp = 0f;
                                }
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
                        DataManagementHelper.CreateEnumStructure("MonsterList", data);
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                    if (GUILayout.Button("Load"))
                    {
                        data = CreateInstance<MonsterData>();
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


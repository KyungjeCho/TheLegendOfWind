using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    public class RespawnTool : EditorWindow
    {
        private int selection = 0;
        private static RespawnData data;

        private static RespawnTool window;
        private GUILayoutOption fieldLayoutOption = GUILayout.Width(450);

        [MenuItem("Tools/Respawn Tool")]
        public static void Init()
        {
            data = CreateInstance<RespawnData>();
            data.LoadData();

            window = GetWindow<RespawnTool>();
            window.Show();
            window.titleContent = new GUIContent("Respawn Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<RespawnData>();
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
                                data.names[selection] = EditorGUILayout.TextField("Scene Name", data.names[selection], fieldLayoutOption);
                                data.data[selection].posX = EditorGUILayout.FloatField("Pos X", data.data[selection].posX, fieldLayoutOption);
                                data.data[selection].posY = EditorGUILayout.FloatField("Pos Y", data.data[selection].posY, fieldLayoutOption);
                                data.data[selection].posZ = EditorGUILayout.FloatField("Pos Z", data.data[selection].posZ, fieldLayoutOption);
                                data.data[selection].rotX = EditorGUILayout.FloatField("Rot X", data.data[selection].rotX, fieldLayoutOption);
                                data.data[selection].rotY = EditorGUILayout.FloatField("Rot Y", data.data[selection].rotY, fieldLayoutOption);
                                data.data[selection].rotZ = EditorGUILayout.FloatField("Rot Z", data.data[selection].rotZ, fieldLayoutOption);
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
                        DataManagementHelper.CreateEnumStructure("RespawnList", data.names);
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                    if (GUILayout.Button("Load"))
                    {
                        data = CreateInstance<RespawnData>();
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

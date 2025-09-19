using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    public class UnlockDataTool : EditorWindow
    {
        private int selection = 0;
        private static UnlockData data;

        private static UnlockDataTool window;
        private GUILayoutOption fieldLayoutOption = GUILayout.Width(450);

        [MenuItem("Tools/Unlock  Data Tool")]
        public static void Init()
        {
            data = CreateInstance<UnlockData>();
            data.LoadData();

            window = GetWindow<UnlockDataTool>();
            window.Show();
            window.titleContent = new GUIContent("Unlock Data Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<UnlockData>();
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
                                data.names[selection] = EditorGUILayout.TextField("Name", data.names[selection], fieldLayoutOption);
                                data.data[selection].isUnlocked = EditorGUILayout.Toggle("Is Unlocked", data.data[selection].isUnlocked, fieldLayoutOption);
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
                            DataManagementHelper.CreateEnumStructure("UnlockList", data.names);
                            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                        }
                        if (GUILayout.Button("Load"))
                        {
                            data = CreateInstance<UnlockData>();
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
}

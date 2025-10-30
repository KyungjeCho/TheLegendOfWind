using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    public class TutorialTool : EditorWindow
    {
        private int selection = 0;
        private static TutorialData data;

        private static TutorialTool window;
        private GUILayoutOption fieldLayoutOption = GUILayout.Width(450);

        [MenuItem("Tools/Tutorial Tool")]
        public static void Init()
        {
            data = CreateInstance<TutorialData>();
            data.LoadData();

            window = GetWindow<TutorialTool>();
            window.Show();
            window.titleContent = new GUIContent("Tutorial Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<TutorialData>();
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
                                EditorGUILayout.LabelField("Description");
                                data.data[selection].description = EditorGUILayout.TextArea(data.data[selection].description, GUILayout.Height(200));
                                data.data[selection].isCleared = EditorGUILayout.Toggle("Is Cleared", data.data[selection].isCleared, fieldLayoutOption);
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
                            DataManagementHelper.CreateEnumStructure("TutorialList", data.names);
                            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                        }
                        if (GUILayout.Button("Load"))
                        {
                            data = CreateInstance<TutorialData>();
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
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace KJ
{
    public class EffectTool : EditorWindow
    {
        private int selection = 0;
        private static EffectData data;
        private GameObject effectSource;

        private static EffectTool window;
        [MenuItem("Tools/Effect Tool")]
        private static void Init()
        {
            data = CreateInstance<EffectData>();
            data.LoadData();

            window = GetWindow<EffectTool>();
            window.Show();
            window.titleContent = new GUIContent("Effect Tool");
        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<EffectData>();
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
                        effectSource = null;
                        EditorGUILayout.BeginScrollView(Vector2.zero);
                        {
                            if (data.GetDataCount() > 0)
                            {
                                EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(450));
                                data.names[selection] = EditorGUILayout.TextField("Name", data.names[selection], GUILayout.Width(450));
                                if (effectSource == null && data.effectClips[selection].clipName != string.Empty)
                                {
                                    data.effectClips[selection].PreLoad();
                                    effectSource = Resources.Load(data.effectClips[selection].clipPath + data.effectClips[selection].clipName) as GameObject;
                                }
                                effectSource = (GameObject) EditorGUILayout.ObjectField("Effect Clip", effectSource, typeof(GameObject), false, GUILayout.Width(450));

                                if (effectSource != null)
                                {
                                    data.effectClips[selection].clipPath = DataManagementHelper.GetPath(effectSource);
                                    data.effectClips[selection].clipName = effectSource.name;

                                    Debug.Log(data.effectClips[selection].clipPath);
                                    Debug.Log(data.effectClips[selection].clipName);
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
            Object source = effectSource;
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
                    Debug.Log(data.effectClips);
                    data.SaveData();
                    DataManagementHelper.CreateEnumStructure("EffectList", data);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
                if (GUILayout.Button("Load"))
                {
                    data = CreateInstance<EffectData>();
                    data.LoadData();
                    selection = 0;
                    effectSource = null;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}

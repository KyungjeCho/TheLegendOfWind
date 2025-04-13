using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Unity.VisualScripting.Member;
using System.Text;

namespace KJ
{
    
    public class SoundTool : EditorWindow
    {
        private int selection = 0;      // 현재 리스트 중에 선택한 번호
        private static SoundData data;  // 데이터 관리 클래스
        private AudioClip audioSource;

        private static SoundTool window;
        [MenuItem("Tools/Sound Tool")]
        public static void Init()
        {
            data = CreateInstance<SoundData>();
            data.LoadData();

            window = GetWindow<SoundTool>();
            window.Show();
            window.titleContent = new GUIContent("Sound Tool");

        }

        private void OnGUI()
        {
            if (data == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    data = CreateInstance<SoundData>();
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
                        audioSource = null;
                        EditorGUILayout.BeginScrollView(Vector2.zero);
                        {
                            if (data.GetDataCount() > 0)
                            {
                                EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(450));
                                data.names[selection] = EditorGUILayout.TextField("Name", data.names[selection], GUILayout.Width(450));
                                data.soundClips[selection].playType = (SoundPlayType)EditorGUILayout.EnumPopup("Play Type", data.soundClips[selection].playType, GUILayout.Width(450));
                                if (audioSource == null && data.soundClips[selection].clipName != string.Empty)
                                    audioSource = Resources.Load(data.soundClips[selection].clipPath + data.soundClips[selection].clipName) as AudioClip;
                                audioSource = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", audioSource, typeof(AudioClip), false, GUILayout.Width(450));
                                
                                if (audioSource != null)
                                {
                                    data.soundClips[selection].clipPath = DataManagementHelper.GetPath(audioSource); ;
                                    data.soundClips[selection].clipName = audioSource.name;

                                    data.soundClips[selection].isLoop = EditorGUILayout.Toggle("Loop", data.soundClips[selection].isLoop, GUILayout.Width(450));
                                    data.soundClips[selection].minDistance = EditorGUILayout.FloatField("Min Distance", data.soundClips[selection].minDistance, GUILayout.Width(450));
                                    data.soundClips[selection].maxDistance = EditorGUILayout.FloatField("Max Distance", data.soundClips[selection].maxDistance, GUILayout.Width(450));

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
            Object source = audioSource;
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
                    Debug.Log(data.soundClips);
                    data.SaveData();
                    CreateEnumStructure();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
                if (GUILayout.Button("Load"))
                {
                    data = CreateInstance<SoundData>();
                    data.LoadData();
                    selection = 0;
                    audioSource = null;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public void CreateEnumStructure()
        {
            string enumName = "SoundList";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.names.Length; i++)
            {
                builder.AppendLine("    " + data.names[i] + " = " + i.ToString() + ",");
            }
            DataManagementHelper.CreateEnumList(enumName, builder);
        }
    }

}

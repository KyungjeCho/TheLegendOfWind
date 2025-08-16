using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    [CustomEditor(typeof(DialogDB))]
    public class DialogDBEdiotr : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // 기본 인스펙터 그리기
            DialogDB db = (DialogDB)target;

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    //if (GUILayout.Button("Save"))
                    //{
                    //    db.SaveData();
                    //}
                    EditorGUILayout.BeginVertical();
                    {
                        foreach (var dialog in db.Container)
                        {
                            EditorGUILayout.LabelField("ID", dialog.Value.id);
                            EditorGUILayout.LabelField("Speaker", dialog.Value.speaker);
                            EditorGUILayout.LabelField("Dialog", dialog.Value.dialog);
                            EditorGUILayout.LabelField("NextID", dialog.Value.nextId);
                            EditorGUILayout.LabelField("Choices", dialog.Value.choices);
                            EditorGUILayout.LabelField("ChoicesNextIds", dialog.Value.choicesNextId);
                            EditorGUILayout.LabelField("Trigger", dialog.Value.trigger);
                        }
                    }
                    EditorGUILayout.EndVertical();
                    if (GUILayout.Button("Load"))
                    {
                        db.LoadCSV();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();


        }
    }
}

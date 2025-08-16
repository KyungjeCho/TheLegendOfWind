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
                            EditorGUILayout.LabelField("ID", dialog.id);
                            EditorGUILayout.LabelField("Speaker", dialog.speaker);
                            EditorGUILayout.LabelField("Dialog", dialog.dialog);
                            EditorGUILayout.LabelField("NextID", dialog.nextId);
                            EditorGUILayout.LabelField("Choices", dialog.choices);
                            EditorGUILayout.LabelField("ChoicesNextIds", dialog.choicesNextId);
                            EditorGUILayout.LabelField("Trigger", dialog.trigger);
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

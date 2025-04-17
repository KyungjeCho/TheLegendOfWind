using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KJ
{
    [CustomEditor(typeof(PlayerData))]
    public class PlayerDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // 기본 인스펙터 그리기
            PlayerData playerData = (PlayerData)target;

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Save"))
                    {
                        playerData.SaveData();
                    }
                    if (GUILayout.Button("Load"))
                    {
                        playerData.LoadData();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KJ
{
    public class SceneEnumTool : EditorWindow
    {
        private static string[] sceneData;

        private static SceneEnumTool window;
        private GUILayoutOption fieldLayoutOption = GUILayout.Width(450);

        [MenuItem("Tools/Create Scene Enum List Tool")]
        public static void Init()
        {
            LoadSceneData();

            window = GetWindow<SceneEnumTool>();
            window.Show();
            window.titleContent = new GUIContent("Create Scene Enum List Tool");
        }

        private static void LoadSceneData()
        {
            sceneData = new string[0];
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

                string sceneName = Path.GetFileNameWithoutExtension(scenePath);

                sceneData = ArrayHelper.Add<string>(sceneName, sceneData);
            }
        }

        private void OnGUI()
        {
            if (sceneData == null)
            {
                if (GUILayout.Button("Reload"))
                {
                    LoadSceneData();
                }
                return;
            }

            if (GUILayout.Button("Create Enum List"))
            {
                DataManagementHelper.CreateEnumStructure("SceneList", sceneData);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KJ
{
    public class LoadScene : SingletonMonoBehaviour<LoadScene>
    {
        public GameObject UISceneLoading;
        public GameObject[] UIMainPanels;

        private void Start()
        {
            DontDestroyOnLoad(UISceneLoading);
        }
        public void LoadAsync(SceneList sceneList, bool isPlayerMove = false)
        {
            // �� �ε� UI�� ȭ�� ������
            // ������ �Է��� ����
            // ���� �񵿱�� �ε�
            // �ε尡 �Ϸ�Ǹ�
            // ������ �Է��� �ٽ� Ȱ��ȭ
            // �� �ε� UI�� ��Ȱ��ȭ

            UISceneLoading.SetActive(true);
            foreach (GameObject go in UIMainPanels)
            {
                go.SetActive(false);
            }
            InputManager.Instance.ChangeStrategy(new StopInput());
            if (isPlayerMove)
            {
                PlayerPrefs.SetInt("PlayerLastScene", (int)sceneList);
            }
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync((int)sceneList, LoadSceneMode.Single);
            asyncOp.completed += OnSceneLoaded;
        }
        public void LoadAsync(int sceneIdx, bool isPlayerMove = false)
        {
            UISceneLoading.SetActive(true);
            foreach (GameObject go in UIMainPanels)
            {
                go.SetActive(false);
            }
            InputManager.Instance.ChangeStrategy(new StopInput());
            if (isPlayerMove)
            {
                PlayerPrefs.SetInt("PlayerLastScene", sceneIdx);
            }
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneIdx, LoadSceneMode.Single);
            asyncOp.completed += OnSceneLoaded;
        }

        public void LoadLastScene()
        {
            UISceneLoading.SetActive(true);
            foreach (GameObject go in UIMainPanels)
            {
                go.SetActive(false);
            }
            InputManager.Instance.ChangeStrategy(new StopInput());
            int sceneIdx = PlayerPrefs.GetInt("PlayerLastScene", (int)SceneList.InGameScene);
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneIdx, LoadSceneMode.Single);
            asyncOp.completed += OnSceneLoaded;
        }
        public void OnSceneLoaded(AsyncOperation op)
        {
            InputManager.Instance.ChangeNormalStrategy();
            UISceneLoading.SetActive(false);
            //op.completed -= OnSceneLoaded;
        }
    }
}
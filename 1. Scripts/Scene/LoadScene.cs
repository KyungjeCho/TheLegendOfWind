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
            // 씬 로드 UI로 화면 가리기
            // 유저의 입력을 통제
            // 씬을 비동기로 로드
            // 로드가 완료되면
            // 유저의 입력을 다시 활성화
            // 씬 로드 UI를 비활성화

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
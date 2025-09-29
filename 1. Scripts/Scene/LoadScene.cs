using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KJ
{
    public class LoadScene : SingletonMonoBehaviour<LoadScene>
    {
        public GameObject UISceneLoading;

        private void Start()
        {
            DontDestroyOnLoad(UISceneLoading);
        }
        public void LoadAsync(SceneList sceneList)
        {
            // 씬 로드 UI로 화면 가리기
            // 유저의 입력을 통제
            // 씬을 비동기로 로드
            // 로드가 완료되면
            // 유저의 입력을 다시 활성화
            // 씬 로드 UI를 비활성화

            UISceneLoading.SetActive(true);
            InputManager.Instance.ChangeStrategy(new StopInput());

            AsyncOperation asyncOp = SceneManager.LoadSceneAsync((int)sceneList, LoadSceneMode.Single);
            asyncOp.completed += OnSceneLoaded;
        }
        public void OnSceneLoaded(AsyncOperation op)
        {
            InputManager.Instance.ChangeNormalStrategy();
            UISceneLoading.SetActive(false);
        }
    }
}
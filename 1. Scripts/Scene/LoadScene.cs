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
            // �� �ε� UI�� ȭ�� ������
            // ������ �Է��� ����
            // ���� �񵿱�� �ε�
            // �ε尡 �Ϸ�Ǹ�
            // ������ �Է��� �ٽ� Ȱ��ȭ
            // �� �ε� UI�� ��Ȱ��ȭ

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
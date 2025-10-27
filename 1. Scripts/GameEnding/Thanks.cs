using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KJ
{
    public class Thanks : MonoBehaviour
    {
        public Text thankText;
        public float duration = 5f;

        public SceneList titleScene;

        void Start()
        {
            StartCoroutine(FadeInRoutine());
        }

        private IEnumerator FadeInRoutine()
        {
            float elaspedTime = 0f;
            Color orig = thankText.color;

            while (elaspedTime < duration)
            {
                elaspedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elaspedTime / duration);
                thankText.color = new Color(orig.r, orig.g, orig.b, alpha);

                yield return null;
            }
            StartCoroutine(FadeOutRoutiine());
        }

        private IEnumerator FadeOutRoutiine()
        {
            float elaspedTime = 0f;
            Color orig = thankText.color;

            while (elaspedTime < duration)
            {
                elaspedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elaspedTime / duration);
                thankText.color = new Color(orig.r, orig.g, orig.b, alpha);

                yield return null;
            }
            SceneManager.LoadScene("TitleScene");
        }
    }
}

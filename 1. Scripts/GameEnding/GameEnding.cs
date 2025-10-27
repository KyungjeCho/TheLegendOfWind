using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace KJ
{
    public class GameEnding : MonoBehaviour
    {
        public PlayableDirector gameEndingTimeline;
        public BloomController dragonItemBloom;
        public GameObject shinyItemEffect;
        public float duration = 3f;
        public float maxScale = 5f;

        public float delayTime = 4f;
        public void Play()
        {
            gameEndingTimeline.Play();
            dragonItemBloom.StartBloomFlash();
            StartCoroutine(EffectScaleRoutine());
            StartCoroutine(DelaySceneRoutine());
        }

        public IEnumerator EffectScaleRoutine()
        {
            float elaspedTime = 0f;

            while (elaspedTime < duration)
            {
                elaspedTime += Time.deltaTime;
                float scale = Mathf.Lerp(1f, maxScale, elaspedTime / duration);
                shinyItemEffect.transform.localScale = Vector3.one * scale;

                yield return null;
            }
        }

        public IEnumerator DelaySceneRoutine()
        {
            yield return new WaitForSeconds(delayTime);
            SceneManager.LoadScene("EndingScene");
        }
    }

}

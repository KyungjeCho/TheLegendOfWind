using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace KJ
{
    public class SpawnDragonHeartItem : MonoBehaviour
    {
        public GameObject dragonHeartItem;
        public float height = 10f;
        public PlayableDirector fallingItemTimeline;
        public float duration = 4f;
        public SoundList clearSound;
        
        private Vector3 origPos;

        public void Spawn()
        {
            dragonHeartItem.SetActive(true);
            origPos = dragonHeartItem.transform.position;
            dragonHeartItem.transform.position += Vector3.up * height;
            fallingItemTimeline.Play();
            SoundManager.Instance.PlayOneShotEffect(clearSound, origPos, 1f);
            StartCoroutine(FallingRoutine());
        }

        private IEnumerator FallingRoutine()
        {
            float elapsedTime = 0f;
            Vector3 upPos = dragonHeartItem.transform.position;

            while(elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                Vector3 pos = Vector3.Lerp(upPos, origPos, elapsedTime / duration);
                dragonHeartItem.transform.position = pos;

                yield return null;
            }
        }
    }
}
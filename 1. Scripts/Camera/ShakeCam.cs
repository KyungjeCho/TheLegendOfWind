using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KJ
{
    public class ShakeCam : MonoBehaviour
    {
        public float duration = 2f;
        public float maginitude = 1.0f;

        public void StartShake()
        {
            StartCoroutine(ShakeRoutine(duration, maginitude));
        }
        private Camera GetActiveCam()
        {
            
            return Camera.allCameras
                .Where(cam => cam.enabled && cam.gameObject.activeInHierarchy)
                .OrderByDescending(cam => cam.depth)
                .FirstOrDefault();
        }
        
        private IEnumerator ShakeRoutine(float duration, float magnitude)
        {
            // 한프레임 기다리기
            yield return null;

            Camera activeCam = GetActiveCam();
            Vector3 originalPos = activeCam.transform.localPosition;
            float elapsedTime = 0f;
            float seed = Random.value * 100f;

            while (elapsedTime < duration)
            {
                float x = (Mathf.PerlinNoise(Time.time * 10f, seed) - 0.5f) * 2f * magnitude;
                float y = (Mathf.PerlinNoise(seed, Time.time * 10f) - 0.5f) * 2f * magnitude;

                activeCam.transform.localPosition = originalPos + new Vector3(x, y, 0);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            activeCam.transform.localPosition = originalPos;
        }
    }

}

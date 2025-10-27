using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KJ
{
    public class BloomController : MonoBehaviour
    {
        public Volume volume;
        private Bloom bloom;

        public float maxIntensity = 25f;
        public float duration = 5f;
        // Start is called before the first frame update
        void Start()
        {
            volume.profile.TryGet(out bloom);
        }

        public void StartBloomFlash()
        {
            StartCoroutine(BloomRoutine());
        }

        private IEnumerator BloomRoutine()
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                bloom.intensity.value = Mathf.Lerp(0f, maxIntensity, elapsedTime / duration);

                yield return null;
            }
        }

#if UNITY_EDITOR
        void OnGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 18;

            GUILayout.BeginArea(new Rect(10, 80, 200, 200));

            if (GUILayout.Button("Bloom Start", style))
            {
                StartBloomFlash();
            }
            GUILayout.EndArea();
        }
#endif
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace KJ
{
    public class WarningDecalProjectorController : MonoBehaviour
    {
        public float radius = 1f;
        public float innerDecalOpacity = 1f;
        public float warningDuration = 5f;
        public float fillAmount = 0f;

        private DecalProjector outlineDecal;
        private DecalProjector innerDecal;
        
        private Material innerDecalMat;

        public event Action onDecalProjectorStart;
        public event Action onDecalProjectorEnd;

        public UnityEvent onWarningEnd;
        void Start()
        {
            Init();

            onDecalProjectorStart?.Invoke();
        }

        public void Init()
        {
            outlineDecal = GetComponentsInChildren<DecalProjector>()[0];
            innerDecal = GetComponentsInChildren<DecalProjector>()[1];

            outlineDecal.size = new Vector3(radius, radius, 10f);
            innerDecal.size = new Vector3(radius, radius, 10f);

            innerDecal.fadeFactor = innerDecalOpacity;

            innerDecalMat = innerDecal.material;

            innerDecalMat.SetFloat("_Fill_Amount", fillAmount);

            onDecalProjectorStart += OnDecalProjectorStart;
            onDecalProjectorEnd += OnDecalProjectorEnd;
        }
        private void OnValidate()
        {
            Init();
        }

        public void OnDecalProjectorStart()
        {
            StopAllCoroutines();
            StartCoroutine(FillDecal());
        }
        public void OnDecalProjectorEnd()
        {
            onWarningEnd?.Invoke();
            outlineDecal.fadeFactor = 0f;
            innerDecal.fadeFactor = 0f;
        }

        public IEnumerator FillDecal()
        {
            float elapsedTime = 0f;
            while(elapsedTime < warningDuration)
            {
                elapsedTime += 0.01f;
                fillAmount = elapsedTime / warningDuration;
                innerDecalMat.SetFloat("_Fill_Amount", fillAmount);
                yield return new  WaitForSeconds(0.01f);
            }
            onDecalProjectorEnd?.Invoke();
        }
    }
}
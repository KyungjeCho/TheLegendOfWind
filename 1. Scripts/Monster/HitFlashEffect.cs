using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class HitFlashEffect : MonoBehaviour
    {
        private SkinnedMeshRenderer meshRenderer;
        public Color flashColor = Color.white;

        [Range(0f, 1f)]
        public float flashDuration = 1.0f;

        private Material material;
        private Color originalEmission;

        private void Awake()
        {
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            material = meshRenderer.material;
            originalEmission = material.GetColor("_EmissionColor");
        }

        public void Flash()
        {
            StartCoroutine(FlashRoutine());
        }
        private IEnumerator FlashRoutine()
        {
            // �Ͼ������ ��½
            material.SetColor("_EmissionColor", flashColor * 2f);
            yield return new WaitForSeconds(flashDuration);

            // ���� ������ ����
            material.SetColor("_EmissionColor", originalEmission);
        }
    }

}

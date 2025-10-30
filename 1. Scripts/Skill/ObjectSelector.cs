using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum RimType
    {
        SELECT = 0,
        SELECTED = 1
    }
    public class ObjectSelector : MonoBehaviour
    {
        public Color selectColor = Color.white;
        public Color selectedColor = Color.white;

        public float maxIntensity = 10f;

        public SkinnedMeshRenderer meshRenderer;
        private MaterialPropertyBlock propBlock;

        private readonly int intensityId = Shader.PropertyToID("_Intensity");
        private readonly int baseColorId = Shader.PropertyToID("_BaseColor");

        private void Start()
        {
            propBlock = new MaterialPropertyBlock();
        }

        public void SetRimLight(bool isEnabled, RimType type = RimType.SELECT)
        {
            if (meshRenderer == null)
            {
                return;
            }

            meshRenderer.GetPropertyBlock(propBlock);
            switch(type)
            {
                case RimType.SELECT:
                    propBlock.SetColor(baseColorId, selectColor);
                    break;
                case RimType.SELECTED:
                    propBlock.SetColor(baseColorId, selectedColor);
                    break;
            }
            propBlock.SetFloat(intensityId, isEnabled ? maxIntensity : 0f);

            meshRenderer.SetPropertyBlock(propBlock);
        }

        public void TriggerRimLight(float duration)
        {
            StartCoroutine(IRimLightingRoutine(duration));
        }
        public IEnumerator IRimLightingRoutine(float duration, RimType type = RimType.SELECTED)
        {
            SetRimLight(true, type);
            yield return new WaitForSeconds(duration);
            SetRimLight(false);
        }
    }

}

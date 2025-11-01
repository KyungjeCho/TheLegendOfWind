using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class TestObjectPool : MonoBehaviour
    {
        public EffectList testEffect;
        public Transform playerTr;

        public EffectClip testEffectClip;

        private void Start()
        {
            testEffectClip = DataManager.EffectData.GetCopy((int)testEffect);
            testEffectClip.PreLoad();

            PoolManager.GetOrCreateInstance().CreatePool(testEffectClip.effectPrefab);
        }
        public void PlayEffect()
        {
            GameObject go = PoolManager.GetOrCreateInstance().Get(testEffectClip.effectPrefab);
            go.transform.position = playerTr.position;
            go.transform.rotation = playerTr.rotation;
        }
    }

}

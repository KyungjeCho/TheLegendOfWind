using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class EffectManager : SingletonMonoBehaviour<EffectManager>
    {
        public GameObject PlayEffect(EffectClip clip, Vector3 position, bool isObjectPool = true)
        {
            clip.PreLoad();
            if (isObjectPool)
                return PoolManager.GetOrCreateInstance().Get(clip.effectPrefab, position, Quaternion.identity);
            else
                return Instantiate(clip.effectPrefab, position, Quaternion.identity) as GameObject;
        }
        public GameObject PlayEffect(EffectClip clip, Vector3 position, Transform parent, bool isObjectPool = true)
        {
            clip.PreLoad();
            if (isObjectPool)
                return PoolManager.GetOrCreateInstance().Get(clip.effectPrefab, position, Quaternion.identity, parent);
            else
                return Instantiate(clip.effectPrefab, position, Quaternion.identity, parent) as GameObject;
        }

        public GameObject PlayEffect(EffectList effect, Vector3 position)
        {
            return PlayEffect(DataManager.EffectData.GetCopy((int)effect), position) as GameObject;
        }
        public GameObject PlayEffect(EffectList effect, Vector3 position, Transform parent)
        {
            return PlayEffect(DataManager.EffectData.GetCopy((int)effect), position, parent) as GameObject;
        }
    }

}

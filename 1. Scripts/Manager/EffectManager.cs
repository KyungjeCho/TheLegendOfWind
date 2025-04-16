using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class EffectManager : SingletonMonoBehaviour<EffectManager>
    {
        public GameObject PlayEffect(EffectClip clip, Vector3 position)
        {
            clip.PreLoad();

            return Instantiate(clip.effectPrefab, position, Quaternion.identity) as GameObject;


        }

        public GameObject PlayEffect(EffectList effect, Vector3 position)
        {
            return PlayEffect(DataManager.EffectData.GetCopy((int)effect), position) as GameObject;
        }
    }

}

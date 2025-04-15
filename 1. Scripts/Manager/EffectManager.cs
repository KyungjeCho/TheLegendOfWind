using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class EffectManager : SingletonMonoBehaviour<EffectManager>
    {
        public void PlayEffect(EffectClip clip, Vector3 position)
        {
            clip.PreLoad();

            Instantiate(clip.effectPrefab, position, Quaternion.identity);


        }

        public void PlayEffect(EffectList effect, Vector3 position)
        {
            PlayEffect(DataManager.EffectData.GetCopy((int)effect), position);
        }
    }

}

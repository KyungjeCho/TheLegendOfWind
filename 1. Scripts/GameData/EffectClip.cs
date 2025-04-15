using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class EffectClip
    {
        public GameObject effectPrefab = null;
        public string clipName = string.Empty;
        public string clipPath = string.Empty;

        public EffectClip() { }

        public void PreLoad()
        {
            if (effectPrefab == null)
            {
                effectPrefab = Resources.Load<GameObject>(clipPath + clipName);
            }
        }
    }

}


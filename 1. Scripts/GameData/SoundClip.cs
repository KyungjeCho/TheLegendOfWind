using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum SoundPlayType { None, BGM, SFX }
    [Serializable]
    public class SoundClip
    {
        public SoundPlayType playType = SoundPlayType.None;
        public string clipPath = string.Empty;
        public string clipName = string.Empty;
        public bool isLoop = false;
        private AudioClip audioClip = null;
        public float minDistance = 10000.0f;
        public float maxDistance = 50000.0f;
        public AudioClip GetClip => audioClip;

        public void PreLoad()
        {
            if (audioClip == null)
            {
                audioClip = Resources.Load(clipPath + clipName) as AudioClip;
            }
        }
    }
}

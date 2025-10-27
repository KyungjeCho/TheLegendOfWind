using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum SoundPlayType { None, BGM, SFX, UI }
    [Serializable]
    public class SoundClip
    {
        public SoundPlayType playType = SoundPlayType.None;
        public string clipPath = string.Empty;
        public string clipName = string.Empty;
        public float maxVolume = 1.0f;
        public bool isLoop = false;
        private AudioClip audioClip = null;
        public float[] checkTime = new float[0];
        public float[] setTime = new float[0];
        public int realId = 0;

        public int currentLoop = 0;
        public float pitch = 1.0f;
        public float dopplerLevel = 1.0f;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        public float minDistance = 10000.0f;
        public float maxDistance = 50000.0f;
        public float spartialBlend = 1.0f;

        public float fadeTime1 = 0.0f;
        public float fadeTime2 = 0.0f;
        public Interpolate.Function interpolate_Func;
        public bool isFadeIn = false;
        public bool isFadeOut = false;

        public void PreLoad()
        {
            if (audioClip == null)
            {
                audioClip = Resources.Load(clipPath + clipName) as AudioClip;
            }
        }

        public void AddLoop()
        {
            checkTime = ArrayHelper.Add(0.0f, checkTime);
            setTime = ArrayHelper.Add(0.0f, setTime);
        }

        public AudioClip GetClip()
        {
            if (audioClip == null)
            {
                PreLoad();
            }
            if (audioClip == null && clipName != string.Empty)
            {
                Debug.LogWarning($"Can not load audio clip Resource {clipName}");
                return null;
            }
            return audioClip;
        }

        public void ReleaseClip()
        {
            if (audioClip != null)
            {
                audioClip = null;
            }
        }

        public bool HasLoop()
        {
            return checkTime.Length > 0;
        }

        public void NextLoop()
        {
            currentLoop++;
            if (currentLoop >= checkTime.Length)
            {
                currentLoop = 0;
            }
        }

        public void CheckLoop(AudioSource source)
        {
            if (HasLoop() && source.time >= checkTime[currentLoop])
            {
                source.time = setTime[currentLoop];
                NextLoop();
            }
        }

        public void FadeIn(float time, Interpolate.EaseType easeType)
        {
            isFadeOut = false;
            fadeTime1 = 0.0f;
            fadeTime2 = time;
            interpolate_Func = Interpolate.Ease(easeType);
            isFadeIn = true;
        }

        public void FadeOut(float time, Interpolate.EaseType easeType)
        {
            isFadeIn = false;
            fadeTime1 = 0.0f;
            fadeTime2 = time;
            interpolate_Func = Interpolate.Ease(easeType);
            isFadeOut = true;
        }

        public void DoFade(float time, AudioSource source)
        {
            if (isFadeIn == true)
            {
                fadeTime1 += time;
                source.volume = Interpolate.Ease(interpolate_Func, 0, maxVolume, fadeTime1, fadeTime2);
                if (fadeTime1 >= fadeTime2)
                {
                    isFadeIn = false;
                }
            }
            else if (isFadeOut == true)
            {
                fadeTime1 += time;
                source.volume = Interpolate.Ease(interpolate_Func, maxVolume, 0 - maxVolume, fadeTime1, fadeTime2);
                if (fadeTime1 >= fadeTime2)
                {
                    isFadeOut = false;
                    source.Stop();
                }
            }
        }
    }
}

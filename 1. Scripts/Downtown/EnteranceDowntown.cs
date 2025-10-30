using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class EnteranceDowntown : MonoBehaviour
    {
        public SoundList enterSound;
        public SoundList exitSound;

        public SoundClip enterSoundClip;
        public SoundClip exitSoundClip;

        private bool isEnter = false;

        private void Start()
        {
            enterSoundClip = DataManager.SoundData.GetCopy((int)enterSound);
            exitSoundClip = DataManager.SoundData.GetCopy((int)exitSound);

            enterSoundClip.PreLoad();
            exitSoundClip.PreLoad();

        }
        private void OnTriggerEnter(Collider other)
        {
            if (!isEnter && other.CompareTag(TagAndLayer.Player))
            {
                SoundManager.GetOrCreateInstance().FadeTo(enterSoundClip, 1f, Interpolate.EaseType.Linear);
                isEnter = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isEnter && other.CompareTag(TagAndLayer.Player))
            {
                SoundManager.Instance.FadeTo(exitSoundClip, 1f, Interpolate.EaseType.Linear);
                isEnter = false;
            }
        }
    }
}


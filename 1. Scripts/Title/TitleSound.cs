using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class TitleSound : MonoBehaviour
    {
        public SoundList titleSound;

        // Start is called before the first frame update
        void Start()
        {
            PlayTitleSound();
        }

        public void PlayTitleSound()
        {
            SoundManager.GetOrCreateInstance().FadeIn((int)titleSound, 1f, Interpolate.EaseType.Linear);
        }
    }
}

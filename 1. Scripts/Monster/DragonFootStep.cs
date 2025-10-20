using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DragonFootStep : MonoBehaviour
    {
        public SoundList[] stepSounds;

        public void PlayDragonFootStep()
        {
            if (stepSounds.Length > 0)
            {
                int idx = Random.Range(0, stepSounds.Length);

                SoundManager.Instance.PlayOneShotEffect(stepSounds[idx], transform.position, 1f);
            }
        }
    }

}

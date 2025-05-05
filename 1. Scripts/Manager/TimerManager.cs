using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class TimerManager : SingletonMonoBehaviour<TimerManager>
    {
        public void StartTimer(float duration, Action callback)
        {
            StartCoroutine(TimerCoroutine(duration, callback));
        }
        private IEnumerator TimerCoroutine(float duration, Action callback)
        {
            yield return new WaitForSeconds(duration);
            callback?.Invoke();
        }
    }

}

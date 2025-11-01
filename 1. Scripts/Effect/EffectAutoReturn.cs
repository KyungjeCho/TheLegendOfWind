using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoReturn : MonoBehaviour
{
    public float duration = 5f;

    private void OnEnable()
    {
        StartCoroutine(DelayRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
        PoolManager.GetOrCreateInstance().Return(this.gameObject);
    }
}

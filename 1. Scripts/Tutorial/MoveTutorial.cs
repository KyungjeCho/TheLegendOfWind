using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : MonoBehaviour
{
    private TutorialController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TutorialController>();
        StartCoroutine(DelayRoutine(3f));
    }

    private IEnumerator DelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        controller.OpenPanel();
    }
}

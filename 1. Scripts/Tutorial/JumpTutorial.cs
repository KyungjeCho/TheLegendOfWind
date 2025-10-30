using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class JumpTutorial : MonoBehaviour
    {
        private TutorialController controller;

        private void Start()
        {
            controller = GetComponent<TutorialController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagAndLayer.Player))
            {
                controller.OpenPanel();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class AttackTutorial : MonoBehaviour
    {
        private TutorialController controller;

        // Start is called before the first frame update
        void Start()
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

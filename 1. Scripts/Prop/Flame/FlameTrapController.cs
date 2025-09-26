using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class FlameTrapController : MonoBehaviour, IStopable
    {
        private FlameController flameController;

        public void StopObject()
        {
            if (flameController != null)
            {
                flameController.StopObject();
            }

        }

        void Start()
        {
            flameController = GetComponentInChildren<FlameController>();
        }
    }
}
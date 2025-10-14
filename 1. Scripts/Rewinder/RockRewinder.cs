using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class RockRewinder : MonoBehaviour, IRewind
    {
        private HomingRockController controller;

        public void Record()
        {
            throw new System.NotImplementedException();
        }

        public void Rewind()
        {
            throw new System.NotImplementedException();
        }

        public void StartRewind()
        {
            controller.targetTr = controller.ownerTr;
        }

        public void StopRewind()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<HomingRockController>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


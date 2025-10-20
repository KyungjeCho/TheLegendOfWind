using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class LookAtCam : MonoBehaviour
    {
        public Transform targetTr;


        // Update is called once per frame
        void Update()
        {
            if (targetTr == null) return;

            transform.LookAt(targetTr);
        }
    }

}


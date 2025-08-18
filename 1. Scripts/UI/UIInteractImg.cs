using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class UIInteractImg : MonoBehaviour
    {
        private Transform mainCam;

        private void Start()
        {
            mainCam = Camera.main.transform;
        }
        private void LateUpdate()
        {
            Vector3 dir = transform.position - mainCam.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

}

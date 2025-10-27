using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ItemCam : MonoBehaviour
    {
        public Transform playerTr;
        public Transform targetTr;

        [Range(0f, 1f)]
        public float pivot = 0.5f;
        public float height = 1f;
        public Vector3 camOffset =  new Vector3(-0.5f, 0.5f, -2.0f);

        private Transform cameraTr;

        // Start is called before the first frame update
        void Start()
        {
            cameraTr = transform;

        }
        private void Update()
        {
            cameraTr.position =
                targetTr.position + Quaternion.identity * camOffset;
            cameraTr.rotation = Quaternion.identity;

            Vector3 direction = Vector3.Lerp(playerTr.position, targetTr.position, pivot); 
            cameraTr.LookAt(direction + Vector3.up * height);

        }
    }
}
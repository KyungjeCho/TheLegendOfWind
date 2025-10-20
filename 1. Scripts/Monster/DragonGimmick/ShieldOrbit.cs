using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ShieldOrbit : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = Vector3.zero;
        public float radius = 2f;
        public float speed = 1f;
        public float startAngle = 0f;

        private float angle;

        // Start is called before the first frame update
        void Start()
        {
            angle = startAngle;
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                return;
            }

            angle += speed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius;
            transform.position = target.position + offset + pos;

            transform.LookAt(target.position + offset);
        }
    }

}

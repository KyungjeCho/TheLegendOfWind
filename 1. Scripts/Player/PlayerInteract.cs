using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField]
        private float distance = 2f;
        [SerializeField]
        private float angle = 60f;
        [SerializeField]
        private LayerMask targetMask;
        [SerializeField]
        private LayerMask obstacleMask;
        private Camera mainCam;
        [SerializeField]
        private Transform target;

        public Transform Target => target;

        
        // Start is called before the first frame update
        void Start()
        {
            mainCam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            ScanTarget();

            if (InputManager.Instance.InteractButton.IsButtonPressed)
            {
                ScanTarget();

                if (target != null)
                {
                    BaseNonPlayerController interactable = target.GetComponent<BaseNonPlayerController>();
                    interactable?.Interact();
                }
            }
        }

        private void ScanTarget()
        {
            target = null;

            Collider[] colliders = Physics.OverlapSphere(transform.position, distance, targetMask);

            float minDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                Transform targetTr = collider.transform;
                Vector3 dirToTarget = (targetTr.position - transform.position ).normalized;

                Vector3 camDir = mainCam.transform.forward;
                camDir.y = 0f;

                if (Vector3.Angle(camDir, dirToTarget) < angle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, targetTr.position);

                    if (distToTarget < minDistance)
                    {
                        minDistance = distToTarget;
                        target = targetTr;
                    }
                }

            }
        }

        private void OnDrawGizmos()
        {
            if (mainCam == null)
                return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distance);

            Vector3 camDir = mainCam.transform.forward;
            camDir.y = 0f;
            Vector3 leftBoundary = Quaternion.Euler(0, - angle / 2, 0) * camDir;
            Vector3 rightBoundary = Quaternion.Euler(0, angle / 2, 0) * camDir;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * distance);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * distance);

            if (target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius = 5f;
        public float viewAngle = 90f;
        public float delay = 0.2f;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        private List<Transform> visibleTargets = new List<Transform>();
        private Transform nearestTarget;

        private float distanceToTarget = 0.0f;

        public List<Transform> VisibleTargets => visibleTargets;
        public Transform NearestTarget => nearestTarget;


        private void Start()
        {
            StartCoroutine(FindTargetsWithDelay(delay));
        }

        private IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void FindVisibleTargets()
        {
            distanceToTarget = 0.0f;
            nearestTarget = null;
            visibleTargets.Clear();

            Collider[] targetsInViewRadius = Physics.OverlapSphere(
                transform.position, viewRadius, targetMask);
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;

                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.forward, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget,  dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);

                        if (nearestTarget == null || (distanceToTarget > dstToTarget))
                        {
                            nearestTarget = target;
                        }

                        distanceToTarget = dstToTarget;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.forward + transform.right);
            Gizmos.DrawLine(transform.position, transform.forward - transform.right);
        }
    }

}

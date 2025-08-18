using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SelectObjectBehaviour : BaseBehaviour
    {
        private bool isSelecting = false;

        private Camera myCam;
        private float maxDistance = 100f;
        [SerializeField]
        private LayerMask targetLayerMask;

        private Transform myTransform;
        private Transform targetTransform;

        public Transform TargetTransform => targetTransform;

        public event Action<bool> OnSelect;
        public event Action<Transform> OnTargetObjectSelected;

        public void SetIsSelecting(bool isSelecting) { this.isSelecting = isSelecting; OnSelect?.Invoke(isSelecting); }
        public bool GetIsSelecting() => this.isSelecting;
        
        private void Start()
        {
            myTransform = transform;
            myCam = Camera.main;

            behaviourController.SubscribeBehaviour(this);

        }
        private void Update()
        {
            CheckCollision();
            if (isSelecting && InputManager.Instance.AttackButton.IsPressedDown)
            {
                //Event Publish
                OnTargetObjectSelected?.Invoke(targetTransform);
                SetIsSelecting(false);
            }
            //SelectManament();
        }
        private void CheckCollision()
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = myCam.ScreenPointToRay(screenCenter);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, targetLayerMask))
            {
                if (hit.collider != null)
                {
                    targetTransform = hit.collider.transform;
                }
                else
                {
                    targetTransform = null;
                }
            }
        }
        private void SelectManament()
        {
            if (isSelecting)
            {
                behaviourController.OverrideWithBehaviour(this);
            }
            else
            {
                behaviourController.RevokeOverridingBehaviour(this);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SelectObjectBehaviour : BaseBehaviour
    {
        public Texture2D crossHair;

        private bool isSelecting = false;

        private Camera myCam;
        private float maxDistance = 100f;
        [SerializeField]
        private LayerMask targetLayerMask;

        private Transform myTransform;
        [SerializeField]
        private Transform targetTransform;
        private AttackBehaviour attackBehaviour;

        public Transform TargetTransform => targetTransform;

        private List<ObjectSelector> selectedGameObjects = new List<ObjectSelector>();

        public event Action<bool> OnSelect;
        public event Action<Transform> OnTargetObjectSelected;

        public void SetIsSelecting(bool isSelecting) { this.isSelecting = isSelecting; OnSelect?.Invoke(isSelecting); }
        public bool GetIsSelecting() => this.isSelecting;
        
        private void Start()
        {
            myTransform = transform;
            myCam = Camera.main;

            behaviourController.SubscribeBehaviour(this);
            attackBehaviour = GetComponent<AttackBehaviour>();
        }
        private void Update()
        {
            CheckCollision();
            if (isSelecting && InputManager.Instance.AttackButton.IsPressedDown)
            {
                //Event Publish
                foreach(ObjectSelector os in selectedGameObjects)
                {
                    os.SetRimLight(false);
                }

                OnTargetObjectSelected?.Invoke(targetTransform);
                
                StartCoroutine(IDelaySelecting());
                SetIsSelecting(false);
            }
            else if (isSelecting)
            {
                behaviourController.LockTempBehaviour(behaviourCode);
                FindCanSelectObject();
            }
        }
        public IEnumerator IDelaySelecting()
        {
            yield return new WaitForSeconds(0.05f);
            behaviourController.UnLockTempBehaviour(behaviourCode);
            yield return null;
        }
        private void OnGUI()
        {
            if (crossHair != null)
            {
                if (isSelecting)
                {
                    GUI.DrawTexture(new Rect(Screen.width * 0.5f - (crossHair.width * 0.5f),
                        Screen.height * 0.5f - (crossHair.height * 0.5f),
                        crossHair.width, crossHair.height), crossHair);
                }
            }
        }
        private void FindCanSelectObject()
        {
            float distance = 20f;

            Collider[] colliders = Physics.OverlapSphere(myTransform.position, distance, targetLayerMask);

            selectedGameObjects.Clear();

            foreach(Collider collider in colliders)
            {
                ObjectSelector os = collider.gameObject.GetComponent<ObjectSelector>();
                if (os != null)
                {
                    selectedGameObjects.Add(os);
                    os.SetRimLight(true, RimType.SELECT);
                }
            }

        }

        private void CheckCollision()
        {
            if (!isSelecting)
            {
                return;
            }
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = myCam.ScreenPointToRay(screenCenter);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, targetLayerMask))
            {
                if (hit.collider != null)
                {
                    targetTransform = hit.collider.transform;

                    ObjectSelector os = targetTransform.GetComponent<ObjectSelector>();
                    if (os != null)
                    {
                        os.SetRimLight(true, RimType.SELECTED);
                    }
                }
                else
                { 
                    if (targetTransform != null)
                    {
                        ObjectSelector os = targetTransform.GetComponent<ObjectSelector>();
                        if (os != null)
                        {
                            os.SetRimLight(true, RimType.SELECT);
                        }
                    }
                    targetTransform = null;
                }
            }
            else
            {
                if (targetTransform != null)
                {
                    ObjectSelector os = targetTransform.GetComponent<ObjectSelector>();
                    if (os != null)
                    {
                        os.SetRimLight(true, RimType.SELECT);
                    }
                }
                targetTransform = null;
            }
        }
    }
}
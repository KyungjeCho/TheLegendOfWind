using KJ.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DialogNonPlayerController : InteractComponent
    {
        [SerializeField]
        private string dialogId;

        // Start is called before the first frame update
        protected override void Start()
        {
            interactStrategy = new DialogInteract(dialogId);
        }

        public override void Interact()
        {
            base.Interact();

            CameraController camController = Camera.main.gameObject.GetComponent<CameraController>();
            camController.MoveToNPCFrom3P(transform);
        }
    }
}
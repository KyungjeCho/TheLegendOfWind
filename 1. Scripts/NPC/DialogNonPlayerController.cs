using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DialogNonPlayerController : BaseNonPlayerController
    {
        [SerializeField]
        private string dialogId;

        // Start is called before the first frame update
        protected override void Start()
        {
            interactStrategy = new DialogInteract(dialogId);
        }
    }
}
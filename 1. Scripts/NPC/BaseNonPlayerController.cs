using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BaseNonPlayerController : MonoBehaviour
    {
        public IInteractable interactStrategy;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            interactStrategy = null;
        }
        public virtual void Interact()
        {
            interactStrategy?.Interact();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class BoxController : InteractComponent
    {
        public SoundList openSound;

        private BaseTrigger trigger;
        private Animator myAnimator;
        private int openInt;
        private bool isOpened;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            trigger = GetComponent<BaseTrigger>();
            myAnimator = GetComponent<Animator>();

            openInt = Animator.StringToHash(AnimatorKey.Open);

            isOpened = false;

            interactStrategy = new TriggerInteract(trigger);

            
        }
        public override void Interact()
        {
            if (isOpened) { return; }
            myAnimator.SetTrigger(openInt);
            SoundManager.Instance.PlayOneShotEffect(openSound, transform.position, 1.0f);
            isOpened = true;
        }
        public void OpenBox()
        {
            trigger.OnTrigger();
        }
    }
}
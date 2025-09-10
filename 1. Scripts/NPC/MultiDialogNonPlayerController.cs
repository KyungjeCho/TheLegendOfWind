using KJ.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class MultiDialogNonPlayerController : InteractComponent
    {
        [SerializeField]
        private NPCState defaultState;

        [HideInInspector]
        public SpriteRenderer spriteRenderer;

        public NPCStateMachine stateMachine;
        public Sprite questionSprite1;
        public Sprite questionSprite2;

        protected override void Start()
        {
            stateMachine = new NPCStateMachine(defaultState, transform);
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
        public override void Interact()
        {
            stateMachine.currentState.Act(stateMachine);

            CameraController camController = Camera.main.gameObject.GetComponent<CameraController>();
            camController.MoveToNPCFrom3P(transform);
        }
        public void HideQuestionMark()
        {
            spriteRenderer.enabled = false;
        }
        public void ShowQuestionMark1()
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = questionSprite1;
        }
        public void ShowQuestionMark2()
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = questionSprite2;
        }
    }
}


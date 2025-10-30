using KJ.CameraControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class MultiDialogNonPlayerController : InteractComponent
    {
        [SerializeField]
        private NPCStateDB db;
        [SerializeField]
        private NPCState defaultState;

        private string jsonFilePath = "Assets/9. Resources/Resources/Data";
        [HideInInspector]
        public SpriteRenderer spriteRenderer;

        public NPCStateMachine stateMachine;
        public Sprite questionSprite1;
        public Sprite questionSprite2;


        protected override void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            HideQuestionMark();
            
            stateMachine = new NPCStateMachine(defaultState, transform);
            LoadState();
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

        public void SaveState()
        {
            string content = stateMachine.currentState.name;

            string path = Path.Combine(jsonFilePath, gameObject.name + ".txt");

            File.WriteAllText(path, content);
        }

        public void LoadState()
        {
            string path = Path.Combine(jsonFilePath, gameObject.name + ".txt");
            try
            {
                string content = File.ReadAllText(path);
                defaultState = db.GetNPCState(content);
                stateMachine.ChangeState(defaultState);
            } 
            catch (Exception e1)
            {
                Debug.Log("Load Dialog State : " + e1.Message);
                SaveState();
            }
        }
    }
}


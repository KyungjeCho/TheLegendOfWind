using KJ.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ.Dialog
{
    public class StartState : State<DialogManager>
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log("Start Dialog");
            context.DialogPanelController.gameObject.SetActive(true);

            stateMachine.ChangeState<DialogState>();
            EventBusSystem.Publish(EventBusType.DIALOG);

            //�÷��̾ NPC �������� �ٶ󺻴�.

            // 3��Ī �˵� ī�޶󿡼� NPC ī�޶�� ���� ����
        }
        public override void Update(float deltaTime) { }
    }

    public class DialogState : State<DialogManager>
    {

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            context.DialogPanelController.ShowDialog(context.CurrentDialog);
        }
        public override void Update(float deltaTime)
        {
            if (context.CurrentDialog.choices != string.Empty)
            {
                var choicesNextId = context.CurrentDialog.choicesNextId.Split('|');
                if (InputManager.Instance.InteractButton.ConsumeButtonPressed())
                {
                    DialogObject nextDialog = context.GetDialogObject(choicesNextId[0]);
                    if (nextDialog != null)
                    {
                        context.SetCurrentDialog(nextDialog);
                        context.DialogPanelController.ShowDialog(context.CurrentDialog);
                    }
                    else
                    {
                        stateMachine.ChangeState<EndState>();
                    }
                }
                else if (Input.GetButtonDown(ButtonName.Deny))
                {
                    DialogObject nextDialog = context.GetDialogObject(choicesNextId[1]);
                    if (nextDialog != null)
                    {
                        context.SetCurrentDialog(nextDialog);
                        context.DialogPanelController.ShowDialog(context.CurrentDialog);
                    }
                    else
                    {
                        stateMachine.ChangeState<EndState>();
                    }
                }
            }
            else
            {
                if (InputManager.Instance.InteractButton.ConsumeButtonPressed())
                {
                    DialogObject nextDialog = context.GetDialogObject(context.CurrentDialog.nextId);
                    if (nextDialog != null)
                    {
                        context.SetCurrentDialog(nextDialog);
                        context.DialogPanelController.ShowDialog(context.CurrentDialog);
                    }
                    else
                    {
                        stateMachine.ChangeState<EndState>();
                    }
                }
            }
        }
    }
    public class EndState : State<DialogManager>
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            context.DialogPanelController.gameObject.SetActive(false);
            EventBusSystem.Publish(EventBusType.START);

            
            // NPC ī�޶󿡼� 3��Ī �˵� ī�޶�� �̵�
            CameraController camController = Camera.main.GetComponent<CameraController>();
            camController.Reset();
            camController.MoveTo3PFromNPC();

            if (context.CanTrigger()) // ��ȭ ���� Ʈ���� �ߵ�
            {
                context.DoTrigger();
            }
        }
        public override void Update(float deltaTime) { }
    }
}

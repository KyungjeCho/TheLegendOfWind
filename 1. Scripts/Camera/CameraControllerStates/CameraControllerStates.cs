using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ.CameraControl
{
    public class IdleState : State<CameraController>
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            Debug.Log("Camera Idle");
        }
        public override void Update(float deltaTime) { }
    }
    public class MoveWithLerpState : State<CameraController>
    {
        private Transform myTransform;
        private Vector3 targetPos;
        private Quaternion targetRot;

        public override void OnInitialize()
        {
            base.OnInitialize();

            myTransform = context.transform;
        }
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            // Target ¼³Á¤
            targetPos = context.TargetPos;
            targetRot = context.TargetRot;

            Debug.Log("Move Lerp State: " + context.MoveType);
        }
        public override void Update(float deltaTime)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPos, deltaTime * context.Smooth);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRot, deltaTime * context.Smooth);

            if (Vector3.Distance(myTransform.position, targetPos) < Mathf.Epsilon + 0.01f)
            {
                stateMachine.ChangeState<IdleState>();
            }
        }
        public override void OnStateExit()
        {
            base.OnStateExit();

            if (context.MoveType == CameraMoveType.To3PFromNPC)
            {
                context.ThirdPersonOrbitCam.StartCamera();
            }
        }
    }
    public class MoveWithoutLerpState : State<CameraController>
    {
        public override void Update(float deltaTime)
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ.ThirdPersonCamStates
{
    public class PlayState : State<ThirdPersonOrbitCamera>
    {
        private Transform cameraTr;
        private Camera myCamerea;

        public override void OnInitialize()
        {
            base.OnInitialize();

            cameraTr = context.transform;
            myCamerea = cameraTr.GetComponent<Camera>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            context.IsPlaying = true;
        }

        public override void Update(float deltaTime)
        {
            context.angleH += Mathf.Clamp(InputManager.Instance.MouseX.ButtonValue, -1f, 1f) * context.horizontalAimingSpeed;
            context.angleV += Mathf.Clamp(InputManager.Instance.MouseY.ButtonValue, -1f, 1f) * context.verticalAimingSpeed;

            context.angleV = Mathf.Clamp(context.angleV, context.minVerticalAngle, context.TargetMaxVerticleAngle);
            context.angleV = Mathf.LerpAngle(context.angleV, context.angleV + context.RecoilAngle, 10f * deltaTime);

            Quaternion camYRotation = Quaternion.Euler(0.0f, context.angleV, 0.0f);
            Quaternion aimRotation = Quaternion.Euler(-context.angleV, context.angleH, 0.0f);
            cameraTr.rotation = aimRotation;

            myCamerea.fieldOfView = Mathf.Lerp(myCamerea.fieldOfView, context.TargetFOV, deltaTime);

            Vector3 baseTempPosition = context.player.position + camYRotation * context.TargetPivotOffset;
            Vector3 noCollisionOffset = context.TargetCamOffset;

            for (float zOffset = context.TargetCamOffset.z; zOffset <= 0f; zOffset += 0.5f)
            {
                noCollisionOffset.z = zOffset;
                if (context.DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0f)
                {
                    break;
                }
            }

            context.SmoothPivotOffset = Vector3.Lerp(context.SmoothPivotOffset, context.TargetPivotOffset, context.smooth * deltaTime);
            context.SmoothCamOffset = Vector3.Lerp(context.SmoothCamOffset, noCollisionOffset, context.smooth * deltaTime);

            cameraTr.position = context.player.position + camYRotation * context.SmoothPivotOffset + aimRotation * context.SmoothCamOffset;

            if (context.RecoilAngle > 0.0f)
            {
                context.RecoilAngle -= context.recoilAngleBounce * deltaTime;
            }
            else if (context.RecoilAngle < 0.0f)
            {
                context.RecoilAngle += context.recoilAngleBounce * deltaTime;
            }
        }
    }

    public class StopState : State<ThirdPersonOrbitCamera>
    {
        private Transform cameraTr;

        public override void OnInitialize()
        {
            base.OnInitialize();

            cameraTr = context.transform;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            context.PreviousCamPos = cameraTr.position;
            context.PreviousCamRot = cameraTr.rotation;
            context.IsPlaying = false;
        }
        public override void Update(float deltaTime)
        {
            
        }
    }
}
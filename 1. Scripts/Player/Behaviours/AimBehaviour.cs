using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 조준시 행동
    /// </summary>
    public class AimBehaviour : BaseBehaviour
    {
        public Texture2D crossHair;
        public float aimTurnSmoothing = 0.15f; // 조준할때의 회전 속도
        public Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f, 0.0f);
        public Vector3 aimCamOffset = new Vector3(0.0f, 0.4f, -0.7f);

        private Transform myTransform;
        private int aimBool;
        private bool aim;

        private Vector3 initialRootRotation; // IK 용
        private Vector3 initialHipRotation;

        private Vector3 initialSpineRotation;

        private void Start()
        {
            myTransform = transform;
            aimBool = Animator.StringToHash(AnimatorKey.Aim);

            Transform hips = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.Hips);
            initialRootRotation = (hips.parent == transform) ? Vector3.zero : hips.parent.localEulerAngles;
            initialHipRotation = hips.localEulerAngles;
            initialSpineRotation = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.Spine).localEulerAngles;

        }
        private void Update()
        {
            if (InputManager.Instance.AimButton.ButtonValue != 0 && !aim)
            {
                StartCoroutine(ToggleAimOn());
            }
            else if (aim && InputManager.Instance.AimButton.ButtonValue == 0)
            {
                StartCoroutine(ToggleAimOff());
            }

            canSprint = !aim;
            if (aim && Input.GetButtonDown(ButtonName.Shoulder))
            {
                aimCamOffset.x = aimCamOffset.x * (-1);
                aimPivotOffset.x = aimPivotOffset.x * (-1f);
            }

            behaviourController.GetAnimator.SetBool(aimBool, aim);
        }

        private void OnGUI()
        {
            if (crossHair != null)
            {
                float length = behaviourController.GetCamScipt.GetCurrentPivotMagnitude(aimPivotOffset);
                if (length < 0.05f)
                {
                    GUI.DrawTexture(new Rect(Screen.width * 0.5f - (crossHair.width * 0.5f),
                        Screen.height * 0.5f - (crossHair.height * 0.5f),
                        crossHair.width, crossHair.height), crossHair);
                }
            }
        }

        void Rotating()
        {
            Vector3 forward = behaviourController.playerCamera.TransformDirection(Vector3.forward);
            forward.y = 0.0f;
            forward = forward.normalized;

            Quaternion targetRotation = Quaternion.Euler(0f, behaviourController.GetCamScipt.GetH, 0.0f);
            float minSpeed = Quaternion.Angle(transform.rotation, targetRotation) * aimTurnSmoothing;

            behaviourController.SetLastDirection(forward);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, minSpeed * Time.deltaTime);

        }

        void AimManagement()
        {
            Rotating();
        }

        private IEnumerator ToggleAimOn()
        {
            yield return new WaitForSeconds(0.05f);
            // 조준이 불가능할 경우
            if (behaviourController.GetTempLockStatus(behaviourCode) || behaviourController.IsOverriding(this))
            {
                yield return false;
            }
            else
            {
                aim = true;
                int signal = 1;

                aimCamOffset.x = Mathf.Abs(aimCamOffset.x) * signal;
                aimPivotOffset.x = Mathf.Abs(aimPivotOffset.x) * signal;
                yield return new WaitForSeconds(0.1f);
                behaviourController.GetAnimator.SetFloat(speedFloat, 0.0f);
                behaviourController.OverrideWithBehaviour(this);
            }
        }

        private IEnumerator ToggleAimOff()
        {
            aim = false;
            yield return new WaitForSeconds(0.3f);
            behaviourController.GetCamScipt.ResetTargetOffsets();
            behaviourController.GetCamScipt.ResetMaxVerticalAngle();
            yield return new WaitForSeconds(0.1f);
            behaviourController.RevokeOverridingBehaviour(this);
        }

        public override void LocalFixedUpdate()
        {
            if (aim)
            {
                behaviourController.GetCamScipt.SetTargetOffset(aimPivotOffset, aimCamOffset);
            }
        }
        public override void LocalLateUpdate()
        {
            AimManagement();
        }

        
    }
}

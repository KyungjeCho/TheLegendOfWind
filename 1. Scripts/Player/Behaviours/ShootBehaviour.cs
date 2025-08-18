using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KJ
{
    public class ShootBehaviour : BaseBehaviour
    {
        // anim hash
        private int weaponInt;
        private int rangeAttackBool;

        //IK
        private Transform hips, spine, chest, rightHand, leftArm;
        private Vector3 initialRootRotation;
        private Vector3 initialHipsRotation;
        private Vector3 initialSpineRotation;
        private Vector3 initialChestRotation;

        private bool isAiming;


        private void Start()
        {
            weaponInt = Animator.StringToHash(AnimatorKey.Weapon);
            rangeAttackBool = Animator.StringToHash(AnimatorKey.RangeAttack);

            hips = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.Hips);
            spine = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.Spine);
            chest = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.Chest);
            rightHand = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            leftArm = behaviourController.GetAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);

            initialRootRotation = (hips.parent == transform) ? Vector3.zero : hips.parent.localEulerAngles;
            initialHipsRotation = hips.localEulerAngles;
            initialSpineRotation = spine.localEulerAngles;
            initialChestRotation = chest.localEulerAngles;

            isAiming = false;
        }

        private void Update()
        {
            if (isAiming && InputManager.Instance.AttackButton.IsPressed)
            {
                behaviourController.GetAnimator.SetBool(rangeAttackBool, true);
            }
            if (InputManager.Instance.AttackButton.IsPressedUp)
            {
                behaviourController.GetAnimator.SetBool(rangeAttackBool, false);
            }

            isAiming = behaviourController.GetAnimator.GetBool(AnimatorKey.Aim);
        }

        public void OnAnimatorIK(int layerIndex)
        {
            if (behaviourController.GetAnimator.GetInteger(weaponInt) == 2)
            {
                Quaternion targetRot = Quaternion.Euler(0.0f, transform.eulerAngles.y + 50f, 0.0f);
                targetRot *= Quaternion.Euler(initialRootRotation);
                targetRot *= Quaternion.Euler(initialHipsRotation);
                targetRot *= Quaternion.Euler(initialSpineRotation);
                behaviourController.GetAnimator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Inverse(hips.rotation) * targetRot);

                float xcamRot = Quaternion.LookRotation(behaviourController.playerCamera.forward).eulerAngles.x;
                targetRot = Quaternion.AngleAxis(xcamRot, this.transform.right);

                targetRot *= spine.rotation;
                targetRot *= Quaternion.Euler(initialChestRotation);
                behaviourController.GetAnimator.SetBoneLocalRotation(HumanBodyBones.Chest,
                    Quaternion.Inverse(spine.rotation) * targetRot);
            }
        }
    }
}



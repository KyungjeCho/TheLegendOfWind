
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 이동과 점프 담당
    /// 충돌처리 
    /// 기본 동작
    /// </summary>
    public class MoveBehaviour : BaseBehaviour
    {
        public float walkSpeed = 0.15f;
        public float runSpeed = 1.0f;
        public float sprintSpeed = 2.0f;
        public float speedDampTime = 0.1f;

        private bool jump;
        private bool crouched;
        public float jumpHeight = 1.5f;
        public float jumpInertiaForce = 10f;
        public float speed;
        private int jumpBool;
        private int groundedBool;
        private int crouchedBool;

        private bool isColliding;
        private bool isStopped = false;
        private CapsuleCollider capsuleCollider;
        private Transform myTransform;
        
        private void Start()
        {
            myTransform = transform;
            capsuleCollider = GetComponent<CapsuleCollider>();
            jumpBool = Animator.StringToHash(AnimatorKey.Jump);
            groundedBool = Animator.StringToHash(AnimatorKey.Grounded);
            crouchedBool = Animator.StringToHash(AnimatorKey.Crouched);
            behaviourController.GetAnimator.SetBool(groundedBool, true);

            behaviourController.SubscribeBehaviour(this);
            behaviourController.RegisterBehaviour(this.behaviourCode);

            EventBusSystem.Subscribe(EventBusType.START, () => isStopped = false);
            EventBusSystem.Subscribe(EventBusType.STOP, () => isStopped = true);
        }

        private void Update()
        {
            if (isStopped) return;

            // 점프 클릭
            if (!jump && InputManager.Instance.JumpButton.IsButtonPressed && behaviourController.IsCurrentBehaviour(behaviourCode) && !behaviourController.IsOverriding())
            {
                jump = true;
            }
            if (InputManager.Instance.CrouchButton.IsButtonPressed && behaviourController.IsCurrentBehaviour(behaviourCode) && !behaviourController.IsOverriding() && !jump && !behaviourController.IsSprinting())
            {
                crouched = !crouched;
            }
        }

        public override void LocalFixedUpdate()
        {
            if (isStopped) return;

            MovementManagement(behaviourController.GetH, behaviourController.GetV);
            JumpManagement();
            CrouchManagement();
        }

        Vector3 Rotating(float horizontal, float vertical)
        {
            Vector3 forward = behaviourController.playerCamera.TransformDirection(Vector3.forward);

            forward.y = 0.0f;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
            Vector3 targetDirection = Vector3.zero;
            targetDirection = forward * vertical + right * horizontal;

            if (behaviourController.IsMoving() && targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion newRotation = Quaternion.Slerp(behaviourController.GetRigidbody.rotation, targetRotation, behaviourController.GetTurnSmoothing);
                behaviourController.GetRigidbody.MoveRotation(newRotation);
                behaviourController.SetLastDirection(targetDirection);
            }
            if (!(Mathf.Abs(horizontal) > 0.9f || Mathf.Abs(vertical) > 0.9f))
            {
                behaviourController.Repositioning();
            }
            return targetDirection;
        }

        private void RemoveVerticalVelocity()
        {
            Vector3 horizontalVelocity = behaviourController.GetRigidbody.velocity;
            horizontalVelocity.y = 0.0f;
            behaviourController.GetRigidbody.velocity = horizontalVelocity;
        }

        void MovementManagement(float horizontal, float vertical)
        {
            if (behaviourController.IsGrounded())
            {
                behaviourController.GetRigidbody.useGravity = true;
            }
            else if (!behaviourController.GetAnimator.GetBool(jumpBool) && behaviourController.GetRigidbody.velocity.y > 0)
            {
                RemoveVerticalVelocity();
            }
            Rotating(horizontal, vertical);

            Vector2 dir = new Vector2(horizontal, vertical);
            speed = Vector2.ClampMagnitude(dir, 1f).magnitude;

            if (crouched)
            {
                speed = Mathf.Clamp(speed, 0f, walkSpeed);
            }
            else if (behaviourController.IsSprinting())
            {
                speed = sprintSpeed;
            }
            behaviourController.GetAnimator.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);

            behaviourController.GetRigidbody.velocity = myTransform.forward * speed * 6f + Vector3.up * behaviourController.GetRigidbody.velocity.y;
           
        }

        private void OnCollisionStay(Collision collision)
        {
            isColliding = true;
            if (behaviourController.IsCurrentBehaviour(GetBehaviourCode) && collision.GetContact(0).normal.y <= 0.1f)
            {
                float vel = behaviourController.GetAnimator.velocity.magnitude;
                Vector3 targetMove = Vector3.ProjectOnPlane(myTransform.forward, collision.GetContact(0).normal).normalized * vel;
                behaviourController.GetRigidbody.AddForce(targetMove, ForceMode.VelocityChange);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            isColliding = false;
        }

        void JumpManagement()
        {
            if (jump && !behaviourController.GetAnimator.GetBool(jumpBool) && behaviourController.IsGrounded())
            {
                behaviourController.LockTempBehaviour(behaviourCode);
                behaviourController.GetAnimator.SetBool(jumpBool, true);

                crouched = false;
                behaviourController.GetAnimator.SetBool(crouchedBool, false);
                // 마찰을 없앤다
                capsuleCollider.material.dynamicFriction = 0f;
                capsuleCollider.material.staticFriction = 0f;
                RemoveVerticalVelocity();
                float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
                velocity = Mathf.Sqrt(velocity);
                behaviourController.GetRigidbody.AddForce(Vector3.up * velocity, ForceMode.Impulse);
            }
            else if (behaviourController.GetAnimator.GetBool(jumpBool))
            {
                if (!isColliding && behaviourController.GetTempLockStatus())
                {
                    behaviourController.GetRigidbody.AddForce(myTransform.forward * jumpInertiaForce * Physics.gravity.magnitude * sprintSpeed, ForceMode.Acceleration);
                }
                if (behaviourController.GetRigidbody.velocity.y < 0f && behaviourController.IsGrounded())
                {
                    behaviourController.GetAnimator.SetBool(groundedBool, true);
                    capsuleCollider.material.dynamicFriction = 0.6f;
                    capsuleCollider.material.staticFriction = 0.6f;
                    jump = false;
                    behaviourController.GetAnimator.SetBool(jumpBool, false);
                    behaviourController.UnLockTempBehaviour(this.behaviourCode);
                }
            }
        }

        void CrouchManagement()
        {
            if (crouched && !behaviourController.GetAnimator.GetBool(AnimatorKey.Crouched) && behaviourController.IsGrounded())
            {
                behaviourController.GetAnimator.SetBool(crouchedBool, true);
            }
            else if (!crouched && behaviourController.GetAnimator.GetBool(AnimatorKey.Crouched))
            {
                behaviourController.GetAnimator.SetBool(crouchedBool, false);
            }
            else if (behaviourController.IsSprinting())
            {
                crouched = false;
                behaviourController.GetAnimator.SetBool(crouchedBool, false);
            }
        }
    }


}

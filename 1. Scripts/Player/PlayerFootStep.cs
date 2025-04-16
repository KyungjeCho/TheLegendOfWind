using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerFootStep : MonoBehaviour
    {
        public enum Foot
        {
            LEFT, RIGHT
        }

        public SoundList stepSound;

        private Animator myAnimator;
        private Rigidbody myRigidBody;
        private Transform leftFoot, rightFoot;
        private int groundedBool;
        private int speedFloat;
        private bool grounded;
        private float speed;

        private Foot step = Foot.LEFT;
        private float footDistance;
        public float threshold = 0.1f;
        
        private float oldDistance, maxDistance;

        private void Awake()
        {
            myAnimator = GetComponent<Animator>();
            myRigidBody = GetComponent<Rigidbody>();    
            leftFoot = myAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = myAnimator.GetBoneTransform(HumanBodyBones.RightFoot);

            groundedBool = Animator.StringToHash(AnimatorKey.Grounded);
            speedFloat = Animator.StringToHash(AnimatorKey.Speed);

            oldDistance = 0;
            maxDistance = 0;
            speed = 0;
        }

        private void PlayFootStep()
        {
            if (oldDistance < maxDistance)
            {
                return;
            }

            oldDistance = 0;
            maxDistance = 0;

            SoundManager.Instance.PlayOneShotEffect(stepSound, transform.position, 1f);
            //Debug.Log("FOOT STEP SOUND!");
        }

        private void Update()
        {
            grounded = myAnimator.GetBool(groundedBool);
            speed = myAnimator.GetFloat(speedFloat);

            if (grounded && speed > 0.9f) // 땅에 있고 이동 중일 경우
            {
                oldDistance = maxDistance;

                switch (step)
                {
                    case Foot.LEFT:
                        footDistance = leftFoot.position.y - transform.position.y;
                        maxDistance = footDistance > maxDistance ? footDistance : maxDistance;
                        if (footDistance <= threshold)
                        {
                            PlayFootStep();
                            step = Foot.RIGHT;
                        }
                        break;

                    case Foot.RIGHT:
                        footDistance = rightFoot.position.y - transform.position.y;
                        maxDistance = footDistance > maxDistance ? footDistance : maxDistance;
                        if (footDistance <= threshold)
                        {
                            PlayFootStep();
                            step = Foot.LEFT;
                        }
                        break;
                }

            }
        }
    }

}



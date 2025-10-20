using KJ.ThirdPersonCamStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 현재 행동, 기본 행동, 오버라이딩 행동, 잠긴 행동, 마우스 이동값
    /// 땅위에 있는지, GenericBehaviour 를 상속받은 동작들을 업데이트
    /// 캐릭터의 행동을 제어하는 클래스
    /// </summary>
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
    public class BehaviourController : MonoBehaviour, IAttackable
    {
        private List<BaseBehaviour> behaviours; 
        private List<BaseBehaviour> overrideBehaviours; //우선시 되는 행동
        private int currentBehaviour;   // 현재 동작 중인 행동
        private int defaultBehaviour;   // 기본 동작 행동
        private int behaviourLocked;    // 잠긴 행동

        //캐싱
        public Transform playerCamera;
        private Transform myTransform;
        private Animator myAnimator;
        private Rigidbody myRigidbody;
        private ThirdPersonOrbitCamera camScript;

        private float h; // h Axis
        private float v; // v Axis
        
        private float turnSmoothing = 0.06f; // 카메라가 보는 방향으로 움직일때 속도
        private bool changedFOV;
        [SerializeField]
        private float sprintFOV = 100f;
        private Vector3 lastDirection;
        private bool sprint;
        private int hFloat; // anim hash
        private int vFloat; // anim hash
        private int groundedBool; // anim hash
        private Vector3 colExtents; // 땅과 충돌체크를 위한 충돌체 영역

        public LayerMask targetMask;
        public float GetH { get => h; }
        public float GetV { get => v; }

        public float GetTurnSmoothing { get => turnSmoothing; }
        public ThirdPersonOrbitCamera GetCamScipt {  get => camScript; }
        public Rigidbody GetRigidbody { get => myRigidbody; }  
        public Animator GetAnimator { get => myAnimator; }
        public int GetDefaultBehaviour { get => defaultBehaviour; }

        private void Awake()
        {
            behaviours = new List<BaseBehaviour>();
            overrideBehaviours = new List<BaseBehaviour>();
            myTransform = transform;
            myAnimator = GetComponent<Animator>();
            hFloat = Animator.StringToHash(AnimatorKey.Horizontal);
            vFloat = Animator.StringToHash(AnimatorKey.Vertical);
            camScript = playerCamera.GetComponent<ThirdPersonOrbitCamera>();
            groundedBool = Animator.StringToHash(AnimatorKey.Grounded);

            camScript = playerCamera.GetComponent<ThirdPersonOrbitCamera>();
            myRigidbody = GetComponent<Rigidbody>();

            colExtents = GetComponent<Collider>().bounds.extents;
            

        }

        private void Update()
        {
            h = InputManager.Instance.HorizontalButton.ButtonValue;
            v = InputManager.Instance.VerticalButton.ButtonValue;

            myAnimator.SetFloat(hFloat, h, 0.1f, Time.deltaTime);
            myAnimator.SetFloat(vFloat, v, 0.1f, Time.deltaTime);

            sprint = InputManager.Instance.SprintButton.IsButtonPressed;
            if (IsSprinting())
            {
                changedFOV = true;
                camScript.SetFOV(sprintFOV);
            }
            else if (changedFOV)
            {
                camScript.ResetFOV();
                changedFOV = false;
            }

            myAnimator.SetBool(groundedBool, IsGrounded());
        }

        private void FixedUpdate()
        {
            bool isAnyBehaviourActive = false;
            if (behaviourLocked > 0 || overrideBehaviours.Count == 0)
            {
                foreach(BaseBehaviour behaviour in behaviours)
                {
                    if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode)
                    {
                        isAnyBehaviourActive = true;
                        behaviour.LocalFixedUpdate();
                    }
                }
            } 
            else
            {
                foreach(BaseBehaviour behaviour in overrideBehaviours)
                {
                    behaviour.LocalFixedUpdate();
                }
            }

            if (!isAnyBehaviourActive && overrideBehaviours.Count == 0)
            {
                myRigidbody.useGravity = true;
                Repositioning();
            }
        }
        private void LateUpdate()
        {
            if (behaviourLocked > 0 || overrideBehaviours.Count == 0)
            {
                foreach (BaseBehaviour behaviour in behaviours)
                {
                    if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode)
                    {
                        behaviour.LocalLateUpdate();
                    }
                }
            }
            else
            {
                foreach (BaseBehaviour behaviour in overrideBehaviours)
                {
                    behaviour.LocalLateUpdate();
                }
            }
        }

        public void Repositioning()
        {
            if (lastDirection != Vector3.zero)
            {
                lastDirection.y = 0; // y 이동을 없앤다. 3인칭게임에서
                Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
                Quaternion newRotation = Quaternion.Slerp(myRigidbody.rotation, targetRotation, turnSmoothing);
                myRigidbody.MoveRotation(newRotation);
            }
        }

        
        public bool IsMoving()
        {
            return Mathf.Abs(h) > Mathf.Epsilon || Mathf.Abs(v) > Mathf.Epsilon;
        }

        public bool IsHorizontalMoving()
        {
            return Mathf.Abs(h) > Mathf.Epsilon;
        }

        public bool CanSprint()
        {
            foreach(BaseBehaviour behaviour in behaviours)
            {
                if (!behaviour.CanSprint)
                {
                    return false;
                }
            }
            foreach(BaseBehaviour behaviour in overrideBehaviours)
            {
                if (!behaviour.CanSprint)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSprinting()
        {
            return sprint && IsMoving() && CanSprint();
        }

        public bool IsGrounded()
        {
            Ray ray = new Ray(myTransform.position + Vector3.up * 2 * colExtents.x, Vector3.down);
            return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f);
        }

        public void SubscribeBehaviour(BaseBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void RegisterDefaultBehaviour(int behaviourCode)
        {
            defaultBehaviour = behaviourCode;
            currentBehaviour = behaviourCode;
        }

        public void RegisterBehaviour(int behaviourCode)
        {
            if (currentBehaviour == defaultBehaviour)
            {
                currentBehaviour = behaviourCode;
            }
        }

        public void UnRegisterBehaviour(int behaviourCode)
        {
            if (currentBehaviour == behaviourCode)
            {
                currentBehaviour = defaultBehaviour;
            }
        }

        public bool OverrideWithBehaviour(BaseBehaviour overrideBehaviour)
        {
            if (!overrideBehaviours.Contains(overrideBehaviour))
            {
                if (overrideBehaviours.Count == 0)
                {
                    foreach(BaseBehaviour behaviour in behaviours)
                    {
                        if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode)
                        {
                            behaviour.OnOverride();
                            break;
                        }
                    }
                }
                overrideBehaviours.Add(overrideBehaviour);
                return true;
            }
            return false;
        }

        public bool RevokeOverridingBehaviour(BaseBehaviour behaviour)
        {
            if (overrideBehaviours.Contains(behaviour))
            {
                overrideBehaviours.Remove(behaviour);
                return true;
            }
            return false;
        }

        public bool IsOverriding(BaseBehaviour behaviour = null)
        {
            if (behaviour == null)
            {
                return overrideBehaviours.Count > 0;
            }
            return overrideBehaviours.Contains(behaviour);
        }

        public bool IsCurrentBehaviour(int behaviourCode)
        {
            return this.currentBehaviour == behaviourCode;
        }

        public bool GetTempLockStatus(int behaviourCode = 0)
        {
            return (behaviourLocked != 0 && behaviourLocked != behaviourCode);
        }
        public void LockTempBehaviour(int behaviourCode) 
        {
            if (behaviourLocked == 0)
            {
                behaviourLocked = behaviourCode;
            }
        }
        public void UnLockTempBehaviour(int behaviourCode)
        {
            if (behaviourLocked == behaviourCode)
            {
                behaviourLocked = 0;
            }
        }

        public Vector3 GetLastDirection()
        {
            return lastDirection;
        }

        public void SetLastDirection(Vector3 direction)
        {
            this.lastDirection = direction;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 2 * colExtents.x, transform.position + Vector3.down * (colExtents.x + 0.2f));
        }

        public void OnAttack()
        {
            ManualCollision manualCollision = GetComponent<ManualCollision>();

            Collider[] targetColliders = manualCollision.CheckOverlapBox(targetMask);
            foreach (Collider collider in targetColliders)
            {
                if (collider.GetComponent<IDamagable>() != null)
                {
                    //Debug.Log("todo : 플레이어 데미지 설정");
                    collider.GetComponent<IDamagable>().OnDamage(gameObject, 10f);
                }
            }

        }
    }

    public abstract class BaseBehaviour : MonoBehaviour
    {
        protected int speedFloat; // anim 해시
        protected BehaviourController behaviourController; 
        protected int behaviourCode; // 해시
        protected bool canSprint;

        public BehaviourController GetBehaviourController() => behaviourController;
        private void Awake()
        {
            behaviourController = GetComponent<BehaviourController>();
            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
            canSprint = true;

            behaviourCode = this.GetType().GetHashCode();
        }

        public int GetBehaviourCode
        {
            get => behaviourCode;
        }

        public bool CanSprint
        {
            get => canSprint;
        }


        public virtual void LocalLateUpdate()
        {

        }

        public virtual void LocalFixedUpdate()
        {

        }

        public virtual void OnOverride()
        {

        }
    }
}


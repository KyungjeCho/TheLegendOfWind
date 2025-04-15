using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(FieldOfView))]
    public abstract class EnemyController : MonoBehaviour
    {
        protected StateMachine<EnemyController> stateMachine;
        public virtual float AttackRange => 3.0f;

        protected NavMeshAgent myAgent;
        protected Animator myAnimator;

        private FieldOfView fieldOfView;

        public Transform Target => fieldOfView.NearestTarget;

        public NavMeshAgent GetAgent => myAgent;
        public Animator GetAnimator => myAnimator;
        public FieldOfView GetFOV => fieldOfView;
        protected virtual void Start()
        {
            stateMachine = new StateMachine<EnemyController>(this, new IdleState());
            
            myAgent = GetComponent<NavMeshAgent>();
            myAnimator = GetComponent<Animator>(); 
            fieldOfView = GetComponent<FieldOfView>();

            myAgent.updatePosition = true;
            myAgent.updateRotation = true;

            stateMachine.CurrentState.OnStateEnter();
        }

        protected virtual void Update()
        {
            stateMachine.Update(Time.deltaTime);
        }

        public R ChangeState<R>() where R : State<EnemyController>
        {
            return stateMachine.ChangeState<R>();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(FieldOfView))]
    public abstract class EnemyController : MonoBehaviour
    {
        public MonsterList monsterList;
        protected MonsterStat monsterStat;
        protected float currentHP;
        public float CurrentHP => currentHP;

        public SoundList hitSound;
        public SoundList dieSound;
        private SoundClip hitSoundClip;
        private SoundClip dieSoundClip;

        public SoundClip HitSoundClip => hitSoundClip;
        public SoundClip DieSoundClip => dieSoundClip;
        public virtual float AttackRange
        {
            get
            {
                if (monsterStat == null)
                {
                    return 1.0f;
                }
                return monsterStat.attackRange;
            }
        }

        protected StateMachine<EnemyController> stateMachine;
        
        protected NavMeshAgent myAgent;
        protected Animator myAnimator;

        private FieldOfView fieldOfView;

        public Transform Target => fieldOfView.NearestTarget;

        public NavMeshAgent GetAgent => myAgent;
        public Animator GetAnimator => myAnimator;
        public FieldOfView GetFOV => fieldOfView;

        protected virtual void Start()
        {
            // Data
            monsterStat = DataManager.MonsterData.GetCopy((int)monsterList);
            currentHP = monsterStat.maxHP;

            // StateMachine
            stateMachine = new StateMachine<EnemyController>(this, new IdleState());
            
            myAgent = GetComponent<NavMeshAgent>();
            myAnimator = GetComponent<Animator>(); 
            fieldOfView = GetComponent<FieldOfView>();

            myAgent.updatePosition = true;
            myAgent.updateRotation = true;
            myAgent.speed = monsterStat.speed;

            stateMachine.CurrentState.OnStateEnter();

            hitSoundClip = DataManager.SoundData.GetCopy((int)hitSound);
            hitSoundClip.PreLoad();
            dieSoundClip = DataManager.SoundData.GetCopy((int)dieSound);
            dieSoundClip.PreLoad();
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

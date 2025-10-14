using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class BossController : MonoBehaviour
    {
        public MonsterList monsterList;
        protected MonsterStat monsterStat;
        protected float currentHP;
        protected float maxHP;

        public event Action<float> OnHealthChanged;
        public float CurrentHP => currentHP;
        public float MaxHP => maxHP;

        public SoundList hitSound;
        public SoundList dieSound;
        public SoundList screamSound;
        public SoundList flameSound;

        private SoundClip hitSoundClip;
        private SoundClip dieSoundClip;
        private SoundClip screamSoundClip;
        private SoundClip flameSoundClip;

        public Vector3 originalPos;
        public float maxMoveDistance = 5f;

        public SoundClip HitSoundClip => hitSoundClip;
        public SoundClip DieSoundClip => dieSoundClip;
        public SoundClip ScreamSoundClip => screamSoundClip;
        public SoundClip FlameSoundClip => flameSoundClip;
        public MonsterStat MonsterStat => monsterStat;

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

        protected NavMeshAgent myAgent;
        protected Animator myAnimator;

        protected BTNode root;

        public DamagePipeline damagePipeline;
        private FieldOfView fieldOfView;

        public Transform Target => fieldOfView.NearestTarget;

        public NavMeshAgent GetAgent => myAgent;
        public Animator GetAnimator => myAnimator;
        public FieldOfView GetFOV => fieldOfView;
        public BTNode Root => root;

        protected virtual void Start()
        {
            // Data

            monsterStat = DataManager.MonsterData.GetCopy((int)monsterList);
            currentHP = monsterStat.maxHP;
            maxHP = monsterStat.maxHP;

            myAgent = GetComponent<NavMeshAgent>();
            myAnimator = GetComponent<Animator>();
            fieldOfView = GetComponent<FieldOfView>();

            myAgent.updatePosition = true;
            myAgent.updateRotation = true;
            myAgent.speed = monsterStat.speed;

            hitSoundClip = DataManager.SoundData.GetCopy((int)hitSound);
            hitSoundClip.PreLoad();
            dieSoundClip = DataManager.SoundData.GetCopy((int)dieSound);
            dieSoundClip.PreLoad();
            screamSoundClip = DataManager.SoundData.GetCopy((int)screamSound);
            screamSoundClip.PreLoad();
            flameSoundClip = DataManager.SoundData.GetCopy((int)flameSound);
            flameSoundClip.PreLoad();

            root = new BTSelector(new List<BTNode>());

            damagePipeline = new DamagePipeline();
        }

        protected virtual void Update()
        {
            root.Evaluate(Time.deltaTime);
        }
        public void HealthChanged(float health) => OnHealthChanged?.Invoke(health);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class DragonController : BossController, IDamagable
    {
        private BTIdleAction            idleAction;
        private BTMoveToTargetAction    moveAction;
        private BTMeleeAttackAction     meleeAttackAction;

        private BTSequence              halfHPGimmickSequencer;
        private BTReturnToCenterOfMap   returnToCenterOfMapAction;
        private BTScreamForGimmick      screamForGimmickAction;
        private BTAoECharging           aoeChargingAction;
        private BTAoEAttack             aoeAttackAction;

        private BTSequence              quarterHPGimmickSequencer;
        private BTInvincible            invincibleAction;
        private BTRockAttack            rockAttackAction;

        private BTConditional notCombatConditional;
        private BTConditional underHalfHPConditional;
        private BTConditional underQuarterHPConditional;

        private BTSequence meleeAttackSequencer;

        public Action onCombatStart;
        public Action onHalfHPGimmickStart;
        public Action onInvincibleBreak;
        public Action onRockAttackExit;

        public bool isCombatting = false;
        public bool isFirstHalfHPGimmick = true;

        public Vector3 centerOfMap = Vector3.zero;
        public ParticleSystem imposionVFX;
        public ParticleSystem shockwaveVFX;

        private int isAliveBool;

        private RockSlide rockSlide;
        private GlobalAoEAttack globalAoEAttack;
        private BossInvincible bossInvincible;
        private MaterialPropertyBlock propBlock;
        private SkinnedMeshRenderer meshRenderer;
        private readonly int intensityId = Shader.PropertyToID("_Highlight_Intensity");

        public GameObject uiBossHealth;

        public bool IsUnderHalfHP => CurrentHP < MaxHP * 0.5f;
        public bool IsUnderQuarterHP => CurrentHP < MaxHP * 0.25f;
        public bool IsAlive => CurrentHP > 0f ? true : false;
        public RockSlide RockSlide => rockSlide;
        public GlobalAoEAttack GetGlobalAoEAttack => globalAoEAttack;
        public BossInvincible GetBossInvincible => bossInvincible;

        public void OnDamage(GameObject target, float damage)
        {
            // Can Get hit ?
            if (!IsAlive)
            {
                GetAnimator.SetBool(isAliveBool, false);
                return;
            }
            else
            {
                // hit effect
                EffectManager.Instance.PlayEffect(EffectList.HitBlood, transform.position + transform.up + transform.forward);
                // hit sound
                SoundManager.Instance.PlayEffectSound(HitSoundClip, transform.position, 1f);

                // Damage Calc
                currentHP -= damagePipeline.Calculate(damage);
                HealthChanged(currentHP / maxHP);

                HomingRockController homingRockController = target.GetComponent<HomingRockController>();

                if (homingRockController != null)
                {
                    // µ¹ ¸ÂÀ» °æ¿ì

                    // ±×·Î±â (5ÃÊ ¸ØÃã)
                    // ¹«Àû ±úÁü RemoveInvincibleDamageHandler
                    // ¼ÎÀÌ´õ ²¨Áü
                    onInvincibleBreak?.Invoke();
                }
                if (currentHP <= 0f)
                {
                    GetAnimator.SetBool(isAliveBool, false);
                }
                else
                {
                    //transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position));
                    // hit animation

                }

            }
        }

        protected override void Start()
        {
            base.Start();

            isAliveBool = Animator.StringToHash(AnimatorKey.IsAlive);

            rockSlide = GetComponent<RockSlide>();
            globalAoEAttack = GetComponent<GlobalAoEAttack>();
            bossInvincible = GetComponent<BossInvincible>();

            onCombatStart += StartCombat;

            idleAction              = new BTIdleAction(this);
            notCombatConditional    = new BTConditional(idleAction, () => isCombatting == false);

            moveAction              = new BTMoveToTargetAction(this);
            meleeAttackAction       = new BTMeleeAttackAction(this);

            meleeAttackSequencer    = new BTSequence(new List<BTNode>()
            {
                moveAction, meleeAttackAction
            });

            #region Half HP Gimmick BT
            returnToCenterOfMapAction   = new BTReturnToCenterOfMap(this);
            screamForGimmickAction      = new BTScreamForGimmick(this);
            aoeChargingAction           = new BTAoECharging(this);
            aoeAttackAction             = new BTAoEAttack(this);

            halfHPGimmickSequencer      = new BTSequence(new List<BTNode>() { 
                returnToCenterOfMapAction, screamForGimmickAction, aoeChargingAction, aoeAttackAction
            });

            underHalfHPConditional      = new BTConditional(halfHPGimmickSequencer, () => IsUnderHalfHP && isFirstHalfHPGimmick);
            #endregion

            #region Quarter HP Gimmick BT
            invincibleAction            = new BTInvincible(this);
            rockAttackAction            = new BTRockAttack(this);

            quarterHPGimmickSequencer   = new BTSequence(new List<BTNode>()
            {
                invincibleAction, rockAttackAction
            });

            underQuarterHPConditional   = new BTConditional(quarterHPGimmickSequencer, () => IsUnderQuarterHP);
            #endregion
            root = new BTMemorySelector(new List<BTNode>()
            {
                notCombatConditional,
                underHalfHPConditional,
                underQuarterHPConditional,
                meleeAttackSequencer
            }) ;

            propBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        protected override void Update()
        {
            base.Update();
        }

        public void StartCombat()
        {
            isCombatting = true;
            uiBossHealth.SetActive(true);
        }

        public void OnMeleeAttackEvent()
        {
            // ¹Ð¸® ¾îÅÃ ÀÌ¹êÆ® ½ÇÇà½Ã
        }

        public void SetHighlight(bool isEnabled)
        {
            meshRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat(intensityId, isEnabled ? 1f : 0f);
            meshRenderer.SetPropertyBlock(propBlock);
        }
    }
}

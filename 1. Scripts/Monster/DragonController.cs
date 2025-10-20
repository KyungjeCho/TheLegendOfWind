using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

namespace KJ
{
    public class DragonController : BossController, IDamagable
    {
        private BTIdleAction            idleAction;
        private BTMoveToTargetAction    moveAction;
        private BTMeleeAttackAction     meleeAttackAction;

        private BTGroggyAction          groggyAction;

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
        private BTConditional isGroggyConditional;

        private BTSequence meleeAttackSequencer;

        public Action onCombatStart;
        public Action onHalfHPGimmickStart;
        public Action onInvincibleBreak;
        public Action onRockAttackExit;

        public bool isCombatting            = false;
        public bool isFirstHalfHPGimmick    = true;
        public bool isGroggy                = false;

        public float groggyDuration = 5f;

        public LayerMask targetMask;

        public Vector3 centerOfMap = Vector3.zero;
        public ParticleSystem imposionVFX;
        public ParticleSystem shockwaveVFX;

        public PlayableDirector introTimeline;
        public PlayableDirector deadTimeline;

        #region Anim Hash
        private int isAliveBool;
        private int screamTrigger;
        private int getHitTrigger;
        private int dieTrigger;
        #endregion

        private RockSlide rockSlide;
        private GlobalAoEAttack globalAoEAttack;
        private BossInvincible bossInvincible;
        private ManualCollision manualCollision;
        private HitFlashEffect hitFlashEffect;
        private MaterialPropertyBlock propBlock;
        private SkinnedMeshRenderer meshRenderer;
        private ShakeCam shakeCam;
        private readonly int intensityId = Shader.PropertyToID("_Highlight_Intensity");

        public GameObject uiBossHealth;
        public PlayerHealth playerHealth;

        public bool IsUnderHalfHP => CurrentHP < MaxHP * 0.5f;
        public bool IsUnderQuarterHP => CurrentHP < MaxHP * 0.25f;
        public bool IsAlive => CurrentHP > 0f ? true : false;
        public RockSlide RockSlide => rockSlide;
        public GlobalAoEAttack GetGlobalAoEAttack => globalAoEAttack;
        public BossInvincible GetBossInvincible => bossInvincible;
        public ShakeCam GetShakeCam => shakeCam;

        public void OnDamage(GameObject target, float damage)
        {
            // Can Get hit ?
            if (!IsAlive)
            {
                return;
            }
            else
            {
                // hit effect
                EffectManager.Instance.PlayEffect(EffectList.HitBlood, transform.position + transform.up + transform.forward);
                // hit sound
                SoundManager.Instance.PlayEffectSound(HitSoundClip, transform.position, 1f);
                hitFlashEffect.Flash();
                // Damage Calc
                currentHP -= damagePipeline.Calculate(damage);
                HealthChanged(currentHP / maxHP);

                HomingRockController homingRockController = target.GetComponent<HomingRockController>();

                if (homingRockController != null)
                {
                    StartCoroutine(GroggyRoutine());
                    GetAnimator.SetTrigger(getHitTrigger);
                    onInvincibleBreak?.Invoke();
                }
                if (currentHP <= 0f)
                {
                    onDragonDie?.Invoke();
                }
                
            }
        }

        protected override void Start()
        {
            base.Start();

            isAliveBool = Animator.StringToHash(AnimatorKey.IsAlive);
            screamTrigger = Animator.StringToHash(AnimatorKey.Scream);
            getHitTrigger = Animator.StringToHash(AnimatorKey.GetHit);
            dieTrigger = Animator.StringToHash(AnimatorKey.Die);

            rockSlide = GetComponent<RockSlide>();
            globalAoEAttack = GetComponent<GlobalAoEAttack>();
            bossInvincible = GetComponent<BossInvincible>();
            manualCollision = GetComponent<ManualCollision>();
            propBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            hitFlashEffect = GetComponent<HitFlashEffect>();
            shakeCam = GetComponent<ShakeCam>();

            if (playerHealth != null)
            {
                playerHealth.OnPlayereDie += ResetDragon;
            }

            onCombatStart += StartCombat;
            onDragonDie += Dead;

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

            groggyAction                = new BTGroggyAction(this);

            isGroggyConditional         = new BTConditional(groggyAction, () => isGroggy);

            root = new BTMemorySelector(new List<BTNode>()
            {
                isGroggyConditional,
                notCombatConditional,
                underHalfHPConditional,
                underQuarterHPConditional,
                meleeAttackSequencer
            }) ;
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
            // Play Sound
            if (attackSounds.Length > 0)
            {
                int idx = UnityEngine.Random.Range(0, attackSounds.Length);
                SoundManager.Instance.PlayOneShotEffect(attackSounds[idx], transform.position, 1f);
            }
            // 밀리 어택 이밴트 실행시
            Collider[] targetsInManualCollision = manualCollision.CheckOverlapBox(targetMask);

            foreach(Collider collider in targetsInManualCollision)
            {
                if (collider.GetComponent<IDamagable>() != null)
                {
                    collider.GetComponent<IDamagable>().OnDamage(gameObject, monsterStat.attack);
                }
            }
        }

        public void SetHighlight(bool isEnabled)
        {
            meshRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat(intensityId, isEnabled ? 1f : 0f);
            meshRenderer.SetPropertyBlock(propBlock);
        }

        public void ResetDragon()
        {
            isCombatting = false;
            isFirstHalfHPGimmick = true;
            transform.position = centerOfMap; 
            transform.rotation = Quaternion.identity;
            currentHP = maxHP;
        }

        public void OnScreamStart()
        {
            introTimeline.Play();
            myAnimator.SetTrigger(screamTrigger);
            SoundManager.Instance.PlayOneShotEffect(screamSound, transform.position, 1f);
            shakeCam.StartShake();
        }

        public void OnScreamEnd()
        {
            onCombatStart?.Invoke();
        }
        
        private IEnumerator GroggyRoutine()
        {
            isGroggy = true;
            yield return new WaitForSeconds(groggyDuration);
            isGroggy = false;
        }

        public void Dead()
        {
            root = new BTSelector(new List<BTNode>());

            myAnimator.SetBool(isAliveBool, false);
            myAnimator.SetTrigger(dieTrigger);
            SoundManager.Instance.PlayOneShotEffect(dieSound, transform.position, 1f);

            deadTimeline.Play();
            shakeCam.StartShake();

        }
        #region Test
        public void TestQuarterHPGimmick()
        {
            isCombatting = true;
            isFirstHalfHPGimmick = false;
            currentHP = maxHP * 0.25f - 1f; 
        }
#if UNITY_EDITOR
        void OnGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 18;

            GUILayout.BeginArea(new Rect(10, 10, 200, 200));

            if (GUILayout.Button("25% 기믹 실행", style))
            {
                TestQuarterHPGimmick();
            }
            if (GUILayout.Button("죽음 실행", style))
            {
                OnDamage(gameObject, 20000);
            }
            GUILayout.EndArea();
        }
#endif
        #endregion
    }
}

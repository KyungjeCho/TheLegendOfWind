using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCombo : StateMachineBehaviour
{
    private AttackBehaviour attackBehaviour;
    private bool nextCombo;

    public int comboStep = 1;
    public SoundList attackSound;

    private int meleeAttackComboInt;
    private int meleeAttackTrigger;
    private SoundClip attackSoundClip;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackBehaviour = animator.GetComponent<AttackBehaviour>();
        nextCombo = false;
        attackBehaviour.SetIsAttacking(true);

        meleeAttackComboInt = Animator.StringToHash(AnimatorKey.MeleeAttackCombo);
        meleeAttackTrigger = Animator.StringToHash(AnimatorKey.MeleeAttack);
        attackSoundClip = DataManager.SoundData.GetCopy((int)attackSound);
        SoundManager.Instance.PlayEffectSound(attackSoundClip, animator.transform.position, 1.0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f && stateInfo.normalizedTime < 0.9f)
        {
            if (Input.GetButtonDown(ButtonName.Attack))
            {
                nextCombo = true;
            }
        }

        if (stateInfo.normalizedTime > 0.9f)
        {
            if (nextCombo && comboStep < 3)
            {
                attackBehaviour.GetBehaviourController().GetAnimator.SetInteger(meleeAttackComboInt, comboStep + 1);
                attackBehaviour.GetBehaviourController().GetAnimator.SetTrigger(meleeAttackTrigger);
            }
            else
            {
                attackBehaviour.GetBehaviourController().GetAnimator.SetInteger(meleeAttackComboInt, 0);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!nextCombo || comboStep == 3)
            attackBehaviour.SetIsAttacking(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

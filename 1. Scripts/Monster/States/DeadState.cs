using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DeadState : State<EnemyController>
    {
        public int dieBool;

        public override void OnInitialize()
        {
            dieBool = Animator.StringToHash(AnimatorKey.Die);
        }

        public override void OnStateEnter()
        {
            SoundManager.Instance.PlayEffectSound(context.DieSoundClip, context.transform.position, 1f);
            context.GetAnimator.SetBool(dieBool, true);
            GameEvent.PublishMonsterKilled(context.monsterList);
            GameObject.Destroy(context.gameObject, 5f);
        }

        public override void Update(float deltaTime)
        {
            
        }
    }

}

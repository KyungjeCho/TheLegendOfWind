using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    public class BossInvincible : MonoBehaviour
    {
        private BossController context;
        // 무적 효과 (노랑색)

        // 데미지 헨들러

        // 무적 효과 해제, 이펙트, 일시 그로기 
        private InvincibleModifier invincibleModifier;

        public InvincibleModifier InvincibleModifier
        {
            get
            {
                if (invincibleModifier == null)
                {
                    invincibleModifier = new InvincibleModifier(); 
                }
                return invincibleModifier;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            context = GetComponent<BossController>();
        }
        public void AddInvincibleModifier()
        {
            context.damagePipeline.AddModifier(InvincibleModifier);
        }
        public void RemoveInvincibleModifier()
        {
            context.damagePipeline.RemoveModifier(InvincibleModifier);
        }
    }
}
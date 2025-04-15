using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum MonsterType
    { 
        None        = -1,
        Melee       = 0,
        Projectile  = 1
    }

    [Serializable]
    public class MonsterStat 
    {
        public MonsterType monsterType  = MonsterType.None;
        public float maxHP              = 0.0f;
        public float attack             = 0.0f; // 공격력
        public float defense            = 0.0f; // 방어력
        public float attackRange        = 1.0f; // 공격 사거리
        public float speed              = 1.0f; // 스피드
        public float dropExp            = 0.0f; // 처치 시 경험치량
    }
}

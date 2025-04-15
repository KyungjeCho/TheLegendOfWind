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
        public float attack             = 0.0f; // ���ݷ�
        public float defense            = 0.0f; // ����
        public float attackRange        = 1.0f; // ���� ��Ÿ�
        public float speed              = 1.0f; // ���ǵ�
        public float dropExp            = 0.0f; // óġ �� ����ġ��
    }
}

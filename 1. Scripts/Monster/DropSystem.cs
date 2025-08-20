using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;
        [Range(0f, 1f)]
        public float probability; // 1 -> 100% drop;
    }
    [RequireComponent(typeof(EnemyController))]
    public class DropSystem : MonoBehaviour
    {
        // 골드 + 경험치 + 아이템 
        private EnemyController enemyController;
        [SerializeField]
        private int gold;
        private float exp;

        [SerializeField]
        private List<DropItem> dropItems = new List<DropItem>();

        // Start is called before the first frame update
        void Start()
        {
            enemyController = GetComponent<EnemyController>();
            MonsterStat monsterStat = DataManager.MonsterData.GetCopy((int)enemyController.monsterList);
            exp = monsterStat.dropExp;
        }

        public bool Drop(GameObject other)
        {
            other.gameObject.GetComponent<PlayerExp>()?.AddExp(exp);
            other.gameObject.GetComponent<PlayerGold>()?.AddGold(gold);

            foreach (var dropItem in dropItems)
            {
                float r = UnityEngine.Random.Range(0f, 1f);
                if (r < dropItem.probability)
                {
                    Instantiate(dropItem.itemPrefab, transform.position, Quaternion.identity);
                }
            }
            return true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DeadState : State<EnemyController>
    {
        

        public override void Update(float deltaTime)
        {
            GameObject.Destroy(context.gameObject);
        }
    }

}

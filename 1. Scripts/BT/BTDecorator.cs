using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BTDecorator : BTNode
    {
        protected BTNode child;

        public BTDecorator(BTNode child)
        {
            this.child = child;
        }
    }
    
    public class BTConditional : BTDecorator
    {
        private Func<bool> condition;

        public BTConditional(BTNode child, Func<bool> condition) : base(child) 
        {
            this.condition = condition;
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            if  (condition())
            {
                return child.Evaluate(deltaTime);
            }

            state = BTNodeState.Failure;
            return state;
        }
    }
}

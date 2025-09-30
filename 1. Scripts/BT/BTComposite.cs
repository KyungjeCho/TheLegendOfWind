using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BTComposite : BTNode
    {
        protected List<BTNode> children = new List<BTNode>();

        public BTComposite(List<BTNode> children)
        {
            this.children = children;
        }
    }

    /// <summary>
    /// Selector
    /// ��� �� Success ���� ������ Ȯ��
    /// </summary>
    public class BTSelector : BTComposite
    {
        public BTSelector(List<BTNode> children) : base(children) { }

        public override BTNodeState Evaluate()
        {
            foreach (BTNode child in children)
            {
                switch (child.Evaluate())
                {
                    case BTNodeState.Success:
                        state = BTNodeState.Success;
                        return state;
                    case BTNodeState.Running:
                        state = BTNodeState.Running;
                        return state;
                }
            }
            state = BTNodeState.Failure;
            return state;
        }
    }

    /// <summary>
    /// Sequence 
    /// ��� �ڽ� ��尡 Success �� ���
    /// </summary>
    public class BTSequence : BTComposite
    {
        public BTSequence(List<BTNode> children) : base(children) { }

        public override BTNodeState Evaluate()
        {
            bool anyChildRunning = false;

            foreach (BTNode child in children)
            {
                BTNodeState result = child.Evaluate();
                if (result == BTNodeState.Failure)
                {
                    state = BTNodeState.Failure;
                    return state;
                }

                if (result == BTNodeState.Running)
                {
                    anyChildRunning = true;
                }
            }

            state = anyChildRunning ? BTNodeState.Running : BTNodeState.Success;
            return state;
        }
    }
}

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
    /// 노드 중 Success 나올 때까지 확인
    /// </summary>
    public class BTSelector : BTComposite
    {
        public BTSelector(List<BTNode> children) : base(children) { }

        public override BTNodeState Evaluate(float deltaTime)
        {
            foreach (BTNode child in children)
            {
                switch (child.Evaluate(deltaTime))
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
    /// Memory Selector
    /// Running이 나오면 그 child만 호출하는 셀렉터
    /// </summary>
    public class BTMemorySelector : BTComposite
    {
        private int currentChild = -1;
        public BTMemorySelector(List<BTNode> children) : base(children)
        {
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            if (currentChild != -1)
            {
                var status = children[currentChild].Evaluate(deltaTime);
                if (status == BTNodeState.Running)
                    return BTNodeState.Running;
                currentChild = -1; // 종료 시 초기화
            }

            for (int i = 0; i < children.Count; i++)
            {
                var status = children[i].Evaluate(deltaTime);
                if (status != BTNodeState.Failure)
                {
                    currentChild = i;
                    return status;
                }
            }

            return BTNodeState.Failure;
        }
    }
    /// <summary>
    /// Sequence 
    /// 모든 자식 노드가 Success 일 경우
    /// </summary>
    public class BTSequence : BTComposite
    {
        public BTSequence(List<BTNode> children) : base(children) { }

        public override BTNodeState Evaluate(float deltaTime)
        {
            bool anyChildRunning = false;

            foreach (BTNode child in children)
            {
                BTNodeState result = child.Evaluate(deltaTime);
                if (result == BTNodeState.Failure)
                {
                    state = BTNodeState.Failure;
                    return state;
                }

                if (result == BTNodeState.Running)
                {
                    anyChildRunning = true;
                    state = BTNodeState.Running;
                    return state;
                }
            }

            state = anyChildRunning ? BTNodeState.Running : BTNodeState.Success;
            return state;
        }
    }
}

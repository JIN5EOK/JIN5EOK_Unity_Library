using System.Collections.Generic;

namespace Jin5eok
{
    /// <summary>
    /// 행동 트리 노드의 기본 인터페이스입니다.
    /// </summary>
    public interface IBehaviourTreeNode
    {
        /// <summary>
        /// 자식 노드를 추가합니다.
        /// </summary>
        /// <param name="child">추가할 자식 노드</param>
        /// <returns>체이닝을 위한 자기 자신</returns>
        public IBehaviourTreeNode AddChild(IBehaviourTreeNode child);
        /// <summary>
        /// 노드를 실행합니다.
        /// </summary>
        /// <returns>실행 성공 여부</returns>
        public bool Execute();
    }

    /// <summary>
    /// 행동 트리 노드의 추상 베이스 클래스입니다.
    /// 자식 노드들을 관리하고 실행할 수 있는 기본 기능을 제공합니다.
    /// </summary>
    public abstract class BehaviourTreeNode : IBehaviourTreeNode 
    {
        protected List<IBehaviourTreeNode> Children { get; set; }
        
        /// <summary>
        /// 자식 노드를 추가합니다.
        /// </summary>
        /// <param name="child">추가할 자식 노드</param>
        /// <returns>체이닝을 위한 자기 자신</returns>
        public IBehaviourTreeNode AddChild(IBehaviourTreeNode child)
        {
            if (Children != null)
            {
                Children.Add(child);    
            }
            return this;
        }

        public abstract bool Execute();
    }
}
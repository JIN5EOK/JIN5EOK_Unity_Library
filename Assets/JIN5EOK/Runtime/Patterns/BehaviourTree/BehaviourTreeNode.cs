using System.Collections.Generic;

namespace Jin5eok
{
    /// <summary>
    /// 행동 트리 노드의 실행 결과 상태입니다.
    /// </summary>
    public enum BTStatus
    {
        Success,
        Failure,
        Running
    }
    
    /// <summary>
    /// 행동 트리 노드의 기본 인터페이스입니다.
    /// </summary>
    /// <typeparam name="TContext">노드 실행 시 사용할 컨텍스트 타입</typeparam>
    public interface IBehaviourTreeNode<TContext>
    {
        /// <summary>
        /// 자식 노드를 추가합니다.
        /// </summary>
        /// <param name="child">추가할 자식 노드</param>
        /// <returns>체이닝을 위한 자기 자신</returns>
        IBehaviourTreeNode<TContext> AddChild(IBehaviourTreeNode<TContext> child);

        /// <summary>
        /// 노드를 실행합니다.
        /// </summary>
        /// <param name="context">실행에 사용할 컨텍스트</param>
        /// <returns>실행 결과 상태</returns>
        BTStatus Execute(TContext context);
    }

    /// <summary>
    /// 행동 트리 노드의 추상 베이스 클래스입니다.
    /// 자식 노드들을 관리하고 실행할 수 있는 기본 기능을 제공합니다.
    /// </summary>
    /// <typeparam name="TContext">노드 실행 시 사용할 컨텍스트 타입</typeparam>
    public abstract class BehaviourTreeNode<TContext> : IBehaviourTreeNode<TContext>
    {
        protected List<IBehaviourTreeNode<TContext>> Children { get; }

        protected BehaviourTreeNode()
        {
            Children = new List<IBehaviourTreeNode<TContext>>();
        }

        /// <summary>
        /// 자식 노드를 추가합니다.
        /// </summary>
        /// <param name="child">추가할 자식 노드</param>
        /// <returns>체이닝을 위한 자기 자신</returns>
        public IBehaviourTreeNode<TContext> AddChild(IBehaviourTreeNode<TContext> child)
        {
            if (child != null)
            {
                Children.Add(child);
            }

            return this;
        }

        /// <inheritdoc />
        public abstract BTStatus Execute(TContext context);
    }
}

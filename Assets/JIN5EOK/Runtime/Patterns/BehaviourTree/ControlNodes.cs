namespace Jin5eok
{
    /// <summary>
    /// 순차 실행 노드입니다. 모든 자식 노드를 순서대로 실행하며, 하나라도 실패하면 즉시 Failure를 반환합니다.
    /// 자식이 Running이면 Running을 전파합니다.
    /// </summary>
    public class SequencerNode<TContext> : BehaviourTreeNode<TContext>
    {
        public override BTStatus Execute(TContext context)
        {
            foreach (var child in Children)
            {
                var status = child.Execute(context);
                if (status == BTStatus.Failure)
                {
                    return BTStatus.Failure;
                }

                if (status == BTStatus.Running)
                {
                    return BTStatus.Running;
                }
            }

            return BTStatus.Success;
        }
    }

    /// <summary>
    /// 병렬 순차 실행 노드입니다. 모든 자식 노드를 병렬로 실행하며, 모든 노드가 성공해야 Success를 반환합니다.
    /// 하나라도 실패하면 Failure, 하나라도 Running이면 Running을 반환합니다.
    /// </summary>
    public class ParallelSequencerNode<TContext> : BehaviourTreeNode<TContext>
    {
        /// <inheritdoc />
        public override BTStatus Execute(TContext context)
        {
            var hasRunning = false;
            foreach (var child in Children)
            {
                var status = child.Execute(context);
                if (status == BTStatus.Failure)
                {
                    return BTStatus.Failure;
                }

                if (status == BTStatus.Running)
                {
                    hasRunning = true;
                }
            }

            return hasRunning ? BTStatus.Running : BTStatus.Success;
        }
    }

    /// <summary>
    /// 선택 노드입니다. 자식 노드를 순서대로 실행하며, 하나라도 성공하면 즉시 Success를 반환합니다.
    /// 자식이 Running이면 Running을 전파합니다.
    /// </summary>
    public class SelectorNode<TContext> : BehaviourTreeNode<TContext>
    {
        /// <inheritdoc />
        public override BTStatus Execute(TContext context)
        {
            foreach (var child in Children)
            {
                var status = child.Execute(context);
                if (status == BTStatus.Success)
                {
                    return BTStatus.Success;
                }

                if (status == BTStatus.Running)
                {
                    return BTStatus.Running;
                }
            }

            return BTStatus.Failure;
        }
    }

    /// <summary>
    /// 병렬 선택 노드입니다. 모든 자식 노드를 병렬로 실행하며, 하나라도 성공하면 Success를 반환합니다.
    /// 모두 실패하면 Failure, 하나라도 Running이면 Running을 반환합니다.
    /// </summary>
    public class ParallelSelectorNode<TContext> : BehaviourTreeNode<TContext>
    {
        /// <inheritdoc />
        public override BTStatus Execute(TContext context)
        {
            var hasRunning = false;
            foreach (var child in Children)
            {
                var status = child.Execute(context);
                if (status == BTStatus.Success)
                {
                    return BTStatus.Success;
                }

                if (status == BTStatus.Running)
                {
                    hasRunning = true;
                }
            }

            return hasRunning ? BTStatus.Running : BTStatus.Failure;
        }
    }
}

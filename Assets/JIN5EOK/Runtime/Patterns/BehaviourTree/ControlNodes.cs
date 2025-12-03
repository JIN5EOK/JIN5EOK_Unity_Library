namespace Jin5eok
{
    /// <summary>
    /// 순차 실행 노드입니다. 모든 자식 노드를 순서대로 실행하며, 하나라도 실패하면 즉시 false를 반환합니다.
    /// </summary>
    public class SequencerNode : BehaviourTreeNode
    {
        public override bool Execute()
        {
            foreach (var child in Children)
            {
                if (child.Execute() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
    
    /// <summary>
    /// 병렬 순차 실행 노드입니다. 모든 자식 노드를 병렬로 실행하며, 모든 노드가 성공해야 true를 반환합니다.
    /// </summary>
    public class ParallelSequencerNode : BehaviourTreeNode
    {
        public override bool Execute()
        {
            var isSucceedAll = true;
            foreach (var child in Children)
            {
                isSucceedAll &= child.Execute();
            }
            return isSucceedAll;
        }
    }
    
    /// <summary>
    /// 선택 노드입니다. 자식 노드를 순서대로 실행하며, 하나라도 성공하면 즉시 true를 반환합니다.
    /// </summary>
    public class SelectorNode : BehaviourTreeNode
    {
        public override bool Execute() 
        {
            foreach (var child in Children)
            {
                if (child.Execute() == true) return true; 
            }
            return false;
        }
    }
    
    /// <summary>
    /// 병렬 선택 노드입니다. 모든 자식 노드를 병렬로 실행하며, 하나라도 성공하면 true를 반환합니다.
    /// </summary>
    public class ParallelSelectorNode : BehaviourTreeNode
    {
        public override bool Execute()
        {
            var isSucceed = false;
            foreach (var child in Children)
            {
                isSucceed |= child.Execute();
            }
            return isSucceed;
        }
    }
}
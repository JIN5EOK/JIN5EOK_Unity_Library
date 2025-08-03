namespace Jin5eok
{
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
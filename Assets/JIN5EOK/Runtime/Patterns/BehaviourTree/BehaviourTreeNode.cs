using System.Collections.Generic;

namespace Jin5eok
{
    public interface IBehaviourTreeNode
    {
        public IBehaviourTreeNode AddChild(IBehaviourTreeNode child);
        public bool Execute();
    }

    public abstract class BehaviourTreeNode : IBehaviourTreeNode 
    {
        protected List<IBehaviourTreeNode> Children { get; set; }
        
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
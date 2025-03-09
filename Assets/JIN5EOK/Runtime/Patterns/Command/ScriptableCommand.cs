using UnityEngine;

namespace Jin5eok.Patterns
{
    public abstract class ScriptableCommand : ScriptableObject, ICommand
    {
        public abstract void Execute(object target);
    }
}
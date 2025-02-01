using UnityEngine;

namespace Jin5eok.Patterns.Command
{
    public abstract class ScriptableCommand : ScriptableObject, ICommand
    {
        public abstract void Execute(object target);
    }
}
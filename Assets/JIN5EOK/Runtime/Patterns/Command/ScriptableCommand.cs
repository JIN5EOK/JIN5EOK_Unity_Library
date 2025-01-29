using UnityEngine;

namespace Jin5eok.Patterns.Commands
{
    public abstract class ScriptableCommand : ScriptableObject, ICommand
    {
        public abstract void Execute(object target);
    }
}
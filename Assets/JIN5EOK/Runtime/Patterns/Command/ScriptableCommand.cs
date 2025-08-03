using UnityEngine;

namespace Jin5eok
{
    public abstract class ScriptableCommand<T> : ScriptableObject, ICommand<T>
    {
        public abstract void Execute(T target);
    }
}
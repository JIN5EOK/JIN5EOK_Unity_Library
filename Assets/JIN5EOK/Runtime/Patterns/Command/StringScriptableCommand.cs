using UnityEngine;

namespace Jin5eok.Patterns
{
    public class StringScriptableCommand : ScriptableCommand
    {
        [SerializeField]
        private StringCommand _stringCommand;
        public override void Execute(object target)
        {
            _stringCommand.Execute(target);
        }
    }
}
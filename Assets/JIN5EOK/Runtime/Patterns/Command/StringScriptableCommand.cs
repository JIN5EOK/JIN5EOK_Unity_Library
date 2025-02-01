using UnityEngine;

namespace Jin5eok.Patterns.Commands
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
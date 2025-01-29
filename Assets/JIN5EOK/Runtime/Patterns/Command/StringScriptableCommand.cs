using UnityEngine;

namespace Jin5eok.Patterns.Commands
{
    public class StringScriptableCommand : ScriptableCommand
    { 
        [SerializeField] private string _commandLine;
        public string CommandLine => _commandLine;
        private StringCommand _stringCommand;
        public override void Execute(object target)
        {
            if (_stringCommand == null || _stringCommand.commandLine.Equals(_commandLine) == false)
            {
                _stringCommand = new StringCommand(_commandLine);
            }
            _stringCommand.Execute(target);
        }
    }
}
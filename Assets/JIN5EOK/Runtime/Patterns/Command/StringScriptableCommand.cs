using System.Collections.Generic;

namespace Jin5eok.Patterns.Commands
{
    public class StringScriptableCommand : ScriptableCommand
    {
        public static Dictionary<string, ICommand> CommandMap { get; private set; } = new();
        public string stringCommand;
        public override void Execute(object target)
        {
            if (CommandMap.TryGetValue(stringCommand, out var command))
            {
                command.Execute(target);
            }
        }
    }
}
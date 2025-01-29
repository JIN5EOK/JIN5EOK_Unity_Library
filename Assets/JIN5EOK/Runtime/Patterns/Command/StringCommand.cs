using System.Collections.Generic;

namespace Jin5eok.Patterns.Commands
{
    public class StringCommand : ICommand
    {
        public static Dictionary<string, ICommand> CommandMap { get; private set; } = new();
        public string commandLine { get; private set;}
        public StringCommand(string commandLine)
        {
            this.commandLine = commandLine;
        }
        public void Execute(object target)
        {
            if (CommandMap.TryGetValue(commandLine, out var command))
            {
                command.Execute(target);
            }
        }
    }
}
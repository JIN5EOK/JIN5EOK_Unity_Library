using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok.Patterns.Command
{
    [Serializable]
    public class StringCommand : ICommand
    {
        public static Dictionary<string, ICommand> CommandMap { get; private set; } = new();
        [SerializeField]
        private string _commandLine;
        public string commandLine
        {
            get => _commandLine; 
            private set => _commandLine = value;
            
        }
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
using System.Collections.Generic;
using Jin5eok.Patterns;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public class InputHandlerUpdater : MonoSingleton<InputHandlerUpdater>
    {
        private readonly List<IInputHandlerBase> _inputHandlers = new();
        
        public void AddInputHandler(IInputHandlerBase handler)
        {
            if (ContainsInputHandler(handler) == false)
            {
                _inputHandlers.Add(handler);    
            }
        }
        
        public void RemoveInputHandler(IInputHandlerBase handler)
        {
            if (ContainsInputHandler(handler) == true)
            {
                _inputHandlers.Remove(handler);    
            }
        }
        
        public bool ContainsInputHandler(IInputHandlerBase handler)
        {
            return _inputHandlers.Contains(handler);
        }
        
        private void Update()
        {
            // Cache a copy to prevent changes during iteration
            var inputHandlers = _inputHandlers.ToArray();

            foreach (var input in inputHandlers)
            {
                input.UpdateState();
            }
        }
    }
}
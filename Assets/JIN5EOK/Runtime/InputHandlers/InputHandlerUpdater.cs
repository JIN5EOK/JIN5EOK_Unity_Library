using System.Collections.Generic;
using Jin5eok.Patterns;
using UnityEngine;

namespace Jin5eok.Inputs
{
    internal class InputHandlerUpdater : MonoSingleton<InputHandlerUpdater>
    {
        private static readonly List<IInputHandlerBase> InputHandlers = new();
        
        private static bool IsNeedInstance => _isInitializedRuntime == true && _isInstantiatedSingleton == false;
        private static bool _isInitializedRuntime = false;
        private static bool _isInstantiatedSingleton = false;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnRuntimeInitialize()
        {
            _isInitializedRuntime = true;
            
            if (InputHandlers.Count != 0)
            {
                EnsureUpdaterInstance();
            }
        }
        
        private static void EnsureUpdaterInstance()
        {
            if (IsNeedInstance == false)
            {
                return;
            }
            
            if (Instance != null) // Create singleton
            {
                _isInstantiatedSingleton = true;
            }
        }
        
        public static void AddInputHandler(IInputHandlerBase handler)
        {
            if (ContainsInputHandler(handler) == false)
            {
                EnsureUpdaterInstance();
                InputHandlers.Add(handler);    
            }
        }
        
        public static void RemoveInputHandler(IInputHandlerBase handler)
        {
            if (ContainsInputHandler(handler) == true)
            {
                InputHandlers.Remove(handler);    
            }
        }
        
        public static bool ContainsInputHandler(IInputHandlerBase handler)
        {
            return InputHandlers.Contains(handler);
        }
        
        private void Update()
        {
            // Cache a copy to prevent changes during iteration
            var inputHandlers = InputHandlers.ToArray();

            foreach (var input in inputHandlers)
            {
                input.UpdateState();
            }
        }
    }
}
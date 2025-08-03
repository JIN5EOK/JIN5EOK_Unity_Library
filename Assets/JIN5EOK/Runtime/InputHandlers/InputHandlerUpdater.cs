using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok
{
    internal class InputHandlerUpdater : MonoSingleton<InputHandlerUpdater>
    {
        private static readonly List<IInputHandlerBase> InputHandlers = new();
        
        private static bool _runtimeInitialized = false;
        private static bool _updaterInstantiated = false;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnRuntimeInitialize()
        {
            _runtimeInitialized = true;
            
            if (InputHandlers.Count != 0)
            {
                EnsureUpdaterInstance();
            }
        }
        
        private static void EnsureUpdaterInstance()
        {
            if (_runtimeInitialized == true && _updaterInstantiated == false)
            {
                var instance = Instance; // Create MonoSingleton
                _updaterInstantiated = true;
            }
        }
        
        public static void Add(IInputHandlerBase handler)
        {
            if (Contains(handler) == false)
            {
                EnsureUpdaterInstance();
                InputHandlers.Add(handler);    
            }
        }
        
        public static void Remove(IInputHandlerBase handler)
        {
            if (Contains(handler) == true)
            {
                InputHandlers.Remove(handler);    
            }
        }
        
        public static bool Contains(IInputHandlerBase handler)
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
using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// InputHandler를 Update 주기에 맞춰 자동으로 업데이트 해줍니다.
    /// InputHandlerUpdater는 DontDestroyOnLoad 싱글턴 게임오브젝트로 InputHandler를 생성하면 씬에 자동 생성됩니다.
    /// InputHandlerUpdater는 internal class로 선언되었으며 사용자는 이에 접근할 필요 없습니다.
    /// 각 InputHandler 생성시 InputHandlerUpdater의 컬렉션에 등록되어 자동으로 업데이트가 수행됩니다.
    /// InputHandler의 Dispose 호출시 컬렉션에서 해제됩니다.
    /// </summary>
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
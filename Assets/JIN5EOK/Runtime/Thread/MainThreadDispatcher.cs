using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace Jin5eok
{
    public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>
    {
        private static int _mainThreadId;
        
        private static readonly ConcurrentQueue<Action> ExecutionQueue = new ();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Create Instance
            _ = Instance;
            
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static bool IsMainThread()
        {
            return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }
        
        public static void RunOrEnqueue(Action action)
        {
            if (action == null)
            {
                return;
            }
        
            // When run on Main Thread : Execute immediately  
            if (IsMainThread() == true)
            {
                action?.Invoke();
            }
            else
            {
                ExecutionQueue.Enqueue(action);
            }
        }

        private void Update()
        {
            while (ExecutionQueue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }
}
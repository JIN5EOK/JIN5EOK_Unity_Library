using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Jin5eok
{
    public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>
    {
        private static readonly ConcurrentQueue<Action> ExecutionQueue = new ();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Create Instance
            _ = Instance;
        }
        
        public static void Enqueue(Action action)
        {
            if (action == null)
            {
                return;
            }
            ExecutionQueue.Enqueue(action);
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
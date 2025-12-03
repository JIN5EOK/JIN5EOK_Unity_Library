using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 메인 스레드에서 액션을 실행하기 위한 디스패처입니다.
    /// 다른 스레드에서 호출된 액션을 큐에 넣고, 메인 스레드의 Update에서 실행합니다.
    /// 메인 스레드에서 호출된 경우 즉시 실행합니다.
    /// </summary>
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

        /// <summary>
        /// 현재 스레드가 메인 스레드인지 확인합니다.
        /// </summary>
        /// <returns>메인 스레드이면 true, 그렇지 않으면 false</returns>
        public static bool IsMainThread()
        {
            return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }
        
        /// <summary>
        /// 액션을 실행합니다. 메인 스레드에서 호출된 경우 즉시 실행하고, 그렇지 않으면 큐에 넣어 메인 스레드에서 실행합니다.
        /// </summary>
        /// <param name="action">실행할 액션</param>
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
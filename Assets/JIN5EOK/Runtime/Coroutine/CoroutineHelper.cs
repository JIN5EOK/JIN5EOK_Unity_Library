using System;
using System.Collections;
using UnityEngine;

namespace Jin5eok
{
    public class CoroutineHelper
    {
        private static MonoBehaviour GlobalCoroutineHost => CoroutineRunner.Instance;
        
        public static Coroutine Delay(float seconds, Action onComplete, MonoBehaviour coroutineHost = null)
        {
            coroutineHost = coroutineHost == null ? GlobalCoroutineHost : coroutineHost;
            
            return coroutineHost.StartCoroutine(DelayRoutine(seconds, onComplete));
            
            IEnumerator DelayRoutine(float seconds, Action onComplete)
            {
                yield return new WaitForSeconds(seconds);
                onComplete?.Invoke();
            }
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace Jin5eok
{
    public class CoroutineHelper
    {
        private static MonoBehaviour GlobalCoroutineHost => CoroutineRunner.Instance;

        private static MonoBehaviour GetValidCoroutineHost(MonoBehaviour coroutineHost)
        {
            return coroutineHost ?? GlobalCoroutineHost;
        }
        
        public static Coroutine Delay(float seconds, Action onComplete, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(DelayRoutine(seconds, onComplete, false));   
        }
        
        public static Coroutine DelayRealtime(float seconds, Action onComplete, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(DelayRoutine(seconds, onComplete, true));
        }
        
        private static IEnumerator DelayRoutine(float seconds, Action onComplete, bool isRealtime)
        {
            yield return isRealtime == true ? new WaitForSecondsRealtime(seconds) : new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
    }
}
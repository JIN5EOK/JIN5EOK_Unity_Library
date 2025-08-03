using System;
using System.Collections;
using System.Collections.Generic;
using Jin5eok.Patterns;
using UnityEngine;

namespace Jin5eok
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        private static MonoBehaviour GetValidCoroutineHost(MonoBehaviour coroutineHost)
        {
            return coroutineHost ?? Instance;
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
        
        public static Coroutine Sequence(IEnumerable<IEnumerator> coroutines, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(SequenceRoutine(coroutines, coroutineHost));
        }

        private static IEnumerator SequenceRoutine(IEnumerable<IEnumerator> coroutines, MonoBehaviour coroutineHost)
        {
            foreach (var coroutine in coroutines)
            {
                yield return coroutineHost.StartCoroutine(coroutine);
            }
        }

        public static Coroutine WaitUntil(Func<bool> waitCondition, Action callBack, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(WaitUntilOrWhileRoutine(waitCondition, callBack, false));
        }

        public static Coroutine WaitWhile(Func<bool> waitCondition, Action callBack, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(WaitUntilOrWhileRoutine(waitCondition, callBack, true));
        }
        
        private static IEnumerator WaitUntilOrWhileRoutine(Func<bool> waitCondition, Action callBack, bool isWhile)
        {
            yield return isWhile == true ? new WaitWhile(waitCondition) : new WaitUntil(waitCondition);
            callBack?.Invoke();
        }
    }
}
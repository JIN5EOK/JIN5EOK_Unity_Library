using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 코루틴을 관리하는 매니저 클래스입니다.
    /// MonoBehaviour가 없는 정적 클래스에서도 코루틴을 사용할 수 있도록 합니다.
    /// </summary>
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        private static MonoBehaviour GetValidCoroutineHost(MonoBehaviour coroutineHost)
        {
            return coroutineHost ?? Instance;
        }
        
        /// <summary>
        /// 지정된 시간 후에 콜백을 실행하는 코루틴을 시작합니다. (게임 시간 기준)
        /// </summary>
        /// <param name="seconds">대기할 시간(초)</param>
        /// <param name="onComplete">완료 시 호출될 콜백</param>
        /// <param name="coroutineHost">코루틴을 실행할 MonoBehaviour, null이면 CoroutineManager 인스턴스 사용</param>
        /// <returns>시작된 코루틴</returns>
        public static Coroutine Delay(float seconds, Action onComplete, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(DelayRoutine(seconds, onComplete, false));   
        }
        
        /// <summary>
        /// 지정된 시간 후에 콜백을 실행하는 코루틴을 시작합니다. (실제 시간 기준)
        /// </summary>
        /// <param name="seconds">대기할 시간(초)</param>
        /// <param name="onComplete">완료 시 호출될 콜백</param>
        /// <param name="coroutineHost">코루틴을 실행할 MonoBehaviour, null이면 CoroutineManager 인스턴스 사용</param>
        /// <returns>시작된 코루틴</returns>
        public static Coroutine DelayRealtime(float seconds, Action onComplete, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(DelayRoutine(seconds, onComplete, true));
        }
        
        private static IEnumerator DelayRoutine(float seconds, Action onComplete, bool isRealtime)
        {
            yield return isRealtime == true ? new WaitForSecondsRealtime(seconds) : new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
        
        /// <summary>
        /// 여러 코루틴을 순차적으로 실행하는 코루틴을 시작합니다.
        /// </summary>
        /// <param name="coroutines">순차적으로 실행할 코루틴들의 열거</param>
        /// <param name="coroutineHost">코루틴을 실행할 MonoBehaviour, null이면 CoroutineManager 인스턴스 사용</param>
        /// <returns>시작된 코루틴</returns>
        public static Coroutine Sequence(IEnumerable<IEnumerator> coroutines, MonoBehaviour coroutineHost = null)
        {
            var host = GetValidCoroutineHost(coroutineHost);
            return host.StartCoroutine(SequenceRoutine(coroutines, host));
        }

        private static IEnumerator SequenceRoutine(IEnumerable<IEnumerator> coroutines, MonoBehaviour coroutineHost)
        {
            foreach (var coroutine in coroutines)
            {
                yield return coroutineHost.StartCoroutine(coroutine);
            }
        }

        /// <summary>
        /// 조건이 true가 될 때까지 대기한 후 콜백을 실행하는 코루틴을 시작합니다.
        /// </summary>
        /// <param name="waitCondition">대기할 조건</param>
        /// <param name="callBack">조건이 만족되면 호출될 콜백</param>
        /// <param name="coroutineHost">코루틴을 실행할 MonoBehaviour, null이면 CoroutineManager 인스턴스 사용</param>
        /// <returns>시작된 코루틴</returns>
        public static Coroutine WaitUntil(Func<bool> waitCondition, Action callBack, MonoBehaviour coroutineHost = null)
        {
            return GetValidCoroutineHost(coroutineHost).StartCoroutine(WaitUntilOrWhileRoutine(waitCondition, callBack, false));
        }

        /// <summary>
        /// 조건이 false가 될 때까지 대기한 후 콜백을 실행하는 코루틴을 시작합니다.
        /// </summary>
        /// <param name="waitCondition">대기할 조건</param>
        /// <param name="callBack">조건이 만족되면 호출될 콜백</param>
        /// <param name="coroutineHost">코루틴을 실행할 MonoBehaviour, null이면 CoroutineManager 인스턴스 사용</param>
        /// <returns>시작된 코루틴</returns>
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
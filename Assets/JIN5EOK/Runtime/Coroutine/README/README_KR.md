# CoroutineManager 사용자 가이드

* MonoBehaviour가 없는 정적 클래스에서도 코루틴을 사용할 수 있도록 해줍니다.
* 코루틴에서 자주 쓰이는 간단한 패턴에 대한 편의 함수들을 제공합니다.
  * 지연 실행 (Delay, DelayRealtime)
  * 조건 대기 (WaitUntil, WaitWhile)
  * 코루틴 순차 실행 (Sequence)


# 정적 클래스에서 코루틴 실행 예시

```csharp
public static class GameUtils
{
    public static IEnumerator PrintMessagesRoutine()
    {
        Debug.Log("2초 대기");
        yield return new WaitForSeconds(2f);
        Debug.Log("메시지 출력");
    }

    public static void RunPrintMessages()
    {
        CoroutineManager.RunCoroutine(PrintMessagesRoutine());
    }
}
```

# 코루틴 편의 기능 함수

## 지연 실행
일정시간 대기한 후 지정한 콜백 함수를 실행합니다.
```csharp
// 게임 시간 기준 (Time.timeScale 영향)
CoroutineManager.Delay(3f, () => Debug.Log("3초 후 실행"));

// 실제 시간 기준 (일시정지 중에도 동작)
CoroutineManager.DelayRealtime(5f, () => Debug.Log("5초 후 실행"));
```

## 조건 대기
지정한 조건이 true가 될 때까지 대기한 후 지정한 명령을 실행합니다.
```csharp
// 조건이 true가 될 때까지 대기
CoroutineManager.WaitUntil(() => _isReady, () => Debug.Log("준비 완료"));

// 조건이 false가 될 때까지 대기
CoroutineManager.WaitWhile(() => _isLoading, () => Debug.Log("로딩 완료"));
```

## 코루틴 순차 실행
코루틴 여러개를 묶어 순차적으로 실행합니다.
```csharp
using System.Collections;

var coroutines = new IEnumerator[] { Coroutine1(), Coroutine2(), Coroutine3() };
CoroutineManager.Sequence(coroutines);
```

# coroutineHost 파라미터

- 모든 메서드는 `coroutineHost`(MonoBehaviour) 파라미터를 받습니다
- null이거나 비워두면 CoroutineManager 싱글턴 객체를 매개로 삼아 실행합니다.
- 코루틴 매니저의 기능들을 사용하면서 게임오브젝트와 코루틴의 생명주기를 동기화 하고 싶을때 사용하세요
```csharp
public class CustomHostExample : MonoBehaviour
{
    private void Start()
    {
        // 이 MonoBehaviour에서 코루틴 실행
        CoroutineManager.Delay(2f, () => 
        {
            Debug.Log("CustomHostExample 게임오브젝트에서 실행됨");
        }, this);
    }
    
    private void OnDestroy()
    {
        // GameObject가 파괴되면 실행 중인 코루틴도 자동으로 중단됨
    }
}
```

```csharp
// coroutineHost를 지정하지 않으면 CoroutineManager 인스턴스 사용
CoroutineManager.Delay(5f, () => Debug.Log("CoroutineManager에서 실행"));
```
# MainThreadDispatcher 사용자 가이드

* 다른 스레드에서 실행되는 명령을 메인 스레드에서 실행하도록 도와주는 디스패처입니다.
* 백그라운드 스레드에서 Unity API를 호출해야 할 때 사용합니다.
* 단 현 시점에서는 UniTask같은 플러그인이나 Awaitable를 사용하면 거의 필요가 없으니 그쪽을 사용해보세요.

## 기본 사용법

```csharp
using Jin5eok;
using System.Threading.Tasks;
using UnityEngine;

await Task.Run(() =>
{
    // 무거운 작업, 비동기 함수 기타 등등..
    var response = await httpClient.GetStringAsync("https://example.com/data");
    // Unity API를 여기서 실행하면 에러 발생하므로 메인스레드 디스패처 사용
    MainThreadDispatcher.RunOrEnqueue(() =>
    {
        transform.position = Vector3.up;
        Debug.Log("메인 스레드에서 실행");
    });
});
```

## 주의사항

⚠️ **명령 실행 시점**
- 디스패처에 삽입한 명령들은 Update문에서 큐의 내용을 꺼내가며 순차 실행됩니다. 따라서 현재 태스크의 흐름과 독립된 태스크가 됩니다.
- 이를 고려해서 기능을 작성해주세요.

```csharp
using Jin5eok;
using System.Threading.Tasks;
using UnityEngine;

await Task.Run(() =>
{
    // 무거운 작업, 비동기 함수 기타 등등..
    var response = await httpClient.GetStringAsync("https://api.example.com/data");
    // Unity API를 여기서 실행하면 에러 발생하므로 메인스레드 디스패처 사용
    MainThreadDispatcher.RunOrEnqueue(() =>
    {
        transform.position = Vector3.up;
        Debug.Log("2번째로 출력");
    });
    Debug.Log("1번째로 출력");
});
```
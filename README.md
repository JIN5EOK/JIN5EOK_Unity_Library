# JIN5EOK_Unity_Library
> 유니티 게임 개발을 위한 라이브러리입니다.

이 리포지토리는 실제로 실행가능한 유니티 프로젝트이며 개발된 기능 코드, 샘플은 모두 아래 경로에 존재합니다.

[Assets/JIN5EOK/](Assets/JIN5EOK/)

## 💾 설치 방법

1. 유니티 -> `Windows` -> `Package Manager` -> + 클릭 -> `Install package from git url`
2. `https://github.com/JIN5EOK/JIN5EOK_Unity_Library.git?path=Assets/JIN5EOK#main` 을 입력합니다.

* 혹은 단순히 샘플 프로젝트를 실행하려면 프로젝트를 Clone하거나 다운로드 받으세요.

## 🧑‍🏫 사용자 가이드

> 각 기능의 사용 예제, 주의사항들이 적힌 문서입니다.
> 아직 문서가 준비되지 않은 기능들도 있습니다.

---
### AddressableScope
* 어드레서블로 로드한 에셋 / 프리펩 인스턴스의 생명주기 관리를 용이하게 하기 위한 스코프 클래스입니다.
* 스코프를 통해 에셋을 로드할 경우 핸들들이 스코프 내부에 캐싱되며 스코프가 Dispose될 때 포함된 에셋들을 모두 Release처리합니다.
    * 에셋의 경우 : 같은 주소 혹은 에셋 레퍼런스당 하나의 핸들만 캐싱하며 캐싱된 상태에서 요청시 이전에 반환된 핸들을 반환합니다
    * 프리펩 인스턴스의 경우 : 인스턴스와 핸들은 1:1 관계이므로 인스턴스 생성시마다 새로운 핸들이 생성되어 반환됩니다.
* Dispose 호출을 누락한채 스코프에 대한 참조를 잃어버렸을 경우 이후 소멸자에서 Dispose를 호출합니다.
    * 하지만 GC의 소멸자 호출 시점은 명확하지 않으며 그에 따른 갑작스런 성능저하가 발생할 수 있으므로 권장하지 않습니다.

[📑 사용자 가이드](Assets/JIN5EOK/Runtime/AddressablesScope/README/README_KR.md)

[📁 디렉토리 위치](Assets/JIN5EOK/Runtime/AddressablesScope)

---
### AudioPlayer
* 오디오 소스를 래핑하는 오디오 플레이어 클래스입니다.
* 오디오 재생 간편화
  * 정적 함수로 OneShot 오디오를 재생하거나 `AudioSource`를 래핑한 객체 `AudioPlayer`를 생성할 수 있습니다.
* 오디오 재생 결과 반환
  * 오디오 재생을 마치고 오디오 재생의 성공,실패,중단 여부를 반환합니다.

[📑 사용자 가이드](Assets/JIN5EOK/Runtime/AudioPlayer/README/README_KR.md)

[📁 디렉토리 위치](Assets/JIN5EOK/Runtime/AudioPlayer)

---
### InputHandlers
* Keycode와 OldInputSystem을 래핑하여 입력 기능 구현을 돕는 클래스입니다. 할당 키와 조합을 런타임 도중 편집이 가능합니다.
* 이벤트 기반으로 키 입력을 수행할 수 있습니다.
* 한가지 액션에 여러가지 키를 할당할 수 있습니다.
* 키 리바인딩을 지원합니다.
* 일반적인 키보드 마우스, 게임패드 입력 처리와 키 리바인딩 처리는 가능하지만 이 이상의 기능이 필요하다면 더 많은 입력기기와 기능을 지원하는 유니티의 공식 패키지 `Input System`을 사용해보세요.

[📑 사용자 가이드](Assets/JIN5EOK/Runtime/InputHandlers/README/README_KR.md)

[📁 디렉토리 위치](Assets/JIN5EOK/Runtime/InputHandlers)

---
### CoroutineManager
* MonoBehaviour가 없는 정적 클래스에서도 코루틴을 사용할 수 있도록 해줍니다.
* 코루틴에서 자주 쓰이는 간단한 패턴에 대한 편의 함수들을 제공합니다.
  * 지연 실행 (Delay, DelayRealtime)
  * 조건 대기 (WaitUntil, WaitWhile)
  * 코루틴 순차 실행 (Sequence)

[📑 사용자 가이드](Assets/JIN5EOK/Runtime/Coroutine/README/README_KR.md)

[📁 디렉토리 위치](Assets/JIN5EOK/Runtime/Coroutine)

---
### MainThreadDispatcher
* 다른 스레드에서 실행되는 명령을 메인 스레드에서 실행하도록 도와주는 디스패처입니다.
* 백그라운드 스레드에서 Unity API를 호출해야 할 때 사용합니다.
* 단 현 시점에서는 UniTask같은 플러그인이나 Awaitable를 사용하면 거의 필요가 없으니 그쪽을 사용해보세요.

[📑 사용자 가이드](Assets/JIN5EOK/Runtime/Thread/README/README_KR.md)

[📁 디렉토리 위치](Assets/JIN5EOK/Runtime/Thread)
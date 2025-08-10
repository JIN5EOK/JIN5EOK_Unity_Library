# AddressablesManager
* 어드레서블 에셋 및 인스턴스 로드, 생성, 해제를 도와주는 매니지먼트 클래스들입니다, 아래 방식으로 로드를 지원합니다.
* 로드 방식
  * WaitForCompletion
  * Coroutine
  * Task
  * UniTask
  * Awaitable
* 멀티스레드 환경에서 안전하게 사용할 수 있도록 설계되었습니다, 단 WaitForCompletion류 함수는 예외입니다. 이유는 아래 주의사항을 참고해주세요.

## Addressables
### [AddressablesManager](AddressablesManager_KR.md)

* 어드레서블 에셋 로드 및 릴리즈를 도와주는 매니저 클래스입니다.
* WaitForCompletion, Task, UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* 이 클래스를 통해 에셋을 로드할 경우
    * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.
    * 처음 로드시 내부 컬렉션에 핸들을 캐싱하여 추후 로드시에도 하나의 핸들만 반환합니다.

### [AddressablesInstanceManager](AddressablesInstanceManager_KR.md)

* 어드레서블을 통한 프리펩 인스턴스 생성 및 릴리즈를 도와주는 매니저 클래스입니다.
* WaitForCompletion, Task UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* 이 클래스를 통해 프리펩을 생성할 경우
    * 게임 오브젝트가 파괴될 때 자동으로 핸들을 릴리즈하는 컴포넌트를 부착합니다.
    * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.

## 주의사항

### `WaitForCompletion`류 로드 함수는 메인스레드 실행이 보장되는 상황에만 호출하세요!
* `WaitForCompletion`은 에셋을 불러오는 동안 메인스레드를 멈추고 로드를 기다리는 특성이 있습니다.
* 따라서 `WaitForCompletion`을 멀티 스레딩 환경에서도 안전하게 만들면 `WaitForCompletion`의 일반적인 사용 의도 (에셋 로드가 끝날때 까지 메인스레드와 코드 흐름을 멈추기)를 벗어난다고 생각하여 멀티스레드 실행 대비 처리를 하지 않았습니다.
* 다른 로드 함수들은 멀티스레딩 환경에서 실행해도 안전하게 처리되어있으니 다른 함수를 사용해주세요.
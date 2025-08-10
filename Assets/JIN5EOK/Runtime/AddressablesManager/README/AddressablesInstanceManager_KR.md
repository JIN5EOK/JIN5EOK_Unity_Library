# AddressablesInstanceManager
## 개요
* 어드레서블을 통한 프리펩 인스턴스 생성 및 릴리즈를 도와주는 매니저 클래스입니다.
* WaitForCompletion, Coroutine, UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* WaitForCompletion을 제외하면 메인스레드 외에서도 안전하게 실행되도록 설계되었습니다.
* 이 클래스를 통해 프리펩을 생성할 경우
  * 게임 오브젝트가 파괴될 때 자동으로 핸들을 릴리즈하는 컴포넌트를 부착합니다.
  * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.

## 함수

### 핸들 로드
> public static AsyncOperationHandle\<GameObject> LoadHandle(string address)

* 인스턴스 생성을 실행한 뒤 핸들을 캐싱 후 반환합니다.
* 이후 호출시 이전 핸들이 해제 상태가 되기 전까진 캐싱된 핸들을 반환합니다.

### 인스턴스 생성
* 내부적으로 LoadHandle를 호출하고 에셋 로드가 완료될 때 까지 대기, 이후 로드된 에셋을 반환합니다.
* 핸들을 반환하지 않으므로 핸들을 얻고 싶다면 동일 주소로 LoadHandle을 호출하세요.

> public static GameObject InstantiateWaitForCompletion(string address)

* 프리펩을 동기적으로 인스턴스화 하여 반환합니다
* 내부적으로 WaitForCompletion를 호출하므로 첫 호출에 한해 메인스레드가 블로킹 될 수 있습니다.
* 반드시 메인스레드 환경에서만 호출해주세요!
  * `WaitForCompletion`은 에셋을 불러오는 동안 메인스레드를 멈추고 로드를 기다리는 특성이 있습니다.
  * 따라서 `WaitForCompletion`을 멀티 스레딩 환경에서도 안전하게 만들면 `WaitForCompletion`의 일반적인 사용 의도 (에셋 로드가 끝날때 까지 메인스레드와 코드 흐름을 멈추기)를 벗어난다고 생각하여 멀티스레드 실행 대비 처리를 하지 않았습니다.

> public static void InstantiateCoroutine(string address, Action\<GameObject> onResult)

* 프리펩을 Coroutine으로 인스턴스화, 콜백을 통해 인스턴스화 된 프리펩을 전달합니다.
* 멀티스레드 상에서의 호출에 안전할 수 있게 하기 위해 내부적으로 `MainThreadDispathcer`에 작업을 위임하여 처리합니다.

> public static async Task\<GameObject> InstantiateAsync(string address)

* 프리펩을 Task를 통해 인스턴스화하여 반환합니다.
* 멀티스레드 상에서의 호출에 안전할 수 있게 하기 위해 내부적으로 `MainThreadDispathcer`에 작업을 위임하여 처리합니다.

> public static async UniTask\<GameObject> InstantiateAsyncUniTask(string address)

* 프리펩을 UniTask를 통해 인스턴스화하여 반환합니다.
* Task를 메인스레드로 전환 후 에셋 로드를 대기하는 처리가 있습니다.

> public static async Awaitable<GameObject> InstantiateAsyncAwaitable(string address)

* 프리펩을 Awaitable을 통해 인스턴스화하여 반환합니다.
* Task를 메인스레드로 전환 후 에셋 로드를 대기하는 처리가 있습니다.

### 핸들 릴리즈

> public static void ReleaseInstances(string address)

* 지정한 주소의 핸들을 릴리즈합니다.

> public static void ReleaseInstanceHandleAll()

* 생성된 모든 인스턴스의 핸들들을 전부 릴리즈합니다.

### 기타

> public static List<AsyncOperationHandle\<GameObject>> GetLoadedHandleAll(string address)

* 해당 주소로 로드된 인스턴스의 핸들 컬렉션을 반환합니다.
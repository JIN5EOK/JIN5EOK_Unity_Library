# AddressablesManager
## 개요
* 어드레서블을 통한 프리펩 인스턴스 생성 및 릴리즈를 도와주는 매니저 클래스입니다.
* UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
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

> public static void InstantiateAsyncCoroutine(string address, Action\<GameObject> onResult)

* 프리펩을 Coroutine으로 인스턴스화, 콜백을 통해 인스턴스화 된 프리펩을 전달합니다.
* 처음 사용시 Coroutine 실행을 위한 DontDestroyOnLoad 상태의 싱글톤 게임오브젝트가 생성됩니다.

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
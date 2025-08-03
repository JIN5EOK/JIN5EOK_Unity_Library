# AddressablesManager
## 개요
* 어드레서블 에셋 로드 및 릴리즈를 도와주는 매니저 클래스입니다.
* UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* 이 클래스를 통해 에셋을 로드할 경우
  * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.
  * 처음 로드시 내부 컬렉션에 핸들을 캐싱하여 추후 로드시에도 하나의 핸들만 반환합니다.

## 함수

### 핸들 로드
> public static AsyncOperationHandle\<T> LoadHandle\<T>(string address) where T : Object

* 에셋 로드를 호출한 뒤 핸들을 캐싱 후 반환합니다.
* 이후 호출시 이전 핸들이 해제 상태가 되기 전까진 캐싱된 핸들을 반환합니다.

### 에셋 로드
* 내부적으로 LoadHandle을 호출하고 에셋 로드가 완료될 때 까지 대기, 이후 로드된 에셋을 반환합니다.
* 핸들을 반환하지 않으므로 핸들을 얻고 싶다면 동일 주소로 LoadHandle을 호출하세요.

> public static T LoadWaitForCompletion\<T>(string address) where T : Object

* 에셋을 동기적으로 로드하여 반환합니다
* 내부적으로 WaitForCompletion를 호출하므로 첫 호출에 한해 메인스레드가 블로킹 될 수 있습니다.

> public static void LoadAssetCoroutine\<T>(string address, Action\<T> onResult) where T : Object

* 에셋을 Coroutine으로 로드, 콜백을 통해 로드된 에셋을 전달합니다.
* 처음 사용시 Coroutine 실행을 위한 DontDestroyOnLoad 상태의 싱글톤 게임오브젝트가 생성됩니다.

> public static async UniTask\<T> LoadAssetUniTask\<T>(string address) where T : Object

* 에셋을 UniTask를 통해 로드하여 반환합니다.
* Task를 메인스레드로 전환 후 에셋 로드를 대기하는 처리가 있습니다.

> public static async Awaitable\<T> LoadAssetAwaitable\<T>(string address) where T : Object

* 에셋을 Awaitable을 통해 로드하여 반환합니다.
* Task를 메인스레드로 전환 후 에셋 로드를 대기하는 처리가 있습니다.

### 핸들 릴리즈

> public static bool Release\<T>(string address) where T : Object

* 지정한 타입과 주소의 핸들을 릴리즈합니다.

> public static void ReleaseAll\<T>() where T : Object

* 지정한 타입의 핸들들을 전부 릴리즈합니다.

### 기타

> public static bool IsLoaded\<T> (string address) where T : Object

* 해당 타입과 주소의 핸들이 로드 되었는지 여부를 반환합니다.

> public static List<AsyncOperationHandle\<T>> GetLoadedHandleAll\<T>() where T : Object

* 지정한 타입과 동일한, 현재 로드 된 핸들 컬렉션을 반환합니다.

> public static List\<string> GetLoadedAddressAll\<T>() where T : Object

* 지정한 타입과 동일한, 현재 로드 된 핸들 주소 컬렉션을 반환합니다.

## 주의사항
* 동일한 에셋을 다른 타입으로 로드를 시도할 경우 각 타입별 핸들이 생성됩니다.
  * 예) Sprite 에셋을 Sprite타입으로 로드, Texture2D 타입으로 로드 할 경우 각 타입에 대한 핸들이 생성됩니다.
# ResourcesManagements
* 동적 리로스 로드와 해제를 도와주는 매니지먼트 클래스들입니다.

## Addressables
### [AddressablesManager](AddressablesManager_KR.md)

* 어드레서블 에셋 로드 및 릴리즈를 도와주는 매니저 클래스입니다.
* UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* 이 클래스를 통해 에셋을 로드할 경우
    * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.
    * 처음 로드시 내부 컬렉션에 핸들을 캐싱하여 추후 로드시에도 하나의 핸들만 반환합니다.

### [AddressablesInstanceManager](AddressablesInstanceManager_KR.md)

* 어드레서블을 통한 프리펩 인스턴스 생성 및 릴리즈를 도와주는 매니저 클래스입니다.
* UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* 이 클래스를 통해 프리펩을 생성할 경우
    * 게임 오브젝트가 파괴될 때 자동으로 핸들을 릴리즈하는 컴포넌트를 부착합니다.
    * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.
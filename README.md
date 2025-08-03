# JIN5EOK_Framework
> 유니티 게임 개발을 위한 프레임워크입니다.

## 설치 방법

1. 유니티 -> `Windows` -> `Package Manager` -> + 클릭 -> `Install package from git url`
2. `https://github.com/JIN5EOK/JIN5EOK_Unity_Framework.git?path=Assets/JIN5EOK#main` 을 입력합니다.

* 혹은 단순히 샘플 프로젝트를 실행하려면 프로젝트를 Clone하거나 다운로드 받으세요.

## 기능 문서
> 각 기능별 함수나 기능, 주의사항들이 적힌 문서입니다.
> 아직 문서가 준비되지 않은 기능들도 있습니다.

### [AddressablesManager](Assets/JIN5EOK/Runtime/AddressablesManager/README/README_KR.md)
* 어드레서블 에셋 로드 및 릴리즈를 도와주는 매니저 클래스입니다.
* UniTask, Awaiatable, Coroutine등 다양한 방식으로 로드할 수 있습니다.
* 이 클래스를 통해 에셋을 로드할 경우
  * 주소와 핸들을 내부에 캐시하고 관리할 수 있습니다.
  * 처음 로드시 내부 컬렉션에 핸들을 캐싱하여 추후 로드시에도 하나의 핸들만 반환합니다.

### [AudioPlayer](Assets/JIN5EOK/Runtime/AudioPlayer/README/README_KR.md)
* 오디오 소스를 래핑하는 오디오 플레이어 클래스입니다.
* 오디오 재생 간편화
  * 정적 함수로 OneShot 오디오를 재생하거나 `AudioSource`를 래핑한 객체 `AudioPlayer`를 생성할 수 있습니다.
* 오디오 재생 결과 콜백
  * 오디오 재생의 성공,실패,중단 여부를 판단하고 그와 관련한 콜백을 등록할 수 있습니다.

### [InputHandler](Assets/JIN5EOK/Runtime/InputHandlers/README/README_KR.md)
* Keycode와 OldInputSystem을 랩핑하여 이벤트 기반으로 키 입력을 수행하거나 키 리바인딩을 도와주는 클래스들입니다.
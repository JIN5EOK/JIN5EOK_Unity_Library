# 개요
* 오디오 재생을 도와주는 매니저 클래스입니다.
### 특징
* 오디오 재생 간편화
  * OneShot 형태로 오디오를 재생하거나 `AudioSource`를 가진 객체 `AudioPlayer`를 생성할 수 있습니다.
* 오디오 관련 설정 간편화
  * Volume, Pitch 등 오디오 관련 설정을 조작하고 음악과 효과음 등 오디오 카테고리별로 설정값을 다르게 적용할 수 있습니다.

## [AudioManager](AudioManager_KR.md)
* 오디오 매니저 시스템에서 메인이 되는 매니저 클래스입니다.
* 다음과 같은 기능을 수행합니다
  * 오디오 타입 추가, 가져오기
  * 오디오 플레이어 생성
  * OneShot 오디오 재생

## [AudioModel](AudioModel_KR.md)
* 특정 오디오 타입에 대한 볼륨, 피치, 음소거 여부를 조작할 수 있는 모델 클래스입니다.

## [AudioPlayer](InputHandlers_KR.md)
* 독립된 오디오 소스를 가진 오디오 재생 컴포넌트입니다.
* 오디오를 일시정지 하거나 멈추는 등 오디오의 재생을 조작해야 할 필요가 있을 때 사용합니다.

## [OneShotAudioPlayer](OneShotAudioPlayer_KR.md)
* OneShot 재생을 할 때 사용되는 `AudioPlayer`입니다, 오디오 타입당 하나만 존재합니다.
* `AudioManager`의 인터페이스를 통해 자동으로 생성하고 다룰 수 있으므로 사용자는 다루지 않아도 됩니다.

## [AudioPlayResult](AudioPlayResult_KR.md)
* 오디오 재생 후 반환되는 관련 정보 구조체입니다.

### 주의사항
* 오디오 모델을 추가할 때 "Global" 타입은 모든 오디모 모델들에 대한 전역적 모델로 미리 선언되었으므로 사용할 수 없습니다. 
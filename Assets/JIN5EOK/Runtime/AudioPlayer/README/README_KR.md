# 개요
* 오디오 소스를 래핑하는 오디오 플레이어 클래스입니다.

### 특징
* 오디오 재생 간편화
  * 정적 함수로 OneShot 오디오를 재생하거나 `AudioSource`를 래핑한 객체 `AudioPlayer`를 생성할 수 있습니다.
* 오디오 재생 결과 콜백
  * 오디오 재생의 성공,실패,중단 여부를 판단하고 그와 관련한 콜백을 등록할 수 있습니다.

## AudioPlayer
* 오디오 소스를 래핑하는 오디오 재생 컴포넌트입니다.

### ```public static AudioPlayer Create(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null, Transform parent = null)```
* 새로운 `AudioPlayer` GameObject를 생성하여 반환합니다.

### ```public static void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null, Action<PlayResult> onPlayFinished = null)```
* OneShot 형태의 오디오를 재생합니다
* 플레이 상태를 추적하기 위해 내부적으로 AudioPlayer 오브젝트 풀에서 AudioPlayer를 가져와 PlayWithCallback을 호출하는 방식으로 동작합니다.
* 플레이 종료시 결과를 반환하는 콜백함수를 등록할 수 있습니다.

### ```public void Play(Action<PlayResult> onPlayFinished = null)```
* 오디오를 재생하며 동시에 플레이 종료시 결과를 반환하는 콜백함수를 등록할 수 있습니다.

### ```public AudioSource AudioSource { get; private set; }```
* 오디오 소스를 조작해야 할 때 사용합니다.

## AudioPlayerPool
* `AudioPlayer`의 오브젝트 풀
* 생성 로직으로 `AudioPlayer`의 Create 함수를 사용합니다.
* `AudioPlayer`의 PlayOneShot에서 사용하기 위해 만들었으나 필요한 경우 사용해도 무방합니다.

### ```public IObjectPool<AudioPlayer> Pool { get; private set; }```
* 오브젝트 풀
# 개요
* 오디오 소스를 래핑하는 오디오 플레이어 클래스와 그를 돕는 클래스들입니다.

### 특징
* 오디오 재생 간편화
  * OneShot 형태로 오디오를 재생하거나 `AudioSource`를 래핑한 객체 `AudioPlayer`를 생성할 수 있습니다.
* 오디오 재생 결과 콜백
  * 오디오 재생의 성공,실패,중단 여부를 판단하고 그와 관련한 콜백을 등록할 수 있습니다.

## AudioPlayer
* 오디오 소스를 래핑하는 오디오 재생 컴포넌트입니다.

### ```public static AudioPlayer Create(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null, Transform parent = null)```
* 정적함수로 새로운 `AudioPlayer` GameObject를 생성하여 반환합니다.

### ```public static void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup, Action<PlayResult> onPlayFinished = null)```
* 정적함수로 OneShot 형태로 오디오를 재생합니다, 내부적으로 DontDestroyOnLoad 상태의 AudioPlayer를 생성하며 같은 AudioMixer끼리 같은 AudioPlayer를 공유합니다.
* 플레이 종료시 결과를 반환하는 콜백함수를 등록할 수 있습니다.

### ```public void PlayWithCallback(Action<PlayResult> onPlayFinished = null)```
* 오디오를 재생하며 동시에 플레이 종료시 결과를 반환하는 콜백함수를 등록할 수 있습니다.

### ```public AudioSource AudioSource { get; private set; }```
* 오디오 소스를 조작해야 할 때 사용합니다.
  
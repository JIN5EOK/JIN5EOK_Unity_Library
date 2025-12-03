# AudioPlayer 튜토리얼

AudioPlayer는 오디오 재생을 간편하게 하고, 재생 결과를 추적할 수 있게 해주는 클래스입니다.

## OneShot 오디오 재생
OneShot 오디오를 재생할때 사용하는 정적 함수입니다.

내부적으로 오디오 재생만을 위한 AudioPlayerPool(Object Pool)을 이용해 AudioPlayer를 생성해두고 사용합니다.
```csharp
using Jin5eok;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _jumpSound;
    
    public void PlayJumpSound()
    {
        // OneShot으로 즉시 재생
        AudioPlayer.PlayOneShot(_jumpSound, onPlayFinished: result => 
        {
            Debug.Log($"재생 완료: {result}");
        });
    }
}
```

## AudioPlayer 인스턴스 생성

지속적으로 사용할 오디오 플레이어가 필요 한 경우 (BGM 재생 등) 사용합니다

```csharp
using Jin5eok;
using UnityEngine;
using UnityEngine.Audio;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _bgmMixer;
    private AudioPlayer _bgmPlayer;
    
    private void Start()
    {
        // AudioPlayer 인스턴스 생성
        _bgmPlayer = AudioPlayer.Create(null, _bgmMixer, transform);
        _bgmPlayer.AudioSource.loop = true;
    }
    
    public void PlayBGM(AudioClip clip)
    {
        _bgmPlayer.AudioSource.clip = clip;
        _bgmPlayer.Play(result => 
        {
            if (result == AudioPlayer.PlayResult.Succeed)
            {
                Debug.Log("BGM 재생 완료");
            }
        });
    }
}
```


## 재생 결과 추적

오디오 재생은 코루틴/비동기 모두 지원합니다, 코루틴의 경우 `PlayResult`를 반환하는 콜백을 사용할 수 있고 비동기의 경우 재생 종료시 `PlayResult`를 반환합니다.

- **Succeed**: 오디오가 정상적으로 끝까지 재생됨
- **Stopped**: 재생이 중단됨 (일시정지, 정지, 또는 클립 변경)
- **Failed**: 재생 실패 (클립이 null이거나 기타 오류)


### 코루틴 콜백 방식

```csharp
public void PlayWithTracking(AudioClip clip)
{
    AudioPlayer.PlayOneShot(clip, onPlayFinished: result => 
    {
        switch (result)
        {
            case AudioPlayer.PlayResult.Succeed:
                Debug.Log("정상적으로 재생 완료");
                break;
            case AudioPlayer.PlayResult.Stopped:
                Debug.Log("재생이 중단되었습니다 (일시정지 또는 정지)");
                break;
            case AudioPlayer.PlayResult.Failed:
                Debug.Log("재생 실패");
                break;
        }
    });
}
```

### async/await 방식

```csharp
using System.Threading.Tasks;

public async void PlayWithAsync(AudioClip clip)
{
    // OneShot 비동기 재생
    var result = await AudioPlayer.PlayOneShotAsync(clip);
    Debug.Log($"재생 완료: {result}");
}
```

## 오디오 믹서 사용
용도에 따라 다른 오디오 믹서를 사용할 수 있습니다, 예를들면 BGM과 SFX를 각각 다른 믹서 그룹으로 재생할 수 있습니다.

```csharp
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _bgmMixer;
    [SerializeField] private AudioMixerGroup _sfxMixer;
    
    private AudioPlayer _bgmPlayer;
    private AudioPlayer _sfxPlayer;
    
    private void Awake()
    {
        // BGM 플레이어 (루프)
        _bgmPlayer = AudioPlayer.Create(null, _bgmMixer, transform);
        _bgmPlayer.AudioSource.loop = true;
        
        // SFX 플레이어
        _sfxPlayer = AudioPlayer.Create(null, _sfxMixer, transform);
    }
    
    public void PlayBGM(AudioClip clip)
    {
        _bgmPlayer.AudioSource.clip = clip;
        _bgmPlayer.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        _sfxPlayer.AudioSource.clip = clip;
        _sfxPlayer.Play();
    }
    
    // 오디오 믹서를 사용하여 OneShot 재생
    public void PlaySFXOneShot(AudioClip clip)
    {
        AudioPlayer.PlayOneShot(clip, _sfxMixer);
    }
}
```

## 기타 사용법

### AudioSource 직접 조작

AudioSource를 직접 조작해야 할 경우

```csharp
private AudioPlayer _player;

private void Start()
{
    _player = AudioPlayer.Create();
    
    _player.AudioSource.volume = 0.5f;
    _player.AudioSource.pitch = 1.2f;
    _player.AudioSource.spatialBlend = 1.0f;
}
```

### AudioPlayerPool 직접 사용

필요한 경우 AudioPlayerPool을 직접 사용할 수 있습니다.

```csharp
using Jin5eok;
using UnityEngine.Pool;

public class CustomAudioManager : MonoBehaviour
{
    private void Start()
    {
        var pool = AudioPlayerPool.Instance.Pool;
        
        // 풀에서 가져오기
        var player = pool.Get();
        player.AudioSource.clip = someClip;
        player.Play();
        
        // 사용 후 반환
        pool.Release(player);
    }
}
```

## 주의사항

⚠️ **AudioPlayer 인스턴스 관리**
- AudioPlayer.Create()로 생성한 인스턴스는 게임오브젝트이므로 수동관리해야 합니다.
  - 더 이상 사용하지 않으면 GameObject를 Destroy하세요
- PlayOneShot의 경우 내부적으로 AudioPlayerPool을 이용해 관리하므로 수동으로 관리할 필요 없습니다.

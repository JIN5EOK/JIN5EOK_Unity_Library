using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Jin5eok
{
    /// <summary>
    /// 오디오 소스를 래핑하는 오디오 플레이어 클래스입니다.
    /// 오디오 재생 간편화 : 정적 함수로 OneShot 오디오를 재생하거나 AudioSource를 래핑한 객체 AudioPlayer를 생성할 수 있습니다.
    /// 재생 방식 지원 : 코루틴 기반 콜백 방식(Play)과 async/await 비동기 방식(PlayAsync)을 모두 제공합니다.
    /// 오디오 재생 결과 반환 : 오디오 재생의 성공, 실패, 중단 여부를 Enum PlayResult로 반환합니다.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private const float PlaybackCompletionTolerance = 0.05f;
        
        public enum PlayResult
        {
            Succeed,
            Stopped, // Paused or Stopped
            Failed
        }
        /// <summary>
        /// 오디오 소스를 조작해야 할 때 사용합니다.
        /// </summary>
        public AudioSource AudioSource { get; private set; }
        
        private void Awake()
        {
            AudioSource = gameObject.AddOrGetComponent<AudioSource>();
        }

        /// <summary>
        /// 새로운 AudioPlayer GameObject를 생성하여 반환합니다.
        /// </summary>
        /// <param name="audioClip">재생할 오디오 클립</param>
        /// <param name="audioMixerGroup">사용할 오디오 믹서 그룹</param>
        /// <param name="parent">부모 Transform</param>
        /// <returns>생성된 AudioPlayer 인스턴스</returns>
        public static AudioPlayer Create(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null, Transform parent = null)
        {
            var playerGameObject = new GameObject(nameof(AudioPlayer));
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();

            playerInstance.AudioSource.clip = audioClip;
            playerInstance.AudioSource.outputAudioMixerGroup = audioMixerGroup;
            playerInstance.transform.SetParent(parent);

            return playerInstance;
        }
        
        /// <summary>
        /// OneShot 형태의 오디오를 재생합니다. (코루틴 콜백 방식)
        /// 플레이 상태를 추적하기 위해 내부적으로 AudioPlayer 오브젝트 풀에서 AudioPlayer를 가져와 Play를 호출하는 방식으로 동작합니다.
        /// 재생이 완료되면 자동으로 풀에 반환됩니다.
        /// </summary>
        /// <param name="audioClip">재생할 오디오 클립</param>
        /// <param name="audioMixerGroup">사용할 오디오 믹서 그룹</param>
        /// <param name="onPlayFinished">플레이 종료 시 호출될 콜백</param>
        public static void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null, Action<PlayResult> onPlayFinished = null)
        {
            if (audioClip == null)
            {
                onPlayFinished?.Invoke(PlayResult.Failed);
                return;
            }
            
            var oneShotPlayer = AudioPlayerPool.Instance.Pool.Get();
            oneShotPlayer.AudioSource.clip = audioClip;
            oneShotPlayer.AudioSource.outputAudioMixerGroup = audioMixerGroup;

            onPlayFinished += _ => { AudioPlayerPool.Instance.Pool.Release(oneShotPlayer); };
            
            oneShotPlayer.Play(onPlayFinished);
        }
        
        /// <summary>
        /// OneShot 형태의 오디오를 비동기적으로 재생합니다. (async/await 방식)
        /// 플레이 상태를 추적하기 위해 내부적으로 AudioPlayer 오브젝트 풀에서 AudioPlayer를 가져와 PlayAsync를 호출하는 방식으로 동작합니다.
        /// 재생이 완료되면 자동으로 풀에 반환됩니다.
        /// </summary>
        /// <param name="audioClip">재생할 오디오 클립</param>
        /// <param name="audioMixerGroup">사용할 오디오 믹서 그룹</param>
        /// <returns>재생 완료 시 PlayResult를 반환하는 Task</returns>
        public static async Task<PlayResult> PlayOneShotAsync(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null)
        {
            if (audioClip == null)
            {
                return PlayResult.Failed;
            }
            
            var oneShotPlayer = AudioPlayerPool.Instance.Pool.Get();
            oneShotPlayer.AudioSource.clip = audioClip;
            oneShotPlayer.AudioSource.outputAudioMixerGroup = audioMixerGroup;

            try
            {
                var result = await oneShotPlayer.PlayAsync();
                return result;
            }
            finally
            {
                AudioPlayerPool.Instance.Pool.Release(oneShotPlayer);
            }
        }
        
        /// <summary>
        /// 오디오를 재생합니다. (코루틴 콜백 방식)
        /// 재생이 완료되면 콜백 함수를 호출합니다.
        /// </summary>
        /// <param name="onPlayFinished">플레이 종료 시 호출될 콜백</param>
        public void Play(Action<PlayResult> onPlayFinished = null)
        {
            if (AudioSource.clip == null)
            {
                onPlayFinished?.Invoke(PlayResult.Failed);
                return;
            }
            
            StartCoroutine(MonitorPlaybackCoroutine(onPlayFinished));
        }
        
        /// <summary>
        /// 오디오를 비동기적으로 재생합니다. (async/await 방식)
        /// 재생이 완료될 때까지 대기하며, 재생 결과를 반환합니다.
        /// </summary>
        /// <returns>재생 완료 시 PlayResult를 반환하는 Task</returns>
        public Task<PlayResult> PlayAsync()
        {
            if (AudioSource.clip == null)
            {
                return Task.FromResult(PlayResult.Failed);
            }
            
            var tcs = new TaskCompletionSource<PlayResult>();
            StartCoroutine(MonitorPlaybackCoroutine(null, tcs));
            return tcs.Task;
        }
        
        private IEnumerator MonitorPlaybackCoroutine(Action<PlayResult> onPlayFinished = null, TaskCompletionSource<PlayResult> tcs = null)
        {
            float playTime = 0f;
            AudioClip playedClip = AudioSource.clip;

            AudioSource.Play();

            // 재생이 완료되거나 중단될 때까지 매 프레임마다 상태를 확인합니다.
            while (AudioSource.isPlaying == true)
            {
                var isPlaybackTargetChanged = playTime > AudioSource.time || playedClip != AudioSource.clip;
                if (isPlaybackTargetChanged == true)
                {
                    break;
                }

                playTime = AudioSource.time;
                yield return null;
            }

            // 루프가 아닌 경우 재생 완료 여부를 확인합니다.
            PlayResult result;
            if (AudioSource.loop == false && playedClip == AudioSource.clip)
            {
                var playBackCompletion = playedClip.length - PlaybackCompletionTolerance;
                result = playTime >= playBackCompletion ? PlayResult.Succeed : PlayResult.Stopped;
            }
            else
            {
                result = PlayResult.Stopped;
            }

            onPlayFinished?.Invoke(result);
            tcs?.SetResult(result);
        }
    }
}

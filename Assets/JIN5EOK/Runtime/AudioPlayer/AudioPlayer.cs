using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Jin5eok
{
    /// <summary>
    /// 오디오 소스를 래핑하는 오디오 플레이어 클래스입니다.
    /// 오디오 재생 간편화 : 정적 함수로 OneShot 오디오를 재생하거나 AudioSource를 래핑한 객체 AudioPlayer를 생성할 수 있습니다.
    /// 오디오 재생 결과 콜백 : 오디오 재생의 성공,실패,중단 여부를 판단하고 그와 관련한 콜백을 등록할 수 있습니다.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private const float PlaybackCompletionTolerance = 0.05f;
        
        public enum PlayResult
        {
            Succeed, // Playback ended without any problems
            Stopped, // Paused or Stopped
            Failed
        }
        /// <summary>
        /// 오디오 소스를 조작해야 할 때 사용합니다.
        /// </summary>
        public AudioSource AudioSource { get; private set; }
        
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
        /// OneShot 형태의 오디오를 재생합니다.
        /// 플레이 상태를 추적하기 위해 내부적으로 AudioPlayer 오브젝트 풀에서 AudioPlayer를 가져와 PlayWithCallback을 호출하는 방식으로 동작합니다.
        /// 플레이 종료시 결과를 반환하는 콜백함수를 등록할 수 있습니다.
        /// </summary>
        /// <param name="audioClip">재생할 오디오 클립</param>
        /// <param name="audioMixerGroup">사용할 오디오 믹서 그룹</param>
        /// <param name="onPlayFinished">플레이 종료 시 호출될 콜백</param>
        public static void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null, Action<PlayResult> onPlayFinished = null)
        {
            if (audioClip == null)
            {
                onPlayFinished?.Invoke(PlayResult.Failed);   
            }
            
            var oneShotPlayer = AudioPlayerPool.Instance.Pool.Get();
            oneShotPlayer.AudioSource.clip = audioClip;
            oneShotPlayer.AudioSource.outputAudioMixerGroup = audioMixerGroup;

            onPlayFinished += _ => { AudioPlayerPool.Instance.Pool.Release(oneShotPlayer); };
            
            oneShotPlayer.Play(onPlayFinished);
        }


        private void Awake()
        {
            AudioSource = gameObject.AddOrGetComponent<AudioSource>();
        }
        
        /// <summary>
        /// 오디오를 재생하며 동시에 플레이 종료시 결과를 반환하는 콜백함수를 등록할 수 있습니다.
        /// </summary>
        /// <param name="onPlayFinished">플레이 종료 시 호출될 콜백</param>
        public void Play(Action<PlayResult> onPlayFinished = null)
        {
            if (AudioSource.clip == null)
            {
                onPlayFinished?.Invoke(PlayResult.Failed);
            }
            else
            {
                StartCoroutine(MonitorPlayback(onPlayFinished));    
            }
        }
        
        private IEnumerator MonitorPlayback(Action<PlayResult> onPlayFinished = null)
        {
            float playTime = 0f;
            AudioClip playedClip = AudioSource.clip;
            
            AudioSource.Play();
            
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
            
            if (AudioSource.loop == false && playedClip == AudioSource.clip)
            {
                var playBackCompletion = playedClip.length - PlaybackCompletionTolerance;
                if (playTime >= playBackCompletion)
                {
                    onPlayFinished?.Invoke(PlayResult.Succeed);
                }
                else
                {
                    onPlayFinished?.Invoke(PlayResult.Stopped);
                }
            }
            else
            {
                onPlayFinished?.Invoke(PlayResult.Stopped);
            }
        }
    }
}
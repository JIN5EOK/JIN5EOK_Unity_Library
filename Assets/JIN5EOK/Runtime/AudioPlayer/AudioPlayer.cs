using System;
using System.Collections;
using System.Collections.Generic;
using Jin5eok.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace Jin5eok.Audios
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private static GameObject _oneShotPlayerParent;
        private static Dictionary<int, AudioPlayer> _oneShotAudioPlayers = new ();
        
        private const float PlaybackCompletionTolerance = 0.05f;
        
        public enum PlayResult
        {
            Succeed, // Playback ended without any problems
            Stopped, // Paused or Stopped
            Failed
        }
        public AudioSource AudioSource { get; private set; }
        public static AudioPlayer Create(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null, Transform parent = null)
        {
            // if No AudioMixerGroup, Use Empty string
            string mixerName = audioMixerGroup == null ? String.Empty : $"{audioMixerGroup.name}";
            
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}:{mixerName}");
            playerGameObject.transform.SetParent(parent);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            
            playerInstance.AudioSource.clip = audioClip;
            playerInstance.AudioSource.outputAudioMixerGroup = audioMixerGroup;

            return playerInstance;
        }
        
        public static void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup, Action<PlayResult> onPlayFinished = null)
        {
            // if No AudioMixerGroup, Use 0
            int hashCodeKey = audioMixerGroup?.GetHashCode() ?? 0;
            
            if (_oneShotAudioPlayers.TryGetValue(hashCodeKey, out var oneShotPlayer) == false)
            {
                if (_oneShotPlayerParent == null)
                {
                    _oneShotPlayerParent = new GameObject("OneShotAudioPlayers");
                    DontDestroyOnLoad(_oneShotPlayerParent);
                }
                
                oneShotPlayer = Create(null, audioMixerGroup, _oneShotPlayerParent.transform);
                oneShotPlayer.name += "(OneShotPlayer)";
                _oneShotAudioPlayers.Add(hashCodeKey, oneShotPlayer);
            }

            if (audioClip == null)
            {
                onPlayFinished?.Invoke(PlayResult.Failed);   
            }
            else
            {
                oneShotPlayer.AudioSource.PlayOneShot(audioClip);
                CoroutineHelper.Delay(audioClip.length, () => onPlayFinished?.Invoke(PlayResult.Succeed), oneShotPlayer);    
            }
        }
        
        private void Awake()
        {
            AudioSource = gameObject.AddOrGetComponent<AudioSource>();
        }
        
        public void PlayWithCallback(Action<PlayResult> onPlayFinished = null)
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
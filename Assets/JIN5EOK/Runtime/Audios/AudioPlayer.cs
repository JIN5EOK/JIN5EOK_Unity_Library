using System;
using System.Collections;
using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Audios
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        public enum PlayResult
        {
            Succeed, // Playback ended without any problems
            Stopped, // Paused or Stopped
            Failed
        }
        public AudioSource AudioSource { get; private set; }
        
        private Coroutine _monitorRoutine;
        
        private void Awake()
        {
            AudioSource = gameObject.AddOrGetComponent<AudioSource>();
        }
        
        public void Play(Action<PlayResult> onPlayFinished = null)
        {
            if (AudioSource.clip == null)
            {
                onPlayFinished?.Invoke(PlayResult.Failed);
            }
            else
            {
                if (_monitorRoutine != null)
                {
                    StopCoroutine(_monitorRoutine);
                    onPlayFinished?.Invoke(PlayResult.Stopped);
                }
                
                _monitorRoutine = StartCoroutine(MonitorPlayback(onPlayFinished));    
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
                if (playTime >= playedClip.length - 0.05f)
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

            _monitorRoutine = null;
        }
        
        public void PlayOneShot(AudioClip audioClip)
        {
            try
            {
                AudioSource.PlayOneShot(audioClip);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        public void Pause()
        {
            AudioSource.Pause();
        }
        
        public void Stop()
        {
            AudioSource.Stop();
        }
    }
}
using System;
using UnityEngine;

namespace Jin5eok.Audios
{
    public class AudioPlayer : MonoBehaviour
    {
        public event Action OnPlay;
        
        /// <summary>
        /// Avoid using play, pause, and stop whenever possible!
        /// </summary>
        public AudioSource AudioSource { get; private set; }

        private void Awake()
        {
            if (AudioSource != null)
            {
                AudioSource = gameObject.AddComponent<AudioSource>();    
            }
        }
        
        public AudioPlayResult Play()
        {
            try
            {
                AudioSource.Play();
                return AudioPlayResult.GetSucceedResult(AudioSource.clip);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return AudioPlayResult.GetFailedResult();
            }
        }

        public AudioPlayResult PlayOneShot(AudioClip audioClip)
        {
            try
            {
                AudioSource.PlayOneShot(audioClip);
                return AudioPlayResult.GetSucceedResult(audioClip);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return AudioPlayResult.GetFailedResult();
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
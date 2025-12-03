using UnityEngine;
using UnityEngine.Pool;

namespace Jin5eok
{
    /// <summary>
    /// AudioPlayer의 오브젝트 풀입니다.
    /// 생성 로직으로 AudioPlayer의 Create 함수를 사용합니다.
    /// AudioPlayer의 PlayOneShot에서 사용하기 위해 만들었으나 필요한 경우 사용해도 무방합니다.
    /// </summary>
    public class AudioPlayerPool : MonoSingleton<AudioPlayerPool>
    {
        /// <summary>
        /// 오브젝트 풀
        /// </summary>
        public IObjectPool<AudioPlayer> Pool { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            Pool = new ObjectPool<AudioPlayer>(Create, Get, Release);
        }
        
        private AudioPlayer Create()
        {
            return AudioPlayer.Create(null,null, transform);
        }
        
        private void Get(AudioPlayer player)
        {
            player.gameObject.SetActive(true);
        }
        
        private void Release(AudioPlayer player)
        {
            ResetAudioSource(player.AudioSource);
            player.transform.SetParent(transform);
            player.gameObject.SetActive(false);
        }
        
        private void ResetAudioSource(AudioSource source)
        {
            source.clip = null;
            source.outputAudioMixerGroup = null;
            source.playOnAwake = false;
            source.loop = false;
            source.mute = false;
            source.bypassEffects = false;
            source.bypassListenerEffects = false;
            source.bypassReverbZones = false;
            source.priority = 128;
            source.volume = 1f;
            source.pitch = 1f;
            source.panStereo = 0f;
            source.spatialBlend = 0f;
            source.reverbZoneMix = 1f;
            source.dopplerLevel = 1f;
            source.spread = 0f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.minDistance = 1f;
            source.maxDistance = 500f;
        }
    }
}
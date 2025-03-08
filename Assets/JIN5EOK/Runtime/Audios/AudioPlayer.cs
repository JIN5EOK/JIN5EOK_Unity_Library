using System;
using UnityEngine;

namespace Jin5eok.Audios
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioClip AudioClip => _audioSource.clip;
        public bool IsPlaying => _audioSource.isPlaying;
        public SoundType SoundType { get; private set; }
        private float _volumeScale;

        public float VolumeScale
        {
            get => _volumeScale;
            private set => _volumeScale = Mathf.Clamp01(value);
        }
        private float DefaultVolume => 
            SoundType == SoundType.Bgm ? AudioModel.Instance.BgmVolume :
            SoundType == SoundType.Sfx ? AudioModel.Instance.SfxVolume : 1.0f;
        
        private AudioSource _audioSource;
        
        public void Initialize(AudioClip audioClip, SoundType soundType)
        {
            _audioSource.clip = audioClip;
            SoundType = soundType;
            AddVolumeChangeEvent();
        }
        
        public AudioPlayResult Play()
        {
            _audioSource.Play();
            return AudioClip != null ? AudioPlayResult.GetSucceedResult(AudioClip) : AudioPlayResult.GetFailedResult();
        }
        
        public void Pause()
        {
            _audioSource.Pause();
        }
        
        public void Stop()
        {
            _audioSource.Stop();
        }
        
        public void SetVolumeScale(float to)
        {
            VolumeScale = to;
            SetAudioSourceVolume();
        }
        
        public async Awaitable FadeVolumeScale(float from, float to, float duration)
        {
            while (duration > 0f)
            {
                duration -= Time.deltaTime;
                SetVolumeScale(Mathf.Lerp(from, to, duration));
                await Awaitable.NextFrameAsync();
            }
            SetVolumeScale(to);
        }
        
        private void AddVolumeChangeEvent()
        {
            switch (SoundType)
            {
                case SoundType.Bgm:
                    AudioModel.Instance.OnBgmVolumeChanged += OnDefaultVolumeChanged;  
                    break;
                case SoundType.Sfx:
                    AudioModel.Instance.OnSfxVolumeChanged += OnDefaultVolumeChanged;
                    break;
            }
        }
        
        private void RemoveVolumeChangeEvent()
        {
            switch (SoundType)
            {
                case SoundType.Bgm:
                    AudioModel.Instance.OnBgmVolumeChanged -= OnDefaultVolumeChanged;  
                    break;
                case SoundType.Sfx:
                    AudioModel.Instance.OnSfxVolumeChanged -= OnDefaultVolumeChanged;
                    break;
            }
        }
        
        private void OnDefaultVolumeChanged(float defaultVolume)
        {
            SetAudioSourceVolume();
        }
        
        private void SetAudioSourceVolume()
        {
            _audioSource.volume = DefaultVolume * VolumeScale;
        }
        
        public void OnDestroy()
        {
            Stop();
            RemoveVolumeChangeEvent();
        }
    }
}
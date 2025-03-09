using System.Collections;
using UnityEngine;

namespace Jin5eok.Audios
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioClip AudioClip => _audioSource.clip;
        public bool IsPlaying => _audioSource.isPlaying;
        public AudioModel AudioPlayerModel { get; set; } = new GlobalAudio();
        
        private AudioModel _baseAudioModel;
        private GlobalAudio _globalAudio;
        private AudioSource _audioSource;
        private Coroutine _fadeCoroutine;
        
        public void Initialize(AudioClip audioClip, AudioModel baseAudioModel, GlobalAudio globalAudio)
        {
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();    
            }
            _audioSource.clip = audioClip;
            _baseAudioModel = baseAudioModel;
            _globalAudio = globalAudio;
            AddEvent();
            OnAudioModelsChanged();
        }

        private void AddEvent()
        {
            _globalAudio.OnVolumeChanged += OnVolumeChanged;
            _globalAudio.OnMuteChanged += OnMuteChanged;
            _baseAudioModel.OnVolumeChanged += OnVolumeChanged;
            _baseAudioModel.OnMuteChanged += OnMuteChanged;
            AudioPlayerModel.OnVolumeChanged += OnVolumeChanged;
            AudioPlayerModel.OnMuteChanged += OnMuteChanged;
        }

        private void OnVolumeChanged(float defaultVolume)
        {
            OnAudioModelsChanged();
        }
        
        private void OnMuteChanged(bool mute)
        {
            OnAudioModelsChanged();
        }
        
        public AudioPlayResult Play()
        {
            if (AudioClip == null)
            {
                return AudioPlayResult.GetFailedResult();
            }
            
            _audioSource.Play();
            return AudioPlayResult.GetSucceedResult(AudioClip);
        }
        
        public void Pause()
        {
            _audioSource.Pause();
        }
        
        public void Stop()
        {
            _audioSource.Stop();
        }
        
        public void FadeVolumeScale(float from, float to, float duration)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeVolumeScaleCoroutine(from, to, duration));
        }
        
        private IEnumerator FadeVolumeScaleCoroutine(float from, float to, float targetDuration)
        {
            var time = 0.0f;
            while (time < targetDuration)
            {
                time += Time.deltaTime;
                AudioPlayerModel.Volume = (Mathf.Lerp(from, to, time / targetDuration)); 
                yield return null;
            }
            AudioPlayerModel.Volume = to;
        }
        
        private void OnAudioModelsChanged()
        {
            _audioSource.mute = AudioPlayerModel.Mute || _baseAudioModel.Mute || _globalAudio.Mute;
            _audioSource.volume = AudioPlayerModel.Volume * _baseAudioModel.Volume * _globalAudio.Volume;
        }
        
        public void OnDestroy()
        {
            Stop();
            RemoveVolumeChangeEvent();
        }

        private void RemoveVolumeChangeEvent()
        {
            _globalAudio.OnVolumeChanged -= OnVolumeChanged;
            _globalAudio.OnMuteChanged -= OnMuteChanged;
            _baseAudioModel.OnVolumeChanged -= OnVolumeChanged;
            _baseAudioModel.OnMuteChanged -= OnMuteChanged;
            AudioPlayerModel.OnVolumeChanged -= OnVolumeChanged;
            AudioPlayerModel.OnMuteChanged -= OnMuteChanged;
        }
    }
}
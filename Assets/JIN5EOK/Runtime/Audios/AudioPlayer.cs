using System.Collections;
using UnityEngine;

namespace Jin5eok.Audios
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioClip AudioClip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }
        public AudioModel AudioModelOfPlayer { get; } = new GlobalAudio();
        public bool Loop
        {
            get => _audioSource.loop;
            set => _audioSource.loop = value;
        }
        public bool IsPlaying => _audioSource.isPlaying;
        public float PlayTime => _audioSource.time;
        public float Length => _audioSource.clip.length;
        
        private AudioModel _baseAudioModel;
        private GlobalAudio _globalAudio;
        private AudioSource _audioSource;
        private Coroutine _fadeVolumeCoroutine;
        private Coroutine _fadePitchCoroutine; 
        
        private enum ScaleType
        {
            Volume,
            Pitch,
        }
        
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
            _globalAudio.OnPitchChanged += OnPitchChanged;
            _baseAudioModel.OnVolumeChanged += OnVolumeChanged;
            _baseAudioModel.OnMuteChanged += OnMuteChanged;
            _baseAudioModel.OnPitchChanged += OnPitchChanged;
            AudioModelOfPlayer.OnVolumeChanged += OnVolumeChanged;
            AudioModelOfPlayer.OnMuteChanged += OnMuteChanged;
            AudioModelOfPlayer.OnPitchChanged += OnPitchChanged;
        }

        private void OnVolumeChanged(float volume)
        {
            OnAudioModelsChanged();
        }
        
        private void OnPitchChanged(float pitch)
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

        public void FadeVolume(float from, float to, float duration)
        {
            if (_fadeVolumeCoroutine != null)
            {
                StopCoroutine(_fadeVolumeCoroutine);
            }
            _fadeVolumeCoroutine = StartCoroutine(FadeScaleCoroutine(from, to, duration, ScaleType.Volume));
        }
        
        public void FadePitch(float from, float to, float duration)
        {
            if (_fadeVolumeCoroutine != null)
            {
                StopCoroutine(_fadePitchCoroutine);
            }
            _fadePitchCoroutine = StartCoroutine(FadeScaleCoroutine(from, to, duration, ScaleType.Pitch));
        }
        
        private IEnumerator FadeScaleCoroutine(float from, float to, float duration, ScaleType scaleType)
        {
            var time = 0.0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                ScaleChange(Mathf.Lerp(from, to, time / duration), scaleType);
                yield return null;
            }
            ScaleChange(to, scaleType);

            void ScaleChange(float scale, ScaleType typeOfScale)
            {
                switch (typeOfScale)
                {
                    case ScaleType.Volume:
                        AudioModelOfPlayer.Volume = scale;
                        break;
                    case ScaleType.Pitch:
                        AudioModelOfPlayer.Pitch = scale;
                        break;
                }
            }
        }
        
        private void OnAudioModelsChanged()
        {
            _audioSource.mute = AudioModelOfPlayer.Mute || _baseAudioModel.Mute || _globalAudio.Mute;
            _audioSource.volume = AudioModelOfPlayer.Volume * _baseAudioModel.Volume * _globalAudio.Volume;
            _audioSource.pitch = AudioModelOfPlayer.Pitch * _baseAudioModel.Pitch * _globalAudio.Pitch;
        }
        
        private void OnDestroy()
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
            AudioModelOfPlayer.OnVolumeChanged -= OnVolumeChanged;
            AudioModelOfPlayer.OnMuteChanged -= OnMuteChanged;
        }
    }
}
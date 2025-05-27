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
        public AudioModel PlayerAudioModel { get; } = new AudioModel();
        public bool Loop
        {
            get => _audioSource.loop;
            set => _audioSource.loop = value;
        }
        public bool IsPlaying => _audioSource.isPlaying;
        public float PlayTime => _audioSource.time;
        public float Length => _audioSource.clip.length;
        
        private AudioModel _baseAudioModel;
        private AudioModel _globalAudioModel;
        private AudioSource _audioSource;
        private Coroutine _fadeVolumeCoroutine;
        private Coroutine _fadePitchCoroutine;
        private bool _isInit;
        
        private enum ScaleType
        {
            Volume,
            Pitch,
        }
        
        public void Initialize(AudioClip audioClip, AudioModel baseAudioModel, AudioModel globalAudioModel)
        {
            if (_isInit == true)
                return;

            _isInit = true;
            
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();    
            }
            
            _audioSource.clip = audioClip;
            _baseAudioModel = baseAudioModel;
            _globalAudioModel = globalAudioModel;
            AddEvent();
            OnAudioModelsChanged();
        }

        private void AddEvent()
        {
            _globalAudioModel.OnVolumeChanged += OnVolumeChanged;
            _globalAudioModel.OnMuteChanged += OnMuteChanged;
            _globalAudioModel.OnPitchChanged += OnPitchChanged;
            _baseAudioModel.OnVolumeChanged += OnVolumeChanged;
            _baseAudioModel.OnMuteChanged += OnMuteChanged;
            _baseAudioModel.OnPitchChanged += OnPitchChanged;
            PlayerAudioModel.OnVolumeChanged += OnVolumeChanged;
            PlayerAudioModel.OnMuteChanged += OnMuteChanged;
            PlayerAudioModel.OnPitchChanged += OnPitchChanged;
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
            if (_fadePitchCoroutine != null)
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
                        PlayerAudioModel.Volume = scale;
                        break;
                    case ScaleType.Pitch:
                        PlayerAudioModel.Pitch = scale;
                        break;
                }
            }
        }
        
        private void OnAudioModelsChanged()
        {
            _audioSource.mute = PlayerAudioModel.Mute || _baseAudioModel.Mute || _globalAudioModel.Mute;
            _audioSource.volume = Mathf.Clamp01(PlayerAudioModel.Volume * _baseAudioModel.Volume * _globalAudioModel.Volume);
            _audioSource.pitch  = Mathf.Clamp(PlayerAudioModel.Pitch  * _baseAudioModel.Pitch  * _globalAudioModel.Pitch, 0.1f, 3f);
        }
        
        private void OnDestroy()
        {
            Stop();
            RemoveVolumeChangeEvent();
        }

        private void RemoveVolumeChangeEvent()
        {
            _globalAudioModel.OnVolumeChanged -= OnVolumeChanged;
            _globalAudioModel.OnMuteChanged -= OnMuteChanged;
            _globalAudioModel.OnPitchChanged -= OnPitchChanged;
            _baseAudioModel.OnVolumeChanged -= OnVolumeChanged;
            _baseAudioModel.OnMuteChanged -= OnMuteChanged;
            _baseAudioModel.OnPitchChanged -= OnPitchChanged;
            PlayerAudioModel.OnVolumeChanged -= OnVolumeChanged;
            PlayerAudioModel.OnMuteChanged -= OnMuteChanged;
            PlayerAudioModel.OnPitchChanged -= OnPitchChanged;
        }
    }
}
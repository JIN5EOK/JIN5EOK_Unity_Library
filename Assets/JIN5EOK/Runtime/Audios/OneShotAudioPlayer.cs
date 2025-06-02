using UnityEngine;

namespace Jin5eok.Audios
{
    internal class OneShotAudioPlayer : MonoBehaviour
    {
        public AudioModel PlayerAudioModel { get; private set; }
        
        private AudioSource _audioSource;
        private AudioModel _globalAudioModel;
        private bool _isInit;
        
        public void Initialize(AudioModel playerAudioModel, AudioModel globalAudioModel)
        {
            if (_isInit == true)
            {
                return;
            }
            
            _isInit = true;
            
            PlayerAudioModel = playerAudioModel;
            _globalAudioModel = globalAudioModel;
            _audioSource = gameObject.AddComponent<AudioSource>();

            if (_globalAudioModel != null)
            {
                _globalAudioModel.OnVolumeChanged += OnVolumeChanged;
                _globalAudioModel.OnMuteChanged += OnMuteChanged;
                _globalAudioModel.OnPitchChanged += OnPitchChanged;    
            }
            
            PlayerAudioModel.OnVolumeChanged += OnVolumeChanged;
            PlayerAudioModel.OnMuteChanged += OnMuteChanged;
            PlayerAudioModel.OnPitchChanged += OnPitchChanged;
            
            OnVolumeChanged();
        }

        public AudioPlayResult Play(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
            return AudioPlayResult.GetSucceedResult(clip);
        }

        private void OnVolumeChanged()
        {
            _audioSource.volume = Mathf.Clamp01(PlayerAudioModel.Volume * _globalAudioModel.Volume);
            _audioSource.pitch = Mathf.Clamp(PlayerAudioModel.Pitch * _globalAudioModel.Pitch, 0.1f, 3f);;
            _audioSource.mute = PlayerAudioModel.Mute || _globalAudioModel.Mute;
        }
        
        private void OnVolumeChanged(float volume)
        {
            OnVolumeChanged();
        }

        private void OnPitchChanged(float volume)
        {
            OnVolumeChanged();
        }
        
        private void OnMuteChanged(bool mute)
        {
            OnVolumeChanged();
        }

        private void OnDestroy()
        {
            if (_isInit == false)
            {
                return;
            }

            if (_globalAudioModel != null)
            {
                _globalAudioModel.OnVolumeChanged -= OnVolumeChanged;
                _globalAudioModel.OnMuteChanged -= OnMuteChanged;
                _globalAudioModel.OnPitchChanged -= OnPitchChanged;    
            }
            
            PlayerAudioModel.OnVolumeChanged -= OnVolumeChanged;
            PlayerAudioModel.OnMuteChanged -= OnMuteChanged;
            PlayerAudioModel.OnPitchChanged -= OnPitchChanged;
        }
    }
}
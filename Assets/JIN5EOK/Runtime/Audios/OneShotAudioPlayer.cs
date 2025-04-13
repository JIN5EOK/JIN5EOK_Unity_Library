using UnityEngine;

namespace Jin5eok.Audios
{
    internal class OneShotAudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private GlobalAudio _globalAudio;
        public AudioModel AudioModel { get; private set; }

        private bool _isInit;
        
        public void Initialize(AudioModel audioModel, GlobalAudio globalAudio)
        {
            if (_isInit == true)
            {
                return;
            }
            _isInit = true;
            AudioModel = audioModel;
            _globalAudio = globalAudio;
            _audioSource = gameObject.AddComponent<AudioSource>();
            AudioModel = audioModel;
            
            _globalAudio.OnVolumeChanged += OnVolumeChanged;
            _globalAudio.OnMuteChanged += OnMuteChanged;
            _globalAudio.OnPitchChanged += OnPitchChanged;
            AudioModel.OnVolumeChanged += OnVolumeChanged;
            AudioModel.OnMuteChanged += OnMuteChanged;
            AudioModel.OnPitchChanged += OnPitchChanged;
            
            OnVolumeChanged();
        }

        public AudioPlayResult Play(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
            return AudioPlayResult.GetSucceedResult(clip);
        }

        private void OnVolumeChanged()
        {
            _audioSource.volume = AudioModel.Volume * _globalAudio.Volume;
            _audioSource.pitch = AudioModel.Pitch * _globalAudio.Pitch;
            _audioSource.mute = AudioModel.Mute || _globalAudio.Mute;
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
            _globalAudio.OnVolumeChanged -= OnVolumeChanged;
            _globalAudio.OnMuteChanged -= OnMuteChanged;
            AudioModel.OnVolumeChanged -= OnVolumeChanged;
            AudioModel.OnMuteChanged -= OnMuteChanged;
        }
    }
}
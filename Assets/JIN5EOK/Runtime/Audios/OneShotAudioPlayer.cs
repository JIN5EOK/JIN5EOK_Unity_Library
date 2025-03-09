using UnityEngine;

namespace Jin5eok.Audios
{
    internal class OneShotAudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private GlobalAudio _globalAudio;
        private AudioModel _audioModel;

        private bool _isInit;
        
        public void Initialize(AudioModel audioModel, GlobalAudio globalAudio)
        {
            if (_isInit == true)
            {
                return;
            }
            _isInit = true;
            _audioModel = audioModel;
            _globalAudio = globalAudio;
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioModel = audioModel;
            
            _globalAudio.OnVolumeChanged += OnVolumeChanged;
            _globalAudio.OnMuteChanged += OnMuteChanged;
            _audioModel.OnVolumeChanged += OnVolumeChanged;
            _audioModel.OnMuteChanged += OnMuteChanged;
        }

        public AudioPlayResult Play(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
            return AudioPlayResult.GetSucceedResult(clip);
        }

        private void OnVolumeChanged(float volume)
        {
            _audioSource.volume = _audioModel.Volume * _globalAudio.Volume;
        }

        private void OnMuteChanged(bool mute)
        {
            _audioSource.mute = _audioModel.Mute || _globalAudio.Mute;
        }

        private void OnDestroy()
        {
            _globalAudio.OnVolumeChanged -= OnVolumeChanged;
            _globalAudio.OnMuteChanged -= OnMuteChanged;
            _audioModel.OnVolumeChanged -= OnVolumeChanged;
            _audioModel.OnMuteChanged -= OnMuteChanged;
        }
    }
}
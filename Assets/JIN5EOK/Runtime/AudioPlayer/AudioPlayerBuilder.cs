using UnityEngine;
using UnityEngine.Audio;

namespace Jin5eok.Audios
{
    public class AudioPlayerBuilder
    {
        private AudioClip _audioClip;
        private AudioMixerGroup _audioMixerGroup;
        private Transform _parent;
        
        public AudioPlayerBuilder(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null, Transform parent = null)
        {
            SetAudioClip(audioClip)
            .SetAudioMixerGroup(audioMixerGroup)
            .SetTransformParent(parent);
        }

        public AudioPlayerBuilder SetAudioClip(AudioClip audioClip)
        {
            _audioClip = audioClip;
            return this;
        }

        public AudioPlayerBuilder SetAudioMixerGroup(AudioMixerGroup audioMixerGroup)
        {
            _audioMixerGroup = audioMixerGroup;
            return this;
        }
        
        public AudioPlayerBuilder SetTransformParent(Transform parent)
        {
            _parent = parent;
            return this;
        }

        public AudioPlayer Build()
        {
            string mixerName = _audioMixerGroup == null ? "NoMixerGroup" : $"{_audioMixerGroup.name}";
            
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}:{mixerName}");
            playerGameObject.transform.SetParent(_parent);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            
            playerInstance.AudioSource.clip = _audioClip;
            playerInstance.AudioSource.outputAudioMixerGroup = _audioMixerGroup;
            
            return playerInstance;
        }
    }
}
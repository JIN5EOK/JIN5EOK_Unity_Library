using System.Collections.Generic;
using Jin5eok.Patterns;
using UnityEngine;
using UnityEngine.Audio;
#if USE_ADDRESSABLES
#endif

namespace Jin5eok.Audios
{
    public class AudioPlayerManager : MonoSingleton<AudioPlayerManager>
    {
        private Dictionary<string, AudioPlayer> _oneShotAudioPlayers = new ();
        
        private AudioPlayerBuilder _audioPlayerBuilder;
        
        AudioMixerGroup _emptyAudioMixerGroup;
        public AudioPlayer InstantiateAudioPlayer(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null, Transform parent = null)
        {
            if (_audioPlayerBuilder == null)
            {
                _audioPlayerBuilder = new AudioPlayerBuilder();
            }
            
            _audioPlayerBuilder
                .SetAudioClip(audioClip)
                .SetAudioMixerGroup(audioMixerGroup)
                .SetTransformParent(parent);
            
            return _audioPlayerBuilder.Build();
        }
        
        public void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null)
        {
            // if No AudioMixerGroup, Use Empty String Key
            string key = audioMixerGroup == null ? string.Empty : audioMixerGroup.name;
            
            if (_oneShotAudioPlayers.ContainsKey(key) == false)
            {
                var oneShotPlayer = InstantiateAudioPlayer(audioClip, audioMixerGroup, transform);
                oneShotPlayer.name = oneShotPlayer.name + "(OneShotPlayer)";
                _oneShotAudioPlayers.Add(key, oneShotPlayer);
            }
            _oneShotAudioPlayers[key].PlayOneShot(audioClip);
        }
    }   
}
using System.Collections.Generic;
using Jin5eok.Patterns;
using UnityEngine;
using UnityEngine.Audio;
#if USE_ADDRESSABLES
#endif

namespace Jin5eok.Audios
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private Dictionary<AudioMixerGroup, AudioPlayer> _oneShotAudioPlayers = new ();
        
        public AudioPlayer InstantiateAudioPlayer(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null)
        {
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}:{audioMixerGroup.name}");
            playerGameObject.transform.SetParent(transform);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            
            playerInstance.AudioSource.clip = audioClip;
            playerInstance.AudioSource.outputAudioMixerGroup = audioMixerGroup;
            
            return playerInstance;
        }
        
        public AudioPlayResult PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null)
        {
            if (_oneShotAudioPlayers.ContainsKey(audioMixerGroup) == false)
            {
                var oneShotPlayerGameObject = new GameObject($"GlobalOneShotAudioPlayer:{audioMixerGroup?.name ?? "NoMixer"}");
                oneShotPlayerGameObject.transform.SetParent(transform);
                
                var oneShotPlayer = oneShotPlayerGameObject.AddComponent<AudioPlayer>();
                
                oneShotPlayer.AudioSource.clip = audioClip;
                oneShotPlayer.AudioSource.outputAudioMixerGroup = audioMixerGroup;
                
                _oneShotAudioPlayers.Add(audioMixerGroup, oneShotPlayer);
            }
            
            return _oneShotAudioPlayers[audioMixerGroup].PlayOneShot(audioClip);
        }
    }   
}
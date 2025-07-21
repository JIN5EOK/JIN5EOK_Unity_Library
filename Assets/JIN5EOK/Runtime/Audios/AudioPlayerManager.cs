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
        private Dictionary<AudioMixerGroup, AudioPlayer> _oneShotAudioPlayers = new ();
        
        public AudioPlayer InstantiateAudioPlayer(AudioClip audioClip = null, AudioMixerGroup audioMixerGroup = null)
        {
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}:{audioMixerGroup?.name ?? "NoMixer"}");
            playerGameObject.transform.SetParent(transform);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            
            playerInstance.AudioSource.clip = audioClip;
            playerInstance.AudioSource.outputAudioMixerGroup = audioMixerGroup;
            
            return playerInstance;
        }
        
        public void PlayOneShot(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null)
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
            
            _oneShotAudioPlayers[audioMixerGroup].PlayOneShot(audioClip);
        }
    }   
}
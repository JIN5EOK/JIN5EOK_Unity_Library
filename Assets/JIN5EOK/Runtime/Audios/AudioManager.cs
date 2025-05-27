using System;
using System.Collections.Generic;
using Jin5eok.Helper;
using Jin5eok.Patterns;
using UnityEngine;
#if USE_ADDRESSABLES
#endif

namespace Jin5eok.Audios
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public AudioModel GlobalAudioModel { get; private set; } = new AudioModel();
        private Dictionary<string, AudioModel> _audioModels = new ();
        private Dictionary<string, OneShotAudioPlayer> _oneShotAudioPlayers = new ();
        
        public bool TryAddAudioType(string key)
        {
            if (_audioModels.ContainsKey(key) || _oneShotAudioPlayers.ContainsKey(key))
                return false;
            
            var audioModel = new AudioModel();
            _audioModels.Add(key, audioModel);
            
            var oneShotPlayerGameObject = new GameObject($"{nameof(OneShotAudioPlayer)}:{key}");
            oneShotPlayerGameObject.transform.SetParent(transform);
                
            var oneShotPlayer = oneShotPlayerGameObject.AddComponent<OneShotAudioPlayer>();
            oneShotPlayer.Initialize(_audioModels[key], GlobalAudioModel);
            _oneShotAudioPlayers.Add(key, oneShotPlayer);

            return true;
        }
        
        public AudioPlayer InstantiateAudioPlayer(AudioClip audioClip, string audioType)
        {
            if (_audioModels.ContainsKey(audioType) == false)
                return null;
            
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}:{audioType}/{audioClip.name}");
            playerGameObject.transform.SetParent(transform);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            playerInstance.Initialize(audioClip, _audioModels[audioType], GlobalAudioModel);
            return playerInstance;
        }
        
        public AudioPlayResult PlayOneShot(AudioClip sfxClip, string audioType)
        {
            if (_oneShotAudioPlayers.ContainsKey(audioType) == true)
            {
                return _oneShotAudioPlayers[audioType].Play(sfxClip);    
            }
            return AudioPlayResult.GetFailedResult();
        }

        public AudioModel GetAudioModel(string key)
        {
            if (_audioModels.TryGetValue(key, out var model))
            {
                return model;
            }
            return null;
        }
    }   
}
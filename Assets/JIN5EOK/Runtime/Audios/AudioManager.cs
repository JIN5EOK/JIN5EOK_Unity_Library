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
        public readonly string GlobalAudioKey = "Global";
        private Dictionary<string, AudioModel> _audioModels = new ();
        private Dictionary<string, OneShotAudioPlayer> _oneShotAudioPlayers = new ();

        protected override void Awake()
        {
            base.Awake();
            TryAddAudioType(GlobalAudioKey);
        }
        
        public bool TryAddAudioType(string key)
        {
            if (_audioModels.ContainsKey(key) || _oneShotAudioPlayers.ContainsKey(key))
                return false;
            
            var audioModel = new AudioModel();
            _audioModels.Add(key, audioModel);
            
            var oneShotPlayerGameObject = new GameObject($"{nameof(OneShotAudioPlayer)}:{key}");
            oneShotPlayerGameObject.transform.SetParent(transform);
                
            var oneShotPlayer = oneShotPlayerGameObject.AddComponent<OneShotAudioPlayer>();
            oneShotPlayer.Initialize(_audioModels[key], GetAudioModelGlobal());
            _oneShotAudioPlayers.Add(key, oneShotPlayer);

            return true;
        }
        
        public AudioPlayer InstantiateAudioPlayerGlobal(AudioClip audioClip)
        {
            return InstantiateAudioPlayer(audioClip, GlobalAudioKey);
        }
        
        public AudioPlayer InstantiateAudioPlayer(AudioClip audioClip, string audioType)
        {
            if (_audioModels.ContainsKey(audioType) == false)
                return null;
            
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}:{audioType}/{audioClip.name}");
            playerGameObject.transform.SetParent(transform);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            playerInstance.Initialize(audioClip, _audioModels[audioType], _audioModels[GlobalAudioKey]);
            return playerInstance;
        }

        public AudioPlayResult PlayOneShotGlobal(AudioClip sfxClip)
        {
            return PlayOneShot(sfxClip, GlobalAudioKey);
        }
        
        public AudioPlayResult PlayOneShot(AudioClip sfxClip, string audioType)
        {
            if (_oneShotAudioPlayers.ContainsKey(audioType) == true)
            {
                return _oneShotAudioPlayers[audioType].Play(sfxClip);    
            }
            return AudioPlayResult.GetFailedResult();
        }
        
        public AudioModel GetAudioModelGlobal()
        {
            return _audioModels[GlobalAudioKey];
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
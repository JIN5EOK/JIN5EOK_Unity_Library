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
        private GlobalAudio _globalAudio;
        private Dictionary<Type, AudioModel> _audioModels = new ();
        private Dictionary<Type, OneShotAudioPlayer> _oneShotAudioPlayers = new ();
        
        protected override void Awake()
        {
            base.Awake();
            var subclasses = ReflectionHelper.GetSubclasses<AudioModel>();
            
            foreach (var sub in subclasses)
            {
                var audioModel = Activator.CreateInstance(sub) as AudioModel;
                _audioModels.Add(sub, audioModel);
            }
            
            foreach (var sub in subclasses)
            {
                var oneShotPlayerGameObject = new GameObject($"{nameof(OneShotAudioPlayer)}:{sub.Name}");
                oneShotPlayerGameObject.transform.SetParent(transform);
                
                var oneShotPlayer = oneShotPlayerGameObject.AddComponent<OneShotAudioPlayer>();
                oneShotPlayer.Initialize(_audioModels[sub], GetGlobalAudioModel());
                _oneShotAudioPlayers.Add(sub, oneShotPlayer);
            }
        }
        
        public AudioPlayer InstantiateAudioPlayer<T>(AudioClip audioClip) where T : AudioModel
        {
            var playerGameObject = new GameObject($"{nameof(AudioPlayer)}/{audioClip.name}");
            playerGameObject.transform.SetParent(transform);
            
            var playerInstance = playerGameObject.AddComponent<AudioPlayer>();
            playerInstance.Initialize(audioClip, _audioModels[typeof(T)], GetGlobalAudioModel());
            return playerInstance;
        }

        public AudioPlayResult PlayOneShot<T>(AudioClip sfxClip) where T : AudioModel
        {
            return _oneShotAudioPlayers[typeof(T)].Play(sfxClip);
        }
        
        public GlobalAudio GetGlobalAudioModel()
        {
            return GetAudioModel<GlobalAudio>();
        }
        
        public T GetAudioModel<T>() where T : AudioModel
        {
            return (T)_audioModels[typeof(T)];
        }
    }   
}
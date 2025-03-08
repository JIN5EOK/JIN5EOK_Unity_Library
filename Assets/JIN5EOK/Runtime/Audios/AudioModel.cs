using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jin5eok.Audios
{    
    public enum SoundType
    {
        Bgm,
        Sfx,
    }
    
    public class AudioModel
    {
        private static AudioModel _instance;

        public static AudioModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioModel();
                }

                return _instance;
            }
        }

        public event Action<float> OnBgmVolumeChanged;
        public event Action<float> OnSfxVolumeChanged;
        
        private float _bgmVolume;
        private float _sfxVolume;
        
        public float BgmVolume
        {
            get => _bgmVolume;
            set
            {
                _bgmVolume = Mathf.Clamp01(value);
                OnBgmVolumeChanged?.Invoke(_bgmVolume);
            }   
        }

        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                OnSfxVolumeChanged?.Invoke(_sfxVolume);
            }   
        }
        
        public Dictionary<string, AudioClip> Clips { get; private set; } = new ();
        
        public AudioClip GetAudioClipResources(string resourcePath)
        {
            if (Clips.ContainsKey(resourcePath) == false)
            {
                var clip = Resources.Load<AudioClip>(resourcePath);
                Clips.Add(resourcePath, clip);
            }
            return Clips[resourcePath];
        }
#if USE_ADDRESSABLES
        public AudioClip GetAudioClipAddressables(string address)
        {
            if (Clips.ContainsKey(address) == false)
            {
                var clip = Addressables.LoadAssetAsync<AudioClip>(address).WaitForCompletion();
                Clips.Add(address, clip);
            }

            return Clips[address];
        }
    }
#endif
}
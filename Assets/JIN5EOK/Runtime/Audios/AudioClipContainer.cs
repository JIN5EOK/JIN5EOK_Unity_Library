using System.Collections.Generic;
using UnityEngine;
#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace Jin5eok.Audios
{
    public class AudioClipContainer
    {
        private static readonly Dictionary<string, AudioClip> _audioClipMap = new();

        public static AudioClip GetFromResources(string resourcePath, bool isCache = true)
        {
            AudioClip clip = null;
            if (_audioClipMap.TryGetValue(resourcePath, out clip) == false)
            {
                clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null && isCache == true)
                {
                    _audioClipMap.Add(resourcePath, clip);
                }
            }
            return clip;
        }

#if USE_ADDRESSABLES
        public static AudioClip GetFromAddressables(string address, bool isCache = true)
        {
            AudioClip clip = null;
            if (_audioClipMap.TryGetValue(address, out clip) == false)
            {
                clip = Addressables.LoadAssetAsync<AudioClip>(address).WaitForCompletion();
                if (clip != null && isCache == true)
                {
                    _audioClipMap.Add(address, clip);
                }
            }
            return _audioClipMap[address];
        }
#endif
        
        public static int GetCachedCount()
        {
            return _audioClipMap.Count;
        }
        
        public static void ClearCachedClips()
        {
            _audioClipMap.Clear();
        }
    }
}
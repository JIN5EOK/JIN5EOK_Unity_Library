using System.Collections.Generic;
using UnityEngine;
#if USE_ADDRESSABLES
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
#endif

namespace Jin5eok.Audios
{
    public class AudioClipContainer
    {
        private static readonly Dictionary<string, AudioClip> _resourcesAudioClips = new();
        
        public static AudioClip GetFromResources(string resourcePath)
        {
            AudioClip clip = null;
            if (_resourcesAudioClips.TryGetValue(resourcePath, out clip) == false)
            {
                clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    _resourcesAudioClips.Add(resourcePath, clip);
                }
            }
            return clip;
        }
        
        public async static Task<AudioClip> GetFromResourcesAsync(string resourcePath)
        {
            AudioClip clip = null;
            if (_resourcesAudioClips.TryGetValue(resourcePath, out clip) == false)
            {
                var request = Resources.LoadAsync<AudioClip>(resourcePath);
                while (request.isDone == false)
                {
                    await Task.Yield();
                }
                clip = request.asset as AudioClip;
                if (clip != null)
                {
                    _resourcesAudioClips.Add(resourcePath, clip);
                }
            }
            return clip;
        }
        
        
#if USE_ADDRESSABLES

#endif
        
        public static int GetCachedResourcesCount()
        {
            return _resourcesAudioClips.Count;
        }
        
        public static void ClearResourcesCache()
        {
            foreach (var clip in _resourcesAudioClips.Values)
            {
                Resources.UnloadAsset(clip);
            }
            _resourcesAudioClips.Clear();
        }
        
#if USE_ADDRESSABLES
        private static readonly Dictionary<string, AudioClip> _addressablesAudioClips = new();

        public static AudioClip GetFromAddressables(string address)
        {
            AudioClip clip = null;
            if (_resourcesAudioClips.TryGetValue(address, out clip) == false)
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(address);
                clip = handle.WaitForCompletion();
                if (clip != null)
                {
                    _resourcesAudioClips.Add(address, clip);
                }
                else
                {
                    Addressables.Release(handle);
                }
            }
            else
            {
                clip = _resourcesAudioClips[address];
            }
            return clip;
        }
        
        public async static Task<AudioClip> GetFromAddressablesAsync(string address)
        {
            AudioClip clip = null;
            if (_resourcesAudioClips.TryGetValue(address, out clip) == false)
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(address);
                clip = await handle.Task;
                if (clip != null)
                {
                    _resourcesAudioClips.Add(address, clip);
                }
                else
                {
                    Addressables.Release(handle);
                }
            }
            else
            {
                clip = _resourcesAudioClips[address];
            }
            return clip;
        }
        
        public static int GetCachedAddressablesCount()
        {
            return _addressablesAudioClips.Count;
        }

        public static void ClearAddressablesCache()
        {
            foreach (var clip in _addressablesAudioClips.Values)
            {
                Addressables.Release(clip);
            }
            _addressablesAudioClips.Clear();
        }
#endif

    }
}
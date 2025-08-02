using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Jin5eok
{
    public static class UnityWebRequestHelper
    {
        public static void Get(string url, Action<string> onSuccess, Action<UnityWebRequestException> onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequest.Get(url), onSuccess, onError, r => r.downloadHandler.text));
        }
        
        public static void GetTexture(string url, Action<Texture2D> onSuccess, Action<UnityWebRequestException> onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequestTexture.GetTexture(url), onSuccess, onError, DownloadHandlerTexture.GetContent));
        }
        
        public static void GetAudioClip(string url, AudioType audioType, Action<AudioClip> onSuccess, Action<UnityWebRequestException> onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequestMultimedia.GetAudioClip(url, audioType), onSuccess, onError, DownloadHandlerAudioClip.GetContent));
        }

        public static void GetAssetBundle(string url, Action<AssetBundle> onSuccess, Action<UnityWebRequestException> onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequestAssetBundle.GetAssetBundle(url), onSuccess, onError, DownloadHandlerAssetBundle.GetContent));
        }
        
        private static IEnumerator RequestRoutine<T>(UnityWebRequest request, Action<T> onSuccess, Action<UnityWebRequestException> onError, Func<UnityWebRequest, T> contentExtractor)
        {
            using (request)
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(contentExtractor(request));
                }
                else
                {
                    onError?.Invoke(new UnityWebRequestException(request));
                }
            }
        }
        
        public static Task<string> GetAsync(string url)
        {
            return RequestAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text);
        }
        
        public static Task<Texture2D> GetTextureAsync(string url)
        {
            return RequestAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent);
        }
        
        public static Task<AudioClip> GetAudioClipAsync(string url, AudioType audioType)
        {
            return RequestAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent);
        }

        public static Task<AssetBundle> GetAssetBundleAsync(string url)
        {
            return RequestAsync(UnityWebRequestAssetBundle.GetAssetBundle(url), DownloadHandlerAssetBundle.GetContent);
        }
        
        private static async Task<T> RequestAsync<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor)
        {
            using (request)
            {
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success) 
                    throw new UnityWebRequestException(request);
                return contentExtractor(request);
            }
        }
        
#if USE_UNITASK
        public static UniTask<string> GetUniTaskAsync(string url)
        {
            return RequestUniTaskAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text);
        }
        
        public static UniTask<Texture2D> GetTextureUniTaskAsync(string url)
        {
            return RequestUniTaskAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent);
        }
        
        public static UniTask<AudioClip> GetAudioClipUniTaskAsync(string url, AudioType audioType)
        {
            return RequestUniTaskAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent);
        }

        public static UniTask<AssetBundle> GetAssetBundleUniTaskAsync(string url)
        {
            return RequestUniTaskAsync(UnityWebRequestAssetBundle.GetAssetBundle(url), DownloadHandlerAssetBundle.GetContent);
        }
        
        private static async UniTask<T> RequestUniTaskAsync<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor)
        {
            using (request)
            {
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success) 
                    throw new UnityWebRequestException(request);
                return contentExtractor(request);
            }
        }
#endif

#if USE_AWAITABLE
        public static Awaitable<string> GetAwaitableAsync(string url)
        {
            return RequestAwaitableAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text);
        }
        
        public static Awaitable<Texture2D> GetTextureAwaitableAsync(string url)
        {
            return RequestAwaitableAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent);
        }
        
        public static Awaitable<AudioClip> GetAudioClipAwaitableAsync(string url, AudioType audioType)
        {
            return RequestAwaitableAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent);
        }

        public static Awaitable<AssetBundle> GetAssetBundleAwaitableAsync(string url)
        {
            return RequestAwaitableAsync(UnityWebRequestAssetBundle.GetAssetBundle(url), DownloadHandlerAssetBundle.GetContent);
        }
        
        private static async Awaitable<T> RequestAwaitableAsync<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor)
        {
            using (request)
            {
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                    throw new UnityWebRequestException(request);
                return contentExtractor(request);
            }
        }
#endif
    }
}
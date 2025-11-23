using System;
using System.Collections;
using System.Threading;
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
        public static void Get(string url, Action<string> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequest.Get(url), onSuccess, onError, r => r.downloadHandler.text, cancellationToken));
        }
        
        public static void GetTexture(string url, Action<Texture2D> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequestTexture.GetTexture(url), onSuccess, onError, DownloadHandlerTexture.GetContent, cancellationToken));
        }
        
        public static void GetAudioClip(string url, AudioType audioType, Action<AudioClip> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequestMultimedia.GetAudioClip(url, audioType), onSuccess, onError, DownloadHandlerAudioClip.GetContent, cancellationToken));
        }

        public static void GetAssetBundle(string url, Action<AssetBundle> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequestAssetBundle.GetAssetBundle(url), onSuccess, onError, DownloadHandlerAssetBundle.GetContent, cancellationToken));
        }
        
        private static IEnumerator RequestRoutine<T>(UnityWebRequest request, Action<T> onSuccess, Action<UnityWebRequestException> onError, Func<UnityWebRequest, T> contentExtractor, CancellationToken cancellationToken = default)
        {
            using (request)
            {
                var asyncOperation = request.SendWebRequest();

                while (!asyncOperation.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        request.Abort();
                        onError?.Invoke(new UnityWebRequestException(request));
                        yield break;
                    }

                    yield return null;
                }

                try
                {
                    var result = ProcessUnityWebRequestResult(request, contentExtractor);
                    onSuccess?.Invoke(result);
                }
                catch (UnityWebRequestException e)
                {
                    onError?.Invoke(e);
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
                return ProcessUnityWebRequestResult(request, contentExtractor);
            }
        }
        
        private static T ProcessUnityWebRequestResult<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor)
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new UnityWebRequestException(request);
            }
            return contentExtractor(request);
        }
    }
}
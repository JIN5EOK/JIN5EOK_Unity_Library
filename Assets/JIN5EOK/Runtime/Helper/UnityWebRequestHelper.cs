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
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequest.Get(url), onSuccess, onError, r => r.downloadHandler.text, cancellationToken));
        }
        
        public static void GetTexture(string url, Action<Texture2D> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequestTexture.GetTexture(url), onSuccess, onError, DownloadHandlerTexture.GetContent, cancellationToken));
        }
        
        public static void GetAudioClip(string url, AudioType audioType, Action<AudioClip> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequestMultimedia.GetAudioClip(url, audioType), onSuccess, onError, DownloadHandlerAudioClip.GetContent, cancellationToken));
        }

        public static void GetAssetBundle(string url, Action<AssetBundle> onSuccess, Action<UnityWebRequestException> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineRunner.Instance.StartCoroutine(RequestRoutine(UnityWebRequestAssetBundle.GetAssetBundle(url), onSuccess, onError, DownloadHandlerAssetBundle.GetContent, cancellationToken));
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
        public static Task<string> GetAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text, cancellationToken);
        }
        
        public static Task<Texture2D> GetTextureAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent, cancellationToken);
        }
        
        public static Task<AudioClip> GetAudioClipAsync(string url, AudioType audioType, CancellationToken cancellationToken = default)
        {
            return RequestAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent, cancellationToken);
        }

        public static Task<AssetBundle> GetAssetBundleAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestAsync(UnityWebRequestAssetBundle.GetAssetBundle(url), DownloadHandlerAssetBundle.GetContent, cancellationToken);
        }
        
        private static async Task<T> RequestAsync<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor, CancellationToken cancellationToken = default)
        {
            using (request)
            {
                await using (cancellationToken.Register(() => request?.Abort()))
                {
                    await request.SendWebRequest();
                    return ProcessUnityWebRequestResult(request, contentExtractor);
                }
            }
        }
        
#if USE_UNITASK
        public static UniTask<string> GetUniTaskAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestUniTaskAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text, cancellationToken);
        }
        
        public static UniTask<Texture2D> GetTextureUniTaskAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestUniTaskAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent, cancellationToken);
        }
        
        public static UniTask<AudioClip> GetAudioClipUniTaskAsync(string url, AudioType audioType, CancellationToken cancellationToken = default)
        {
            return RequestUniTaskAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent, cancellationToken);
        }

        public static UniTask<AssetBundle> GetAssetBundleUniTaskAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestUniTaskAsync(UnityWebRequestAssetBundle.GetAssetBundle(url), DownloadHandlerAssetBundle.GetContent,cancellationToken);
        }
        
        private static async UniTask<T> RequestUniTaskAsync<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor, CancellationToken cancellationToken = default)
        {
            using (request)
            {
                await request.SendWebRequest().WithCancellation(cancellationToken);
                return ProcessUnityWebRequestResult(request, contentExtractor);
            }
        }
#endif

#if USE_AWAITABLE
        public static Awaitable<string> GetAwaitableAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestAwaitableAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text, cancellationToken);
        }
        
        public static Awaitable<Texture2D> GetTextureAwaitableAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestAwaitableAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent, cancellationToken);
        }
        
        public static Awaitable<AudioClip> GetAudioClipAwaitableAsync(string url, AudioType audioType, CancellationToken cancellationToken = default)
        {
            return RequestAwaitableAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent, cancellationToken);
        }

        public static Awaitable<AssetBundle> GetAssetBundleAwaitableAsync(string url, CancellationToken cancellationToken = default)
        {
            return RequestAwaitableAsync(UnityWebRequestAssetBundle.GetAssetBundle(url), DownloadHandlerAssetBundle.GetContent, cancellationToken);
        }
        
        private static async Awaitable<T> RequestAwaitableAsync<T>(UnityWebRequest request, Func<UnityWebRequest, T> contentExtractor, CancellationToken cancellationToken = default)
        {
            using (request)
            {
                using (cancellationToken.Register(() => request?.Abort()))
                {
                    await request.SendWebRequest();
                    return ProcessUnityWebRequestResult(request, contentExtractor);    
                }
            }
        }
#endif
        
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
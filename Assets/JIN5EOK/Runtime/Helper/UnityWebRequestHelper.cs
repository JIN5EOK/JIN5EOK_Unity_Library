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
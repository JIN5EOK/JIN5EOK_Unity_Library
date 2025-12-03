using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Jin5eok
{
    /// <summary>
    /// UnityWebRequest를 사용한 네트워크 요청을 간편하게 수행하기 위한 헬퍼 클래스입니다.
    /// 코루틴 기반과 비동기 메서드를 모두 제공합니다.
    /// </summary>
    public static class UnityWebRequestHelper
    {
        /// <summary>
        /// GET 요청을 통해 문자열 데이터를 가져옵니다. (코루틴 기반)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <param name="onSuccess">성공 시 호출될 콜백</param>
        /// <param name="onError">실패 시 호출될 콜백</param>
        /// <param name="cancellationToken">취소 토큰</param>
        public static void Get(string url, Action<string> onSuccess, Action<Exception> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequest.Get(url), onSuccess, onError, r => r.downloadHandler.text, cancellationToken));
        }

        /// <summary>
        /// GET 요청을 통해 텍스처를 가져옵니다. (코루틴 기반)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <param name="onSuccess">성공 시 호출될 콜백</param>
        /// <param name="onError">실패 시 호출될 콜백</param>
        /// <param name="cancellationToken">취소 토큰</param>
        public static void GetTexture(string url, Action<Texture2D> onSuccess, Action<Exception> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequestTexture.GetTexture(url), onSuccess, onError, DownloadHandlerTexture.GetContent, cancellationToken));
        }

        /// <summary>
        /// GET 요청을 통해 오디오 클립을 가져옵니다. (코루틴 기반)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <param name="audioType">오디오 타입</param>
        /// <param name="onSuccess">성공 시 호출될 콜백</param>
        /// <param name="onError">실패 시 호출될 콜백</param>
        /// <param name="cancellationToken">취소 토큰</param>
        public static void GetAudioClip(string url, AudioType audioType, Action<AudioClip> onSuccess, Action<Exception> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequestMultimedia.GetAudioClip(url, audioType), onSuccess, onError, DownloadHandlerAudioClip.GetContent, cancellationToken));
        }

        /// <summary>
        /// GET 요청을 통해 에셋 번들을 가져옵니다. (코루틴 기반)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <param name="onSuccess">성공 시 호출될 콜백</param>
        /// <param name="onError">실패 시 호출될 콜백</param>
        /// <param name="cancellationToken">취소 토큰</param>
        public static void GetAssetBundle(string url, Action<AssetBundle> onSuccess, Action<Exception> onError = null, CancellationToken cancellationToken = default)
        {
            CoroutineManager.Instance.StartCoroutine(RequestRoutine(UnityWebRequestAssetBundle.GetAssetBundle(url), onSuccess, onError, DownloadHandlerAssetBundle.GetContent, cancellationToken));
        }

        private static IEnumerator RequestRoutine<T>(UnityWebRequest request, Action<T> onSuccess, Action<Exception> onError, Func<UnityWebRequest, T> contentExtractor, CancellationToken cancellationToken = default)
        {
            using (request)
            {
                var asyncOperation = request.SendWebRequest();

                while (!asyncOperation.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        request.Abort();
                        onError?.Invoke(new OperationCanceledException("Request was cancelled"));
                        yield break;
                    }

                    yield return null;
                }

                try
                {
                    var result = ProcessUnityWebRequestResult(request, contentExtractor);
                    onSuccess?.Invoke(result);
                }
                catch (Exception e)
                {
                    onError?.Invoke(e);
                }
            }
        }

        /// <summary>
        /// GET 요청을 통해 문자열 데이터를 가져옵니다. (비동기)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <returns>문자열 데이터를 반환하는 Task</returns>
        public static Task<string> GetAsync(string url)
        {
            return RequestAsync(UnityWebRequest.Get(url), r => r.downloadHandler.text);
        }

        /// <summary>
        /// GET 요청을 통해 텍스처를 가져옵니다. (비동기)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <returns>텍스처를 반환하는 Task</returns>
        public static Task<Texture2D> GetTextureAsync(string url)
        {
            return RequestAsync(UnityWebRequestTexture.GetTexture(url), DownloadHandlerTexture.GetContent);
        }

        /// <summary>
        /// GET 요청을 통해 오디오 클립을 가져옵니다. (비동기)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <param name="audioType">오디오 타입</param>
        /// <returns>오디오 클립을 반환하는 Task</returns>
        public static Task<AudioClip> GetAudioClipAsync(string url, AudioType audioType)
        {
            return RequestAsync(UnityWebRequestMultimedia.GetAudioClip(url, audioType), DownloadHandlerAudioClip.GetContent);
        }

        /// <summary>
        /// GET 요청을 통해 에셋 번들을 가져옵니다. (비동기)
        /// </summary>
        /// <param name="url">요청할 URL</param>
        /// <returns>에셋 번들을 반환하는 Task</returns>
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
                throw new InvalidOperationException($"UnityWebRequest failed: {request.url}\nError: {request.error}\nResponse Code: {request.responseCode}");
            }
            return contentExtractor(request);
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Jin5eok
{
    public static class UnityWebRequestHelper
    {
        public static void Get(string url, Action<string> onSuccess, Action<string> onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(GetRoutine(url, onSuccess, onError));
        }
#if USE_UNITASK
        public static async UniTask<string> GetAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }
                else
                {
                    return null;
                }
            }
        }
#endif
        private static IEnumerator GetRoutine(string url, Action<string> onSuccess, Action<string> onError = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke(request.error);
                }
            }
        }

        public static void GetTexture(string url, Action<Texture2D> onSuccess, Action onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(GetTextureRoutine(url, onSuccess, onError));
        }
#if USE_UNITASK
        public static async UniTask<Texture2D> GetTextureAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return DownloadHandlerTexture.GetContent(request);    
                }
                else
                {
                    return null;
                }
            }
        }
#endif
        private static IEnumerator GetTextureRoutine(string url, Action<Texture2D> onSuccess, Action onError = null)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(DownloadHandlerTexture.GetContent(request));
                }
                else
                {
                    onError?.Invoke();
                }
            }
        }
    }
}
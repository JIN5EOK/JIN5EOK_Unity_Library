using System;
using System.Collections;
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
            CoroutineRunner.Instance.StartCoroutine(GetRoutine(url, onSuccess, onError));
        }
        
        private static IEnumerator GetRoutine(string url, Action<string> onSuccess, Action<UnityWebRequestException> onError = null)
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
                    onError?.Invoke(new UnityWebRequestException(request));
                }
            }
        }
        
#if USE_UNITASK
        public static async UniTask<string> GetUniTaskAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest(); 
                return request.downloadHandler.text;
            }
        }
#endif

        public static void GetTexture(string url, Action<Texture2D> onSuccess, Action<UnityWebRequestException> onError = null)
        {
            CoroutineRunner.Instance.StartCoroutine(GetTextureRoutine(url, onSuccess, onError));
        }
        
        private static IEnumerator GetTextureRoutine(string url, Action<Texture2D> onSuccess, Action<UnityWebRequestException> onError = null)
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
                    onError?.Invoke(new UnityWebRequestException(request));
                }
            }
        }
        
#if USE_UNITASK
        public static async UniTask<Texture2D> GetTextureAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                await request.SendWebRequest(); 
                return DownloadHandlerTexture.GetContent(request);
            }
        }
#endif
    }
}
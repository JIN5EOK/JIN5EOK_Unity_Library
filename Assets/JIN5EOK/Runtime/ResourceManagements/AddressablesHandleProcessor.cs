#if USE_ADDRESSABLES
using System;
using System.Collections;
using System.Threading.Tasks;
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Jin5eok.Patterns;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Jin5eok.ResourceManagements
{
    internal class AddressablesHandleProcessor : MonoSingleton<AddressablesHandleProcessor>
    {
        public static void ProcessAsyncCoroutine<T>(AsyncOperationHandle<T> handle, Action<T> onResult) where T : Object
        {
            if (handle.IsValid() == false || handle.Status == AsyncOperationStatus.Failed)
            {
                onResult?.Invoke(null);
            }
            else if (handle.IsDone && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                onResult?.Invoke(handle.Result);
            }
            else
            {
                Instance.StartCoroutine(ProcessAsyncCoroutineInternal(handle, onResult));
            }
        }

        private static IEnumerator ProcessAsyncCoroutineInternal<T>(AsyncOperationHandle<T> handle, Action<T> onResult) where T : Object
        {
            yield return new WaitUntil(() => handle.IsDone);
            onResult?.Invoke(handle.Result);
        }
        
#if USE_UNITASK
        public static async UniTask<T> ProcessAsyncUniTask<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            if (handle.IsValid() == false || handle.Status == AsyncOperationStatus.Failed)
            {
                return null;
            }
            
            if (handle.IsDone && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                return handle.Result;
            }

            await UniTask.SwitchToMainThread();
            await handle.Task;
            return handle.Result;
        }
#endif
        
#if USE_AWAITABLE
        public static async Awaitable<T> ProcessAsyncAwaitable<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            if (handle.IsValid() == false || handle.Status == AsyncOperationStatus.Failed)
            {
                return null;
            }
            
            if (handle.IsDone && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                return handle.Result;
            }
            
            await Awaitable.MainThreadAsync();
            await handle.Task;
            return handle.Result;
        }
#endif
        
        public static async Task<T> ProcessAsyncTask<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            if (handle.IsValid() == false || handle.Status == AsyncOperationStatus.Failed)
            {
                return null;
            }
            
            if (handle.IsDone && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                return handle.Result;
            }
            
            await handle.Task;
            return handle.Result;
        }
    }
}
#endif
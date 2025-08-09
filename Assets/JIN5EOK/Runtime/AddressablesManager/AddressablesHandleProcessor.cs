#if USE_ADDRESSABLES
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Jin5eok
{
    internal static class AddressablesHandleProcessor
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
                CoroutineManager.WaitUntil(() => handle.IsDone, () => onResult?.Invoke(handle.Result));
            }
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
            
            await handle.Task;
            return handle.Result;
        }
#endif
    }
}
#endif
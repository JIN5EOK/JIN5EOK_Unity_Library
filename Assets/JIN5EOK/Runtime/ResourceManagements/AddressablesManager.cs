#if USE_ADDRESSABLES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Jin5eok.ResourceManagements
{
    public class AddressablesManager
    {
        private static class AsyncOperationHandleMap<T> where T : Object
        {
            public static Dictionary<string, AsyncOperationHandle<T>> AddressToHandleMap { get; private set; } = new ();
        }
        private static object _lock = new ();
        
        public static T LoadSync<T>(string address) where T : Object => GetHandle<T>(address).WaitForCompletion();
        
        public static void LoadAsyncCoroutine<T>(string address, Action<T> onResult) where T : Object 
        {
            var handle = GetHandle<T>(address);
            AddressablesHandleProcessor.ProcessAsyncCoroutine(handle, onResult);
        }
        
#if USE_UNITASK
        public static async UniTask<T> LoadAsyncUniTask<T>(string address) where T : Object
        {
            var handle = GetHandle<T>(address);
            return await AddressablesHandleProcessor.ProcessAsyncUniTask(handle);
        }
#endif
        
#if USE_AWAITABLE
        public static async Awaitable<T> LoadAsyncAwaitable<T>(string address) where T : Object
        {
            var handle = GetHandle<T>(address);
            return await AddressablesHandleProcessor.ProcessAsyncAwaitable(handle);
        }
#endif
        
        public static async Task<T> LoadAsyncTask<T>(string address) where T : Object
        {
            var handle = GetHandle<T>(address);
            return await AddressablesHandleProcessor.ProcessAsyncTask(handle);
        }
        
        private static AsyncOperationHandle<T> GetHandle<T>(string address) where T : Object
        {
            lock (_lock)
            {
                if(AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var handle) == true)
                {
                    if(handle.IsValid() == true && handle.Status != AsyncOperationStatus.Failed)
                    {
                        return handle;
                    }
                    else
                    {
                        AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);    
                    }
                }
                
                handle = Addressables.LoadAssetAsync<T>(address);
                AsyncOperationHandleMap<T>.AddressToHandleMap.Add(address, handle);
                
                return handle;
            }
        }
        
        public static bool Release<T>(string address) where T : Object
        {
            lock (_lock)
            {
                if (AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var handle) == true)
                {
                    if (handle.IsValid() == true && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
                    {
                        Addressables.Release(handle);    
                    }
                    
                    AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                    return true;
                }

                return false;
            }
        }
        
        public static void ReleaseAll<T>() where T : Object
        {
            lock (_lock)
            {
                var addresses = AsyncOperationHandleMap<T>.AddressToHandleMap.Keys.ToArray();
                foreach (var address in addresses)
                {
                    Release<T>(address);
                }
            }
        }

        public static bool IsLoaded<T> (string address) where T : Object
        {
            lock (_lock)
            {
                return AsyncOperationHandleMap<T>.AddressToHandleMap.ContainsKey(address);
            }
        }

        public static int GetLoadedTypeCount<T>() where T : Object
        {
            lock (_lock)
            {
                return AsyncOperationHandleMap<T>.AddressToHandleMap.Values.Count;    
            }
        }
    }
}
#endif

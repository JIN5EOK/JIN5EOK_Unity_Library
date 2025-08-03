#if USE_ADDRESSABLES
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public static AsyncOperationHandle<T> LoadHandle<T>(string address) where T : Object
        {
            AsyncOperationHandle<T> handle;

            lock (_lock)
            {
                if (AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out handle) == true)
                {
                    if (handle.IsValid() == true && handle.Status != AsyncOperationStatus.Failed)
                    {
                        return handle;
                    }
                    else
                    {
                        Debug.LogWarning($"{nameof(AddressablesManager)} Instantiation failed for address: {address}");
                        AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                    }
                }
            }
            
            handle = Addressables.LoadAssetAsync<T>(address);

            lock (_lock)
            {
                if (AsyncOperationHandleMap<T>.AddressToHandleMap.ContainsKey(address) == false)
                {
                    AsyncOperationHandleMap<T>.AddressToHandleMap.Add(address, handle);
                }
            }
                
            handle.Completed += (completedHandle) =>
            {
                // 실패한 경우에만 딕셔너리에서 자동 제거
                if (completedHandle.Status == AsyncOperationStatus.Failed)
                {
                    lock (_lock)
                    {
                        AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                    }
                }
            };
            handle.Destroyed += _ => OnRelease<T>(address);
            
            return handle;
        }
        
        private static void OnRelease<T>(string address) where T : Object
        {
            lock (_lock)
            {
                AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
            }
        }
        
        public static List<AsyncOperationHandle<T>> GetLoadedHandleAll<T>() where T : Object
        {
            lock (_lock)
            {
                return AsyncOperationHandleMap<T>.AddressToHandleMap.Values.ToList();    
            }
        }
        
        public static T LoadWaitForCompletion<T>(string address) where T : Object => LoadHandle<T>(address).WaitForCompletion();
        
        public static void LoadAssetCoroutine<T>(string address, Action<T> onResult) where T : Object 
        {
            var handle = LoadHandle<T>(address);
            AddressablesHandleProcessor.ProcessAsyncCoroutine(handle, onResult);
        }
        
#if USE_UNITASK
        public static async UniTask<T> LoadAssetUniTask<T>(string address) where T : Object
        {
            var handle = LoadHandle<T>(address);
            return await AddressablesHandleProcessor.ProcessAsyncUniTask(handle);
        }
#endif
        
#if USE_AWAITABLE
        public static async Awaitable<T> LoadAssetAwaitable<T>(string address) where T : Object
        {
            var handle = LoadHandle<T>(address);
            return await AddressablesHandleProcessor.ProcessAsyncAwaitable(handle);
        }
#endif
        
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
                return AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var handle) &&
                       handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded;
            }
        }

        public static List<string> GetLoadedAddressAll<T>() where T : Object
        {
            lock (_lock)
            {
                return AsyncOperationHandleMap<T>.AddressToHandleMap.Keys.ToList();    
            }
        }
    }
}
#endif

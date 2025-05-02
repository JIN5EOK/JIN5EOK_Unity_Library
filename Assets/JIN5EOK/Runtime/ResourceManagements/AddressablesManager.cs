#if USE_ADDRESSABLES
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Jin5eok.ResourceManagements
{
    public class AddressablesManager
    {
        private static class AsyncOperationHandleMap<T> where T : Object
        {
            public static Dictionary<AsyncOperationHandle<T>, string> HandleToAddressMap { get; private set; } = new ();
            public static Dictionary<string, AsyncOperationHandle<T>> AddressToHandleMap { get; private set; } = new ();
        }
        private static object _lock = new ();
        
        public static T LoadAsset<T>(string address) where T : Object => LoadAssetAsync<T>(address).WaitForCompletion();
        
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string address) where T : Object
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
                        AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);    
                    }
                }
                
                handle = Addressables.LoadAssetAsync<T>(address);
                handle.Destroyed += _ =>
                {
                    lock (_lock)
                    {
                        if (AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var currentHandle) == true
                            && currentHandle.Equals(handle) == true)
                        {
                            AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                            AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);    
                        }
                    }
                };
                AsyncOperationHandleMap<T>.AddressToHandleMap.Add(address, handle);
                AsyncOperationHandleMap<T>.HandleToAddressMap.Add(handle, address);
                
                return handle;
            }
        }
        
        public static bool ReleaseAsset<T>(string address) where T : Object
        {
            lock (_lock)
            {
                if (AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var handle) == true)
                {
                    return ReleaseAssetInternal(handle);
                }
                return false;
            }
        }
        
        public static bool ReleaseAsset<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            return ReleaseAssetInternal(handle);
        }

        public static void ReleaseAssetAll<T>() where T : Object
        {
            lock (_lock)
            {
                var handles = AsyncOperationHandleMap<T>.AddressToHandleMap.Values.ToArray();
                foreach (var handle in handles)
                {
                    ReleaseAssetInternal(handle);
                }
            }
        }
        
        private static bool ReleaseAssetInternal<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            lock (_lock)
            {
                if (AsyncOperationHandleMap<T>.HandleToAddressMap.TryGetValue(handle, out var address) == true)
                {
                    if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
                    {
                        Addressables.Release(handle);    
                    }
                
                    AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                    AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);
                    return true;
                }

                return false;
            }
        }
        
        public static int GetHandleCount<T>() where T : Object
        {
            lock (_lock)
            {
                return AsyncOperationHandleMap<T>.AddressToHandleMap.Values.Count;    
            }
        }
    }
}
#endif

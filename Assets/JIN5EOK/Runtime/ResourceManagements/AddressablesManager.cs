#if USE_ADDRESSABLES
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
        
        public static T LoadAsset<T>(string address) where T : Object => LoadAssetAsync<T>(address).WaitForCompletion();
        
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string address) where T : Object
        {
            if (AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var handle) == false || handle.IsValid() == false)
            {
                if (handle.IsValid() == false)
                {
                    AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                    AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);
                }
                
                handle = Addressables.LoadAssetAsync<T>(address);
                AsyncOperationHandleMap<T>.AddressToHandleMap.Add(address, handle);
                AsyncOperationHandleMap<T>.HandleToAddressMap.Add(handle, address);
            }
            else
            {
                handle = AsyncOperationHandleMap<T>.AddressToHandleMap[address];
            }
            return handle;
        }

        public static bool ReleaseAsset<T>(string address) where T : Object
        {
            if (AsyncOperationHandleMap<T>.AddressToHandleMap.TryGetValue(address, out var handle) == true)
            {
                if (handle.IsValid() == true)
                {
                    Addressables.Release(handle);    
                }
                AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);
                return true;
            }
            
            return false;
        }
        
        public static bool ReleaseAsset<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            if (AsyncOperationHandleMap<T>.HandleToAddressMap.TryGetValue(handle, out var address) == true)
            {
                if (handle.IsValid() == true)
                {
                    Addressables.Release(handle);    
                }
                
                AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);
                return true;
            }
            return false;
        }
        
        public static void ReleaseAssetAll<T>() where T : Object
        {
            foreach (var handle in AsyncOperationHandleMap<T>.AddressToHandleMap.Values)
            {
                if (handle.IsValid() == true)
                {
                    Addressables.Release(handle);    
                }
            }
            AsyncOperationHandleMap<T>.AddressToHandleMap.Clear();
            AsyncOperationHandleMap<T>.HandleToAddressMap.Clear();
        }
        
        public static int GetHandleCount<T>() where T : Object
        {
            ClearInvalidHandles<T>();
            return AsyncOperationHandleMap<T>.AddressToHandleMap.Values.Count;
        }

        private static void ClearInvalidHandles<T>() where T : Object
        {
            var handles = AsyncOperationHandleMap<T>.AddressToHandleMap.Values.ToArray();
            foreach (var handle in handles)
            {
                if (handle.IsValid() == false)
                {
                    var address = AsyncOperationHandleMap<T>.HandleToAddressMap[handle];
                    AsyncOperationHandleMap<T>.AddressToHandleMap.Remove(address);
                    AsyncOperationHandleMap<T>.HandleToAddressMap.Remove(handle);
                }
            }
        }
    }
}
#endif

#if USE_ADDRESSABLES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Jin5eok.ResourceManagements
{
    public class AddressablesInstanceManager
    {
        private static Dictionary<string, List<AsyncOperationHandle<GameObject>>> AddressToInstanceCollectionMap { get; set; } = new ();
     
        public static AsyncOperationHandle<GameObject> LoadHandle(string address)
        {
            if (AddressToInstanceCollectionMap.TryGetValue(address, out var handles) == false)
            {
                AddressToInstanceCollectionMap.Add(address, new List<AsyncOperationHandle<GameObject>>());
                handles = AddressToInstanceCollectionMap[address];
            }
            
            var handle = Addressables.InstantiateAsync(address);
            handle.Completed += _ =>
            {
                if (handle.Result != null)
                {
                    handle.Result.AddComponent<AddressablesInstanceReleaser>().Initialize(handle);
                }
            };

            handle.Destroyed += _ =>
            {
                handles.Remove(handle);
            };
            handles.Add(handle);
            
            return handle;
        }
        
        public static List<AsyncOperationHandle<GameObject>> GetLoadedHandleAll(string address)
        {
            if (AddressToInstanceCollectionMap.TryGetValue(address, out var list))
            {
                return list;
            }
            return null;
        }
        
        public static GameObject InstantiateWaitForCompletion(string address) => LoadHandle(address).WaitForCompletion();
        
        public static void InstantiateAsyncCoroutine(string address, Action<GameObject> onResult) 
        {
            var handle = LoadHandle(address);
            AddressablesHandleProcessor.ProcessAsyncCoroutine(handle, onResult);
        }
        
#if USE_UNITASK
        public static async UniTask<GameObject> InstantiateAsyncUniTask(string address)
        {
            var handle = LoadHandle(address);
            return await AddressablesHandleProcessor.ProcessAsyncUniTask(handle);
        }
#endif
        
#if USE_AWAITABLE
        public static async Awaitable<GameObject> InstantiateAsyncAwaitable(string address)
        {
            var handle = LoadHandle(address);
            return await AddressablesHandleProcessor.ProcessAsyncAwaitable(handle);
        }
#endif
        
        public static void ReleaseInstances(string address)
        {
            if (AddressToInstanceCollectionMap.TryGetValue(address, out var list))
            {
                foreach (var handle in AddressToInstanceCollectionMap[address])
                {
                    if (handle.IsValid() == true && handle.Status == AsyncOperationStatus.Succeeded &&
                        handle.Result != null)
                    {
                        Object.Destroy(handle.Result);
                    }
                }
                list.Clear();
            }
        }
        
        public static void ReleaseInstanceHandleAll()
        {
            foreach (var list in AddressToInstanceCollectionMap.Values)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var handle = list[i];
                    handle.Release();
                    list.Remove(handle);
                }
            }
        }
    }
}
#endif
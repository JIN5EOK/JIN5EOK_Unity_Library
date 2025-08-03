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

namespace Jin5eok
{
    public class AddressablesInstanceManager
    {
        private static Dictionary<string, List<AsyncOperationHandle<GameObject>>> AddressToInstanceCollectionMap { get; set; } = new ();
     
        public static AsyncOperationHandle<GameObject> LoadHandle(string address)
        {
            if (AddressToInstanceCollectionMap.TryGetValue(address, out var handles) == false)
            {
                handles = new List<AsyncOperationHandle<GameObject>>();
                AddressToInstanceCollectionMap.Add(address, handles);
            }

            var handle = Addressables.InstantiateAsync(address);

            handle.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded && op.Result != null)
                {
                    op.Result.AddComponent<AddressablesInstanceReleaser>().Initialize(op);
                }
                else
                {
                    Debug.LogWarning($"{nameof(AddressablesInstanceManager)} Instantiation failed for address: {address}");
                    handles.Remove(op);
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
            foreach (var address in AddressToInstanceCollectionMap.Keys)
            {
                ReleaseInstances(address);
            }
        }
    }
}
#endif
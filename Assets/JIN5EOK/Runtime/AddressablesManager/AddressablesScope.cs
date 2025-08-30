using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Jin5eok
{
    public class AddressablesScope : IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _addressToHandles = new();
        private readonly Dictionary<string, AsyncOperationHandle> _assetReferenceToHandles = new();
        private readonly HashSet<AsyncOperationHandle<GameObject>> _instantiatedHandles = new();

        private bool _isDisposed = false;

        public AsyncOperationHandle<T> LoadAssetAsync<T>(AssetReferenceT<T> assetReference)  where T : Object
        {
            var key = assetReference.RuntimeKey.ToString();
            if (_assetReferenceToHandles.TryGetValue(key, out AsyncOperationHandle handle))
            {
                if (handle.IsValid())
                {
                    return handle.Convert<T>();
                }

                _assetReferenceToHandles.Remove(key);
            }
            
            AsyncOperationHandle<T> newHandle = assetReference.LoadAssetAsync<T>();
            _assetReferenceToHandles[key] = newHandle;

            return newHandle;
        }
        
        public AsyncOperationHandle<T> LoadAssetAsync<T>(string address) where T : Object
        {
            if (_addressToHandles.TryGetValue(address, out AsyncOperationHandle handle))
            {
                if (handle.IsValid())
                {
                    return handle.Convert<T>();
                }

                _addressToHandles.Remove(address);
            }
            
            AsyncOperationHandle<T> newHandle = Addressables.LoadAssetAsync<T>(address);
            _addressToHandles[address] = newHandle;

            return newHandle;
        }
        
        public AsyncOperationHandle<GameObject> InstantiateAsync(AssetReferenceGameObject assetReference, bool releaseOnGameObjectDestroy = true)
        {
            var handle = assetReference.InstantiateAsync();
            return InstantiateProcess(handle,releaseOnGameObjectDestroy);
        }
        
        public AsyncOperationHandle<GameObject> InstantiateAsync(string address, bool releaseOnGameObjectDestroy = true)
        {
            var handle = Addressables.InstantiateAsync(address);
            return InstantiateProcess(handle,releaseOnGameObjectDestroy);
        }

        private AsyncOperationHandle<GameObject> InstantiateProcess(AsyncOperationHandle<GameObject> handle, bool releaseOnGameObjectDestroy = true)
        {
            _instantiatedHandles.Add(handle);
            
            handle.Completed += h =>
            {
                if (releaseOnGameObjectDestroy == true)
                {
                    h.Result.AddComponent<AddressablesInstanceReleaser>().Initialize(h); 
                }
                
                if (h.Status == AsyncOperationStatus.Failed)
                {
                    _instantiatedHandles.Remove(handle);
                }
            };

            handle.Destroyed += _ =>
            {
                _instantiatedHandles.Remove(handle);
            };

            return handle;
        }
        
        public void Dispose()
        {
            if (_isDisposed)
                return;

            foreach (var handle in _addressToHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            foreach (var handle in _assetReferenceToHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            foreach (var handle in _instantiatedHandles)
            {
                if (handle.IsValid())
                {
                    Addressables.ReleaseInstance(handle);
                }
            }

            _addressToHandles.Clear();
            _assetReferenceToHandles.Clear();
            _instantiatedHandles.Clear();
            _isDisposed = true;
        }

        ~AddressablesScope()
        {
            Dispose();
        }
    }
}
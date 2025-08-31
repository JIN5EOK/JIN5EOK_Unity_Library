using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Jin5eok
{
    public class AddressablesScope : IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _addressToHandles = new();
        private readonly Dictionary<string, AsyncOperationHandle> _assetReferenceToHandles = new();
        private readonly HashSet<AsyncOperationHandle> _instantiatedHandles = new();

        private bool _isDisposed = false;
        
        public AsyncOperationHandle<T> LoadAssetAsync<T>(AssetReference assetReference)  where T : Object
        {
            ThrowIfDisposed();
            
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
            newHandle.Destroyed += Destroyed;
            
            return newHandle;
            
            void Destroyed(AsyncOperationHandle _)
            {
                _assetReferenceToHandles.Remove(key);
            }
        }
        
        public AsyncOperationHandle<T> LoadAssetAsync<T>(string address) where T : Object
        {
            ThrowIfDisposed();
            
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
            newHandle.Destroyed += Destroyed;
            
            return newHandle;
            
            void Destroyed(AsyncOperationHandle _)
            {
                _addressToHandles.Remove(address);
            }
        }
        
        public AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetReference)
        {
            ThrowIfDisposed();
            
            var handle = assetReference.InstantiateAsync();
            return InstantiateProcess(handle);
        }
        
        public AsyncOperationHandle<GameObject> InstantiateAsync(string address)
        {
            ThrowIfDisposed();
            
            var handle = Addressables.InstantiateAsync(address);
            return InstantiateProcess(handle);
        }

        private AsyncOperationHandle<GameObject> InstantiateProcess(AsyncOperationHandle<GameObject> handle)
        {
            ThrowIfDisposed();
            
            _instantiatedHandles.Add(handle);
            
            handle.Completed += h =>
            {
                if (h.Status == AsyncOperationStatus.Failed)
                {
                    _instantiatedHandles.Remove(handle);
                }
            };
            handle.Destroyed += Destroyed;
            
            return handle;
            
            void Destroyed(AsyncOperationHandle destroyedHandle)
            {
                _instantiatedHandles.Remove(destroyedHandle);
            }
        }
        
        public void Dispose()
        {
            ThrowIfDisposed();
            
            // 순회 도중 컬렉션 내용 변경으로 인한 예외 방지
            var addressToHandlesCache = _addressToHandles.Values.ToArray();
            
            for (int i = 0; i < addressToHandlesCache.Length; i++)
            {
                var handle = addressToHandlesCache[i]; 
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            var assetReferenceToHandlesCache = _assetReferenceToHandles.Values.ToArray();
            
            for (int i = 0; i < assetReferenceToHandlesCache.Length; i++)
            {
                var handle = assetReferenceToHandlesCache[i]; 
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            var instantiatedHandlesCache = _instantiatedHandles.ToArray();
            
            for (int i = 0; i < instantiatedHandlesCache.Length; i++)
            {
                var handle = instantiatedHandlesCache[i]; 
                if (handle.IsValid())
                {
                    Addressables.ReleaseInstance(handle);
                }
            }

            _addressToHandles.Clear();
            _assetReferenceToHandles.Clear();
            _instantiatedHandles.Clear();
            _isDisposed = true;
            
            GC.SuppressFinalize(this);
        }

        ~AddressablesScope()
        {
            Dispose();
        }
        
        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
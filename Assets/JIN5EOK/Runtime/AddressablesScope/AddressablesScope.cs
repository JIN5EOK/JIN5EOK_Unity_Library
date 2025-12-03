#if USE_ADDRESSABLES
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Jin5eok
{
    /// <summary>
    /// 어드레서블로 로드한 에셋 / 프리펩 인스턴스의 생명주기 관리를 용이하게 하기 위한 스코프 클래스입니다.
    /// 스코프를 통해 에셋을 로드할 경우 핸들들이 스코프 내부에 캐싱되며 스코프가 Dispose될 때 포함된 에셋들을 모두 Release처리합니다.
    /// - 에셋의 경우 : 같은 주소 혹은 에셋 레퍼런스당 하나의 핸들만 캐싱하며 캐싱된 상태에서 요청시 이전에 반환된 핸들을 반환합니다
    /// - 프리펩 인스턴스의 경우 : 인스턴스와 핸들은 1:1 관계이므로 인스턴스 생성시마다 새로운 핸들이 생성되어 반환됩니다.
    /// Dispose 호출을 누락한채 스코프에 대한 참조를 잃어버렸을 경우 이후 소멸자에서 Dispose를 호출합니다.
    /// 하지만 GC의 소멸자 호출 시점은 명확하지 않으며 그에 따른 갑작스런 성능저하가 발생할 수 있으므로 권장하지 않습니다.
    /// </summary>
    public class AddressablesScope : IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _addressToHandles = new();
        private readonly Dictionary<string, AsyncOperationHandle> _assetReferenceToHandles = new();
        private readonly HashSet<AsyncOperationHandle> _instantiatedHandles = new();

        private bool _isDisposed = false;
        
        /// <summary>
        /// 에셋 레퍼런스를 기반으로 에셋을 로드하여 반환합니다.
        /// </summary>
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
        
        /// <summary>
        /// 주소를 기반으로 에셋을 로드하여 반환합니다.
        /// </summary>
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
        
        /// <summary>
        /// 에셋 레퍼런스를 기반으로 프리펩을 인스턴스화하여 반환합니다.
        /// </summary>
        public AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetReference)
        {
            ThrowIfDisposed();
            
            var handle = assetReference.InstantiateAsync();
            return InstantiateProcess(handle);
        }
        
        /// <summary>
        /// 주소를 기반으로 프리펩을 인스턴스화하여 반환합니다.
        /// </summary>
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
        
        /// <summary>
        /// 내부에 캐싱된 핸들들을 모두 Release처리합니다.
        /// Dispose이후 스코프의 함수를 호출하면 예외가 발생합니다.
        /// </summary>
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
#endif
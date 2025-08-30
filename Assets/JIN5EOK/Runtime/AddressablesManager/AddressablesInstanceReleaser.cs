#if USE_ADDRESSABLES
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Jin5eok
{
    internal class AddressablesInstanceReleaser : MonoBehaviour
    {
        private bool _initialized = false;
        private AsyncOperationHandle<GameObject> _handle;
        
        public void Initialize(AsyncOperationHandle<GameObject> handle)
        {
            if (_initialized == true)
            {
                return;
            }
            _initialized = true;
            _handle = handle;
        }

        private void OnDestroy()
        {
            if (_handle.IsValid() == true)
            {
                Addressables.Release(_handle);
            }
        }
    }
}
#endif
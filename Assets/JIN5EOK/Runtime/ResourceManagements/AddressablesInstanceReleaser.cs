using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Jin5eok.ResourceManagements
{
    public class AddressablesInstanceReleaser : MonoBehaviour
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
            if (_handle.IsValid() == true && _handle.IsDone == true && _handle.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(_handle);
            }
        }
    }
}
using System;
using System.Collections;
using Jin5eok.Patterns;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Jin5eok.ResourceManagements
{
    internal class AddressablesCoroutineLoader : MonoSingleton<AddressablesCoroutineLoader>
    {
        public void LoadAsyncCoroutine<T>(AsyncOperationHandle<T> handle, Action<T> onResult) where T : Object
        {
            StartCoroutine(LoadAsyncCoroutineInternal(handle, onResult));
        }

        private IEnumerator LoadAsyncCoroutineInternal<T>(AsyncOperationHandle<T> handle, Action<T> onResult) where T : Object
        {
            yield return new WaitUntil(() => handle.IsDone);
            onResult?.Invoke(handle.Result);
        }
    }
}
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Jin5eok.Test
{
    public class AddressablesScopeTest
    {
        private AddressablesScope _scope;
        private AddressablesScopeTestData _scopeTestData;
        
        [UnitySetUp]
        public IEnumerator BeforeTestProcess()
        {
            _scope = new AddressablesScope();
            
            var handle = _scope.LoadAssetAsync<AddressablesScopeTestData>("AddressablesScopeTestAsset");
            yield return handle;
            
            _scopeTestData = handle.Result;
        }
    
        [TearDown]
        public void AfterTestProcess()
        {
            _scope?.Dispose();
            _scope = null;
        }
    
        [Description("같은 주소로 에셋을 여러번 로드 할 경우 중복 핸들 로드 없이 캐싱된 핸들이 반환되는지 확인")]
        [Test]
        public void Test_SameAssetLoad()
        {
            var handleA1 = _scope.LoadAssetAsync<Object>(_scopeTestData.assetAddress);
            var handleA2 = _scope.LoadAssetAsync<Object>(_scopeTestData.assetAddress);

            var handleB1 = _scope.LoadAssetAsync<Object>(_scopeTestData.assetReferencePrefab);
            var handleB2 = _scope.LoadAssetAsync<Object>(_scopeTestData.assetReferencePrefab);
            
            Assert.AreEqual(handleA1, handleA2); 
            Assert.AreEqual(handleB1, handleB2); 
        }

        [Description("스코프를 통한 에셋/프리펩 로드 및 스코프 해제시 포함된 핸들들이 정상적으로 릴리즈되는지 확인")]
        [UnityTest]
        public IEnumerator Test_AssetLoadAndRelease()
        {
            var handleA = _scope.LoadAssetAsync<Object>(_scopeTestData.assetAddress);
            var handleB = _scope.InstantiateAsync(_scopeTestData.prefabAddress);
            var handleC = _scope.LoadAssetAsync<Object>(_scopeTestData.assetReferenceAsset);
            var handleD = _scope.InstantiateAsync(_scopeTestData.assetReferencePrefab);
            
            yield return handleA;
            yield return handleB;
            yield return handleC;
            yield return handleD;
            
            Assert.IsTrue(handleA.IsValid());
            Assert.IsTrue(handleB.IsValid());
            Assert.IsTrue(handleC.IsValid());
            Assert.IsTrue(handleD.IsValid());
            
            _scope?.Dispose();
            _scope = null;
            
            Assert.IsFalse(handleA.IsValid());
            Assert.IsFalse(handleB.IsValid());
            Assert.IsFalse(handleC.IsValid());
            Assert.IsFalse(handleD.IsValid());
        }
    }
}

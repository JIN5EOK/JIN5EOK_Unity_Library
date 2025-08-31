using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jin5eok.Test
{
    [CreateAssetMenu(fileName = "New Test Asset", menuName = "Test/Test Asset")]
    public class AddressablesScopeTestData : ScriptableObject
    {
        public AssetReference assetReferenceAsset;
        public AssetReference assetReferencePrefab;
        
        public string assetAddress;
        public string prefabAddress;
    }
}
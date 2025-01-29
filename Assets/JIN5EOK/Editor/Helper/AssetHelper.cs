using UnityEditor;
using UnityEngine;

namespace Jin5eok.Editor.Helper
{
    public class AssetHelper
    {
        public static void CreateScriptableObject<T>(string createPath = "Assets/") where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            var path = $"{createPath}{nameof(T)}.asset";
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
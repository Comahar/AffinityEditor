using UnityEngine;

namespace AffinityEditor {
    public static class AssetBundleHelper {
        public static AssetBundle assetBundle;
        public static void LoadBundle(string path) {
            if (!Application.isEditor)
                assetBundle = AssetBundle.LoadFromFile(path);
        }

        public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object {
            #if UNITY_EDITOR
                if (string.IsNullOrEmpty(assetName)){
                    Debug.LogError("Requested asset name is null or empty");
                    return null;
                }
                var assets = UnityEditor.AssetDatabase.FindAssets(assetName);
                if (assets.Length == 0){
                    Debug.LogError("Asset not found with name " + assetName);
                    return null;
                } else if (assets.Length > 1){
                    Debug.LogError("Multiple assets found with name " + assetName);
                    return null;
                }else{
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]));
                }
            #else
                if (string.IsNullOrEmpty(assetName)){
                    Plugin.Logger.LogError("Requested asset name is null or empty");
                    return null;
                }
                return assetBundle.LoadAsset<T>(assetName);
            #endif
        }
    }
}
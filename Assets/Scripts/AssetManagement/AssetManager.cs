using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using Object = System.Object;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Wixot.Engine;

namespace Wixot.AssetManagement
{
    public class AssetManager : Singleton<AssetManager>
    {
        // Caches for loaded assets, reference counts, callbacks, and handles
        private readonly Dictionary<string, Object> _cachedLoadedAssets = new();
        private readonly Dictionary<string, int> _assetReferenceCount = new();
        private readonly Dictionary<string, List<Delegate>> _assetCallbacks = new();

        public void LoadAllAssets(IEnumerable<string> labels, Action<Dictionary<string, Object>> onAllLoaded)
        {
            StartCoroutine(LoadAllAssetsInParallel(labels, onAllLoaded));
        }

        // Loads multiple assets in parallel
        private IEnumerator LoadAllAssetsInParallel(IEnumerable<string> labels, Action<Dictionary<string, Object>> onAllLoaded)
        {
            int assetsToLoadCount = labels.Count();
            int loadedAssetsCount = 0;
            Dictionary<string, Object> loadedAssets = new Dictionary<string, Object>();

            foreach (var label in labels)
            {
                LoadAsset<UnityEngine.Object>(label, (asset) =>
                {
                    if (asset != null)
                    {
                        loadedAssets.Add(label, asset);
                        loadedAssetsCount++;
                    }

                    if (loadedAssetsCount == assetsToLoadCount)
                    {
                        onAllLoaded?.Invoke(loadedAssets);
                    }
                });
            }

            yield return null;
        }

        public void LoadAllAssets<T>(IEnumerable<string> labels, Action<Dictionary<string, T>> onAllLoaded) where T : UnityEngine.Object
        {
            StartCoroutine(LoadAllAssetsInParallel<T>(labels, onAllLoaded));
        }

        private IEnumerator LoadAllAssetsInParallel<T>(IEnumerable<string> labels, Action<Dictionary<string, T>> onAllLoaded) where T : UnityEngine.Object
        {
            int assetsToLoadCount = labels.Count();
            int loadedAssetsCount = 0;
            Dictionary<string, T> loadedAssets = new Dictionary<string, T>();

            foreach (var label in labels)
            {
                LoadAsset<T>(label, (asset) =>
                {
                    if (asset != null)
                    {
                        loadedAssets.Add(label, asset);
                        loadedAssetsCount++;
                    }

                    if (loadedAssetsCount == assetsToLoadCount)
                    {
                        onAllLoaded?.Invoke(loadedAssets);
                    }
                });
            }

            yield return null;
        }

        // Loads a single asset by label
        public void LoadAsset<T>(string label, Action<T> callback) where T : UnityEngine.Object
        {
            if (_cachedLoadedAssets.ContainsKey(label))
            {
                callback?.Invoke(_cachedLoadedAssets[label] as T);
                return;
            }

            if (_assetCallbacks.ContainsKey(label))
            {
                _assetCallbacks[label].Add(callback);
                return;
            }

            _assetCallbacks[label] = new List<Delegate> { callback };

            var handle = Addressables.LoadAssetAsync<T>(label);
            handle.Completed += (op) => OnAssetLoadCompleted(label, op, callback);
        }

        // Gets a loaded asset by label
        public T GetLoadedAsset<T>(string label) where T : UnityEngine.Object
        {
            if (_cachedLoadedAssets.TryGetValue(label, out var asset))
            {
                return asset as T;
            }

            Debug.LogWarning($"[AssetLoader] - Attempted to get asset that isn't loaded: {label}");
            return null;
        }
        
        // Instantiates a GameObject by label at a given position, rotation, and parent transform
        public GameObject Instantiate(string label, Vector3 position = default, Quaternion rotation = default, Transform parentTransform = null)
        {
            var loadedAsset = GetLoadedAsset<GameObject>(label);
            if (loadedAsset == null)
            {
                Debug.LogWarning($"[AssetLoader] - Attempted to instantiate asset that isn't loaded: {label}");
                return null;
            }

            if (!_assetReferenceCount.TryAdd(label, 1)) _assetReferenceCount[label]++;
            var instantiatedGameObject = UnityEngine.Object.Instantiate(loadedAsset, position, rotation, parentTransform);

            return instantiatedGameObject;
        }
        

       

        // Callback method when asset loading is completed
        private void OnAssetLoadCompleted<T>(string label, AsyncOperationHandle op, Action<T> callback) where T : UnityEngine.Object
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[AssetLoader] - Asset loaded successfully: {label}");

                _cachedLoadedAssets.Add(label, op.Result);
                
                foreach (var cb in _assetCallbacks[label])
                {
                    ((Action<T>)cb)?.Invoke(op.Result as T);
                }

                // Addressables.Release(op);
                _assetCallbacks.Remove(label);
            }
            else
            {
                Debug.LogError($"[AssetLoader] - Asset load failed: {label}");
                // Handle error callbacks or notifications here if needed
            }
        }
        
    }
}
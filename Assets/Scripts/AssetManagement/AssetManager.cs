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
        private readonly Dictionary<string, AsyncOperationHandle> _assetHandles = new();

        public void LoadAllAssets(IEnumerable<string> labels, Action<Dictionary<string, Object>> onAllLoaded)
        {
            StartCoroutine(LoadAllAssetsInParallel(labels, onAllLoaded));
        }

        // Loads multiple assets in parallel
        public IEnumerator LoadAllAssetsInParallel(IEnumerable<string> labels, Action<Dictionary<string, Object>> onAllLoaded)
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
            _assetHandles.Add(label, handle);
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
        
        // Instantiates  asset at a given position and rotation
        public T Instantiate<T>(string label, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            var instantiatedGameObject = Instantiate(label, position, rotation, parentTransform);
            return instantiatedGameObject.GetComponent<T>();
        }
        
        public T Instantiate<T>(string label, Transform parentTransform)
        {
            var instantiatedGameObject = Instantiate(label, parentTransform);
            return instantiatedGameObject.GetComponent<T>();
        }

        public GameObject Instantiate(GameObject loadedGameObject, Vector3 position = default, Quaternion rotation = default, Transform parentTransform = null)
        {
            if (!loadedGameObject.TryGetComponent(out IAddressableTag addressableLabelComponent))
            {
                Debug.LogWarning($"[AssetLoader] - Attempted to instantiate asset that isn't have Addressable Label Component");
                return null;
            }

            string label = addressableLabelComponent.AddressableLabel;

            if (!_assetReferenceCount.TryAdd(label, 1)) _assetReferenceCount[label]++;

            var instantiatedGameObject = UnityEngine.Object.Instantiate(loadedGameObject, position, rotation, parentTransform);

            IAddressableTag addressableTag = GetOrAddAddressableComponent(instantiatedGameObject);
            addressableTag.AddressableLabel = label;

            return instantiatedGameObject;
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

            IAddressableTag addressableTag = GetOrAddAddressableComponent(instantiatedGameObject);
            addressableTag.AddressableLabel = label;

            return instantiatedGameObject;
        }
        
        public GameObject Instantiate(string label, Transform parentTransform = null)
        {
            var loadedAsset = GetLoadedAsset<GameObject>(label);
            if (loadedAsset == null)
            {
                Debug.LogWarning($"[AssetLoader] - Attempted to instantiate asset that isn't loaded: {label}");
                return null;
            }

            if (!_assetReferenceCount.TryAdd(label, 1)) _assetReferenceCount[label]++;
            var instantiatedGameObject = UnityEngine.Object.Instantiate(loadedAsset, parentTransform);

            IAddressableTag addressableTag = GetOrAddAddressableComponent(instantiatedGameObject);
            addressableTag.AddressableLabel = label;

            return instantiatedGameObject;
        }


        // Destroys a GameObject
        public void Destroy(GameObject obj)
        {
            if (!obj.TryGetComponent<IAddressableTag>(out var addressableTag))
            {
                Debug.LogWarning("[AssetLoader] - Attempted to destroy a non-addressable GameObject.");
                return;
            }

            var label = addressableTag.AddressableLabel;
            if (!_cachedLoadedAssets.ContainsKey(label))
            {
                Debug.LogWarning($"[AssetLoader] - Attempted to destroy object of an asset that isn't loaded: {label}");
                return;
            }

            UnloadAsset(label);
            UnityEngine.Object.Destroy(obj);
        }

        // Unloads an asset by label
        public void UnloadAsset(string label)
        {
            if (!_cachedLoadedAssets.ContainsKey(label))
            {
                Debug.LogWarning($"[AssetLoader] - Attempted to unload asset that isn't loaded: {label}");
                return;
            }

            // Decrement reference count for the asset
            if (_assetReferenceCount.ContainsKey(label))
            {
                Debug.Log(_assetReferenceCount[label]);
                _assetReferenceCount[label]--;

                if (_assetReferenceCount[label] <= 0)
                {
                    Debug.Log($"[AssetLoader] - Unloading asset: {label}");

                    Addressables.Release(_assetHandles[label]);
                    _assetHandles.Remove(label);
                    _cachedLoadedAssets.Remove(label);
                    _assetReferenceCount.Remove(label);
                }
            }
            else
            {
                Debug.LogWarning($"[AssetManagementService] - Unloaded object had no recorded reference for asset: {label}");
                // Addressables.Release(_cachedLoadedAssets[label]);
                Addressables.Release(_assetHandles[label]);
                _assetHandles.Remove(label);
                _cachedLoadedAssets.Remove(label);
            }
        }

        // Callback method when asset loading is completed
        private void OnAssetLoadCompleted<T>(string label, AsyncOperationHandle op, Action<T> callback) where T : UnityEngine.Object
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[AssetLoader] - Asset loaded successfully: {label}");

                _cachedLoadedAssets.Add(label, op.Result);

                if (op.Result is GameObject loadedGameObject)
                {
                    IAddressableTag addressableTag = GetOrAddAddressableComponent(loadedGameObject);
                    addressableTag.AddressableLabel = label;
                }

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
        
        public IAddressableTag GetOrAddAddressableComponent(GameObject gameObject)
        {
            IAddressableTag addressableTag = gameObject.GetComponent<IAddressableTag>();
            if (addressableTag == null) addressableTag = gameObject.AddComponent<AddressableLabelComponent>();

            return addressableTag;
        }
        
    }
}
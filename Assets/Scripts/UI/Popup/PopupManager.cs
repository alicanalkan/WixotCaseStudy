using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Wixot.AssetManagement;

namespace Wixot.UI.Popup
{
    public class PopupManager : IPopupManager
    {
        private int _activeUILayerIndex = 0;
        private BasePopupView[] _activePopupViews;

        private readonly int _layerCount = 4;
        private readonly UILayer[] _uiLayers;
        private readonly CanvasGroup _canvasGroup;
        private readonly AssetManager _assetManager;
        private readonly Queue<PopupQueue> _popupQueue = new Queue<PopupQueue>();
        private readonly Dictionary<string, BasePopupView> _popupViews = new Dictionary<string, BasePopupView>();

        public PopupManager()
        {
            _assetManager = AssetManager.Instance;
            _uiLayers = new UILayer[_layerCount];
        }
        
        /// <summary>
        /// PopupCanvas Load
        /// </summary>
        /// <param name="parentTransform"></param>
        public void InstantiatePopupManager(Transform parentTransform)
        {
            _assetManager.LoadAsset<GameObject>("PopupCanvas", CanvasLoaded);
        }
        private void CanvasLoaded(GameObject canvas)
        {
            GameObject canvasGameObject = _assetManager.Instantiate(canvas, Vector3.zero, Quaternion.identity, UIManager.Instance.transform);
            _activePopupViews = new BasePopupView[_layerCount];

            for (int i = 0; i < _layerCount; i++)
            {
                _uiLayers[i] = new UILayer(i, canvasGameObject.transform);
            }
        }
        
        /// <summary>
        /// Pop popup with definition at given layerindex
        /// </summary>
        /// <param name="popupDefinition"></param>
        /// <param name="layerIndex"></param>
        public void ShowPopup(PopupDefinition popupDefinition, int layerIndex = 0)
        {
            try
            {
                _popupQueue.Enqueue(new PopupQueue(popupDefinition, layerIndex));

                // Check if the popup view already exists in the dictionary
                if (_popupViews.ContainsKey(popupDefinition.AddressableName))
                {
                    ShowPopup();
                }
                else
                {
                    // If the popup view does not exist, load it.
                    _assetManager.LoadAsset<GameObject>(popupDefinition.AddressableName, loadedPopup => OnPopupLoaded(loadedPopup, popupDefinition.AddressableName, layerIndex));
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[PopupService] Exception occurred: {e.Message}");
            }
        }

        /// <summary>
        /// Hide for next usage scenario
        /// </summary>
        public void HidePopup()
        {
            _activePopupViews[_activeUILayerIndex]?.Hide();
            _activePopupViews[_activeUILayerIndex] = null;

            if (_popupQueue.Count > 1)
            {
                ShowPopup();
            }
            else
            {
                
                _uiLayers[_activeUILayerIndex].HideLayer();
                if(_activeUILayerIndex >= 1)
                    _activeUILayerIndex--;
            }
        }

        private void OnPopupLoaded(GameObject loadedPopup, string prefabName, int layerIndex)
        {
            if (loadedPopup == null)
            {
                Debug.LogError("[PopupService] Loaded popup is null.");
                return;
            }

            if (_popupViews.ContainsKey(prefabName))
            {
                return;
            }

            GameObject popupGameObject = _assetManager.Instantiate(loadedPopup, Vector3.zero, Quaternion.identity, _uiLayers[layerIndex].Transform);
            popupGameObject.SetActive(false);

            if (!popupGameObject.TryGetComponent(out BasePopupView popupView))
            {
                Debug.LogError($"PopupView not found on {popupGameObject.name}");
                return;
            }

            popupView.Initialize(this);
            _popupViews.Add(prefabName, popupView);

            ShowPopup();
        }

        /// <summary>
        /// Activate loaded popup at popup canvas
        /// </summary>
        private void ShowPopup()
        {
            PopupQueue popupQueue = _popupQueue.Peek();

            int layerIndex = popupQueue.LayerIndex;
            PopupDefinition popupDefinition = popupQueue.PopupDefinition;

            if (_activePopupViews[layerIndex] == null)
            {
                if (_popupViews.TryGetValue(popupDefinition.AddressableName, out BasePopupView popupView))
                {
                    _popupQueue.Dequeue();

                    popupView.InitializeDefinition(popupDefinition);
                    popupView.Show();
                    _activePopupViews[layerIndex] = popupView;

                    _activeUILayerIndex = layerIndex;
                    _uiLayers[layerIndex].ShowLayer();
                }
            }

            ReorderPopupQueue();
        }

        /// <summary>
        /// popup Queue Handle
        /// </summary>
        private void ReorderPopupQueue()
        {
            bool needReorder = _popupQueue.Any(popup => popup.PopupDefinition.Priority != _popupQueue.Peek().PopupDefinition.Priority);
            if (!needReorder)
                return;

            var sortedPopups = _popupQueue.OrderByDescending(popup => popup.PopupDefinition.Priority).ToList();
            _popupQueue.Clear();

            foreach (var popup in sortedPopups)
            {
                _popupQueue.Enqueue(popup);
            }
        }
        
    }
}
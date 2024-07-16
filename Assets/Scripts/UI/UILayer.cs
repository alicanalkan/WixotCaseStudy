using UnityEngine;
using UnityEngine.UI;
using Wixot.UI.Utils;

namespace Wixot.UI
{
    public class UILayer
    {
        public Transform Transform => _transform;

        private readonly Transform _transform;
        private readonly Image _backgroundImage;
        private readonly CanvasGroup _canvasGroup;

        public UILayer(int layerIndex, Transform parent, bool isActive = false)
        {
            _transform = CreateUILayer(layerIndex, parent, out _canvasGroup, out _backgroundImage);
            _backgroundImage.color = new Color(0, 0, 0, .49f);

            if (isActive)
                ShowCanvasGroup(_canvasGroup);
            else
                HideCanvasGroup(_canvasGroup);
        }

        public void ShowLayer()
        {
            ShowCanvasGroup(_canvasGroup);
        }

        public void HideLayer()
        {
            HideCanvasGroup(_canvasGroup);
        }
        
        private void ShowCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void HideCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        private  RectTransform CreateUILayer(int layerIndex, Transform parent, out CanvasGroup canvasGroup, out Image backgroundImage)
        {
            GameObject uiLayerGo = new GameObject($"Layer-{layerIndex}");

            backgroundImage = uiLayerGo.GetOrAdd<Image>();
            canvasGroup = uiLayerGo.GetOrAdd<CanvasGroup>();

            uiLayerGo.transform.SetParent(parent);
            uiLayerGo.transform.SetSiblingIndex(layerIndex);
            uiLayerGo.transform.localScale = Vector3.one;
            RectTransform rectTransform = uiLayerGo.transform as RectTransform;
            
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = Vector2.zero;
            rectTransform.offsetMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            
            rectTransform.anchoredPosition = new Vector2(0, 0);

            return rectTransform;
        }
    }
}
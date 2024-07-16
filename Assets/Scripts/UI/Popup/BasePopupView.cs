using UnityEngine;
using Wixot.UI.View;

namespace Wixot.UI.Popup
{
    /// <summary>
    /// Priority for more urgent popups
    /// </summary>
    public enum PopupPriority
    {
        A = 1, B = 2, C = 3,
    }
    public abstract class BasePopupView : MonoBehaviour, IUIView
    {
        public RectTransform RectTransform => (RectTransform)transform;

        protected IPopupManager PopupManager;

        public void Initialize(IPopupManager popupManager)
        {
            PopupManager = popupManager;
        }

        public abstract void InitializeDefinition(UIDefinition uiDefinition);

        public virtual void Show(Vector2 position = default)
        {
            ((RectTransform)transform).anchoredPosition = position;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public void CloseView()
        {
            PopupManager.HidePopup();
        }
    }
}
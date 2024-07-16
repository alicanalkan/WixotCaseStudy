using UnityEngine;

namespace Wixot.UI.View
{
    public interface IUIView
    {
        RectTransform RectTransform { get; }

        void InitializeDefinition(UIDefinition uiDefinition);
        void Show(Vector2 position);
        void Hide();
        void CloseView();
    }
}

namespace Wixot.UI.Popup
{
    public interface IPopupManager
    {
        void ShowPopup(PopupDefinition popupDefinition, int layerIndex = 0);
        void HidePopup();
    }
}

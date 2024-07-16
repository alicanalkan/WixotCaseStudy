namespace Wixot.UI.Popup
{
    public class PopupQueue
    {
        public int LayerIndex => _layerIndex;
        public PopupDefinition PopupDefinition => _popupDefinition;

        private readonly int _layerIndex;
        private readonly PopupDefinition _popupDefinition;

        public PopupQueue(PopupDefinition popupDefinition, int layerIndex)
        {
            _layerIndex = layerIndex;
            _popupDefinition = popupDefinition;
        }
    }
}

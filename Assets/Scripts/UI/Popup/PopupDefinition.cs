using Wixot.UI.View;
namespace Wixot.UI.Popup
{
    public abstract class PopupDefinition : UIDefinition
    {
        public abstract PopupPriority Priority { get; }
        public abstract override string AddressableName { get; }
    }
}

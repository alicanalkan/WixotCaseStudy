using System;

namespace Wixot.UI.Popup.Definitions
{
    public class WarningPopupDefinition : PopupDefinition
    {
        public override string AddressableName => "WarningPopup";
        public override PopupPriority Priority => PopupPriority.B;
        
        public readonly Action OnConfirm;
        public readonly string TextLabel;
        
        public WarningPopupDefinition(string textLabel, Action onConfirm)
        {
            OnConfirm = onConfirm;
            TextLabel = textLabel;
        }
    }
}

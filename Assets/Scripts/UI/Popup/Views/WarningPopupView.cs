using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wixot.UI.Popup.Definitions;
using Wixot.UI.View;

namespace Wixot.UI.Popup.Views
{
    public class WarningPopupView : BasePopupView
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button exitButton;
        
        private Action _onConfirm;
        public override void InitializeDefinition(UIDefinition uiDefinition)
        {
            if (uiDefinition is not WarningPopupDefinition confirmPopupDefinition) return;
            
            _onConfirm = confirmPopupDefinition.OnConfirm;
            textField.SetText(confirmPopupDefinition.TextLabel);
        }
        
        private void Awake()
        {
            confirmButton.onClick.AddListener(Confirm);
        }

        private void Exit()
        {
            CloseView();
        }
        public void Confirm()
        {
            _onConfirm?.Invoke();
            CloseView();
        }
        private void OnDestroy()
        {
            confirmButton.onClick.RemoveListener(Confirm);
        }
    }
    
   
}

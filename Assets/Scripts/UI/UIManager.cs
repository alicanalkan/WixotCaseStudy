using System;
using UnityEngine;
using UnityEngine.UI;
using Wixot.AssetManagement;
using Wixot.Character;
using Wixot.Engine;
using Wixot.Stats;
using Wixot.UI.Panel;
using Wixot.UI.Popup;
using Wixot.UI.Popup.Definitions;

namespace Wixot.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private PopupManager PopupManager;
        private AssetManager _assetManager;
        
        [SerializeField] private Canvas UICanvas;
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button ExitButton;
        public Canvas Canvas =>  UICanvas;
        private void Start()
        {
            _assetManager = AssetManager.Instance;
        }

        public void LoadUI()
        {
            PopupManager = new PopupManager();
            PopupManager.InstantiatePopupManager(transform);
            AssetManager.Instance.LoadAsset<GameObject>("PowerUpPanel",(_)=>{});
        }

        public void InstantiatePanel(Hero hero)
        {
            var powerUpPanel = _assetManager.Instantiate<PowerUpPanel>("PowerUpPanel",UICanvas.transform);
            powerUpPanel.hero = hero;
        }

        public void SetPlayButton(bool state)
        {
            PlayButton.gameObject.SetActive(state);
        }

        public void SetExitButton(bool state)
        {
            ExitButton.gameObject.SetActive(state);
        }

        public void ShowWarningPopup()
        {
            PopupManager.ShowPopup(new WarningPopupDefinition("Are you sure want to exit",
                GameManager.Instance.StopGame));
        }
        
    }
}

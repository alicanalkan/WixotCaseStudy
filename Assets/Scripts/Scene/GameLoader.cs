using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wixot.AssetManagement;
using Wixot.AssetManagement.PoolManagement;
using Wixot.UI;

namespace Wixot.Scene
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;

        [SerializeField] private UIManager uiManager;

        [SerializeField] private Image _loadingBar;

        private AssetManager _assetManager;

        private float _loadingValue = 0.0f;
        
        /// <summary>
        /// Loading UI Character And riffle
        /// </summary>
        void Start()
        {
            _assetManager = AssetManager.Instance;
            poolManager.FillPool(OnPoolFilled);

            LoadUI();
            LoadCharacter();
            LoadRifle();
        }

        private void OnPoolFilled()
        {
            _loadingValue += 0.25f;
            _loadingBar.fillAmount =_loadingValue;
            
            CheckIsGameCanStart();
        }

        private void LoadUI()
        {
            uiManager.LoadUI();
            _loadingValue += 0.25f;
            _loadingBar.fillAmount =_loadingValue;
        }

        private void CheckIsGameCanStart()
        {
            if (_loadingValue >= 1)
            {
                SceneManager.LoadScene("GameScene");
            }
        }

        private void LoadCharacter()
        {
            _assetManager.LoadAsset("Character",(GameObject character)=>{_loadingValue += 0.25f; _loadingBar.fillAmount =_loadingValue;});
        }

        private void LoadRifle()
        {
            _assetManager.LoadAsset("Rifle",(GameObject rifle)=>{_loadingValue += 0.25f; _loadingBar.fillAmount =_loadingValue;});
        }

    }
}

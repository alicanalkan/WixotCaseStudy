using Unity.Mathematics;
using UnityEngine;
using Wixot.AssetManagement;
using Wixot.Camera;
using Wixot.Character;
using Wixot.Engine;
using Wixot.UI;

namespace Wixot
{
    public class GameManager : Singleton<GameManager>
    {
        private AssetManager _assetManager;
        private UIManager _uiManager;

        private Hero character;
        public void StartLoad()
        {
            _assetManager = AssetManager.Instance;
            _uiManager = UIManager.Instance;
            GetCharacter();
            SpawnPowerUpButtons();
            SetPlayButton(true);
            
            var camera = UnityEngine.Camera.main!.gameObject;
            var movement = camera.AddComponent<CameraMovement>();
            movement._transform = character.transform;
        }

        /// <summary>
        /// Instantiate Loaded Character
        /// </summary>
        private void GetCharacter()
        {
            character = _assetManager.Instantiate<Hero>("Character", Vector3.zero, quaternion.identity, null);
            GetWeapon(character);
        }

        /// <summary>
        /// Instantiate weapon to instantiated character
        /// </summary>
        /// <param name="character"></param>
        private void GetWeapon(Hero character)
        {
            var weapon = _assetManager.Instantiate<Weapon.BasicWeapon>("Rifle", character.transform);
            character.characterWeapon = weapon;
        }

        /// <summary>
        /// start controller and fire
        /// </summary>
        public void StartGame()
        {
            character.GetComponent<HeroController>().characterCanMove = true;
            character.StartShooting();
            SetPlayButton(false);
            SetExitButton(true);
        }
        /// <summary>
        /// Stop control and fire
        /// </summary>
        public void StopGame()
        {
            //Reset position
            character.transform.position = Vector3.zero;
            character.transform.rotation = quaternion.identity;
            
            character.GetComponent<HeroController>().characterCanMove = false;
            character.StopShooting();
            
            SetPlayButton(true);
            SetExitButton(false);
        }
        
        private void SpawnPowerUpButtons()
        {
            _uiManager.InstantiatePanel(character);
        }

        private void SetPlayButton(bool active)
        {
            _uiManager.SetPlayButton(active);
        }

        private void SetExitButton(bool active)
        {
            _uiManager.SetExitButton(active);
        }

       
    }
}

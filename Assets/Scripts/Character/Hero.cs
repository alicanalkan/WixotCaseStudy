using System.Collections.Generic;
using UnityEngine;
using Wixot.Stats;
using Wixot.Weapon;

namespace Wixot.Character
{
    public class Hero : MonoBehaviour
    {
        public BasicWeapon characterWeapon;
        private HeroController _controller;
        
        /// <summary>
        /// Stats modifier for character
        /// todo move to editor script or scriptable object for better manage
        /// </summary>
        private Dictionary<StatType, BasicStatModifier> _statModifiers = new Dictionary<StatType, BasicStatModifier>
        {
            {StatType.NozzleCount,  new BasicStatModifier(StatType.NozzleCount,v => v=3)},
            {StatType.BulletCount,  new BasicStatModifier(StatType.BulletCount,v => v*2)},
            {StatType.BulletSpeed,  new BasicStatModifier(StatType.BulletSpeed,v => v + v%50)},
            {StatType.FireRate,  new BasicStatModifier(StatType.FireRate,v => v + v%50)},
            {StatType.CharacterSpeed,  new BasicStatModifier(StatType.CharacterSpeed,v => v*2)}
        };

        private void Start()
        {
            _controller = GetComponent<HeroController>();
            _controller.speed = characterWeapon.Stats.CharacterSpeed;
        }

        /// <summary>
        /// Starting Auto Shooting
        /// </summary>
        public void StartShooting()
        {
            characterWeapon.StartShooting();
        }
        /// <summary>
        /// Stop Auto Shooting
        /// </summary>
        public void StopShooting()
        {
            characterWeapon.StopShooting();
        }
        

        /// <summary>
        /// Perform Power Up for given statType
        /// </summary>
        /// <param name="statType"></param>
        public void PowerUpWeapon(StatType statType)
        {
            if (CheckExistModifier(statType))
            {
                RemovePowerUp(statType);
            }
            else
            {
                characterWeapon.Stats.Mediator.AddModifier(_statModifiers[statType]);
            }

            _controller.speed = characterWeapon.Stats.CharacterSpeed;
        }

        /// <summary>
        /// Remove existing power up manualy
        /// </summary>
        /// <param name="statType"></param>
        public void RemovePowerUp(StatType statType)
        {
            _statModifiers[statType].Dispose();
        }

        /// <summary>
        /// Check modifier is exist for prevent duplicate modifier
        /// Max 3 modifier is static for base and next modifiers
        /// todo move modifier count to editor script
        /// </summary>
        /// <param name="statType"></param>
        /// <returns></returns>
        public bool CheckExistModifier(StatType statType)
        {
            if (characterWeapon.Stats.Mediator.HasModifier(_statModifiers[statType]))
            {
                return true;
            }
            else
            {
                if (characterWeapon.Stats.Mediator.modifierCount >= 3)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

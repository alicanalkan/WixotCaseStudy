using UnityEngine;
using UnityEngine.UI;
using Wixot.Character;
using Wixot.Stats;

namespace Wixot.UI.Panel
{
    public class PowerUpPanel : MonoBehaviour
    {
        public Hero hero;

        [SerializeField] private Image TrippleNozzle;
        [SerializeField] private Image DoubleBullet;
        [SerializeField] private Image SwiftStrike;
        [SerializeField] private Image FastBulet;
        [SerializeField] private Image FastHero;
        
        public void TripleNozzleButton()
        {
           
            if (!hero.CheckExistModifier(StatType.NozzleCount))
            {
                TrippleNozzle.color = Color.green;
                hero.PowerUpWeapon(StatType.NozzleCount);
            }
            else
            {
                TrippleNozzle.color = Color.white;
                hero.RemovePowerUp(StatType.NozzleCount);
            }
        }
        
        public void DoubleBulletButton()
        {
            if (!hero.CheckExistModifier(StatType.BulletCount))
            {
                DoubleBullet.color = Color.green;
                hero.PowerUpWeapon(StatType.BulletCount);
            }
            else
            {
                DoubleBullet.color = Color.white;
                hero.RemovePowerUp(StatType.BulletCount);
            }
           
        }
        
        public void SwiftStrikeButton()
        {
            if (!hero.CheckExistModifier(StatType.BulletSpeed))
            {
                SwiftStrike.color = Color.green;
                hero.PowerUpWeapon(StatType.BulletSpeed);
            }
            else
            {
                SwiftStrike.color = Color.white;
                hero.RemovePowerUp(StatType.BulletSpeed);
            }
            
        }
        public void FastBuletButton()
        {
            if (!hero.CheckExistModifier(StatType.FireRate))
            {
                FastBulet.color = Color.green;
                hero.PowerUpWeapon(StatType.FireRate);
            }
            else
            {
                FastBulet.color = Color.white;
                hero.RemovePowerUp(StatType.FireRate);
            }
            
        }
        public void FastHeroButton()
        {
            if (!hero.CheckExistModifier(StatType.CharacterSpeed))
            {
                FastHero.color = Color.green;
                hero.PowerUpWeapon(StatType.CharacterSpeed);
            }
            else
            {
                FastHero.color = Color.white;
                hero.RemovePowerUp(StatType.CharacterSpeed);
            }
        }
        
    }
}

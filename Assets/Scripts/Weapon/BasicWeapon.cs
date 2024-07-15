using UnityEngine;
using Wixot.Stats;

namespace Wixot.Weapon
{
    public abstract class BasicWeapon : MonoBehaviour
    {
        [SerializeField] public BaseStats _baseStats;
        public abstract void StartShooting();
        public abstract void StopShooting();
        public WeaponStats Stats { get; private set; }
        void Awake()
        {
            Stats = new WeaponStats(new StatsMediator(), _baseStats);
        }
        
    }
}

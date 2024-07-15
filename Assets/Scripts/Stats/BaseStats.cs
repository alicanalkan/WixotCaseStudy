using UnityEngine;

namespace Wixot.Stats
{
    public enum StatType
    {
        BulletCount,
        NozzleCount,
        FireRate,
        BulletSpeed,
        CharacterSpeed,
    }
    [CreateAssetMenu(fileName = "BaseStats",menuName = "Stats/BaseStats")]
    public class BaseStats : ScriptableObject
    {
        public int bulletCount;
        public int nozzleCount;
        public int fireRate;
        public int bulletSpeed;
        public int characterSpeed;
    }
}

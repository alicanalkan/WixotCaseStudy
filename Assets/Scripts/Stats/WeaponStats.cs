namespace Wixot.Stats
{
    public class WeaponStats
    {
        private readonly StatsMediator _mediator;
        private readonly BaseStats _baseStats;
        public StatsMediator Mediator => _mediator;
        public WeaponStats(StatsMediator mediator, BaseStats baseStats)
        {
            this._mediator = mediator;
            this._baseStats = baseStats;
        }
        public int BulletSpeed
        {
            get
            {
                var q = new StatsQuery(StatType.BulletSpeed, _baseStats.bulletSpeed);
                _mediator.PerformQuery(this,q);
                return q.Value;
            }
        }
        
        public int CharacterSpeed
        {
            get
            {
                var q = new StatsQuery(StatType.CharacterSpeed, _baseStats.characterSpeed);
                _mediator.PerformQuery(this,q);
                return q.Value;
            }
        }
        
        public int BulletCount
        {
            get
            {
                var q = new StatsQuery(StatType.BulletCount, _baseStats.bulletCount);
                _mediator.PerformQuery(this,q);
                return q.Value;
            }
        }
        
        public int NozzleCount
        {
            get
            {
                var q = new StatsQuery(StatType.NozzleCount, _baseStats.nozzleCount);
                _mediator.PerformQuery(this,q);
                return q.Value;
            }
        }
        
        public int FireRate
        {
            get
            {
                var q = new StatsQuery(StatType.FireRate, _baseStats.fireRate);
                _mediator.PerformQuery(this,q);
                return q.Value;
            }
        }
    }
}

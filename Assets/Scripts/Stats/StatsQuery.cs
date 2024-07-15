namespace Wixot.Stats
{ 
    public class StatsQuery
    {
        public readonly StatType StatType;
        public int Value;
        public StatsQuery(StatType statType, int value)
        {
            StatType = statType;
            Value = value;
        }
    }
}

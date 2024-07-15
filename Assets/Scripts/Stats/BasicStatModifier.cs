using System;

namespace Wixot.Stats
{
    public class BasicStatModifier : StatsModifier
    {
        private readonly StatType type;
        private readonly Func<int, int> operation;

        public BasicStatModifier(StatType statType,Func<int, int> operation) : base()
        {
            this.type = statType;
            this.operation = operation;
        }

        public override void Handle(object sender, StatsQuery query)
        {
            if (query.StatType == type)
            {
                query.Value = operation(query.Value);
            }
        }
    }
}

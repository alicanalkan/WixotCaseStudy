using System;
using System.Collections.Generic;

namespace Wixot.Stats
{
    public class StatsMediator
    {
        private readonly LinkedList<StatsModifier> _modifiers = new();
        public event EventHandler<StatsQuery> Queries;
        public void PerformQuery(object sender, StatsQuery query) => Queries?.Invoke(sender, query);
        public void AddModifier(StatsModifier modifier)
        {
            if (_modifiers.Contains(modifier))
                return;
            modifier.OnDispose += Remove;
            _modifiers.AddLast(modifier);
            Queries += modifier.Handle;
        }

        private void Remove(StatsModifier modifier)
        {
            _modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        }

        public int modifierCount => _modifiers.Count;
        public bool HasModifier(StatsModifier modifier)
        {
            return _modifiers.Contains(modifier);
        }
    }
}

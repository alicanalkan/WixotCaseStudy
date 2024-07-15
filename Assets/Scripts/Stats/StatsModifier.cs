using System;

namespace Wixot.Stats
{
    public abstract class StatsModifier : IDisposable
    {
        public abstract void Handle(object sender, StatsQuery query);
        public event Action<StatsModifier> OnDispose = delegate {  };

        public void Dispose()
        {
            OnDispose.Invoke(this);
        }
    }
}

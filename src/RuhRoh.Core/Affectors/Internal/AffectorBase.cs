using System.Collections.Generic;

namespace RuhRoh.Core.Affectors.Internal
{
    public abstract class AffectorBase : IAffector
    {
        internal AffectorBase() {}

        private readonly List<ITrigger> _triggers = new List<ITrigger>();

        public ITrigger[] Triggers => _triggers.ToArray();

        public void AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public abstract void Affect();
    }
}
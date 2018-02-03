using System.Collections.Generic;

namespace RuhRoh.Affectors.Internal
{
    internal abstract class AffectorBase : IAffector
    {
        private readonly List<ITrigger> _triggers = new List<ITrigger>();

        public ITrigger[] Triggers => _triggers.ToArray();

        public void AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public abstract void Affect();
    }
}
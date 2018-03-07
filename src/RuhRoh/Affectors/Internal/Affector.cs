using System.Collections.Generic;

namespace RuhRoh.Affectors.Internal
{
    /// <summary>
    /// Base class for affectors
    /// </summary>
    public abstract class Affector : IAffector
    {
        private readonly List<ITrigger> _triggers = new List<ITrigger>();

        ITrigger[] IAffector.Triggers => _triggers.ToArray();

        void IAffector.AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
        }

        void IAffector.Affect()
        {
            Affect();
        }

        /// <summary>
        /// Execute the affector
        /// </summary>
        protected internal abstract void Affect();
    }
}
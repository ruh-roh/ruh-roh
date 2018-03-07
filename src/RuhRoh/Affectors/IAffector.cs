namespace RuhRoh.Affectors
{
    /// <summary>
    /// Defines an affector. An affector changes the behavior of a configured call.
    /// </summary>
    public interface IAffector
    {
        /// <summary>
        /// Return the <see cref="ITrigger"/>'s that have been added to this affector.
        /// </summary>
        ITrigger[] Triggers { get; }

        /// <summary>
        /// Add an <see cref="ITrigger"/> to this affector.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        void AddTrigger(ITrigger trigger);
        
        /// <summary>
        /// Execute the affector
        /// </summary>
        void Affect();
    }
}
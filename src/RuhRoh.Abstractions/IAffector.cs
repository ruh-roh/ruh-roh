namespace RuhRoh
{
    /// <summary>
    /// Defines an affector. An affector changes the behavior of a configured call.
    /// </summary>
    public interface IAffector
    {
        /// <summary>
        /// Execute the affector
        /// </summary>
        void Affect();

        /// <summary>
        /// Add an <see cref="ITrigger"/> to this affector.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        void AddTrigger(ITrigger trigger);

        /// <summary>
        /// Return the <see cref="ITrigger"/>'s that have been added to this <see cref="IAffector"/>.
        /// </summary>
        ITrigger[] Triggers { get; }
    }
}
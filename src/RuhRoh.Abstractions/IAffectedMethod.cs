namespace RuhRoh
{
    /// <summary>
    /// Represents an affected method of a service. During runtime, the behavior of the affected method might change because of this.
    /// </summary>
    public interface IAffectedMethod
    {
        /// <summary>
        /// The name of the affected method.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Adds an <see cref="IAffector"/> to this method.
        /// </summary>
        /// <param name="affector"></param>
        /// <returns></returns>
        IAffector AddAffector(IAffector affector);

        /// <summary>
        /// Adds an <see cref="ITrigger"/> to the configured <see cref="IAffector"/>
        /// </summary>
        /// <param name="affector"></param>
        /// <param name="trigger"></param>
        void AddTrigger(IAffector affector, ITrigger trigger);

    }
}

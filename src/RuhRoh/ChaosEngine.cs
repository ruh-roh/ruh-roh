namespace RuhRoh
{
    /// <summary>
    /// Static helper class to get started when configuring services to affect at runtime.
    /// </summary>
    public static class ChaosEngine
    {
        /// <summary>
        /// Affect the behavior of the service.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        public static AffectedType<T> Affect<T>() where T : class
        {
            return new AffectedType<T>();
        }
    }
}
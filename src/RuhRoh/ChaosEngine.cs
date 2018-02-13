using System;

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
        public static AffectedType<T> Affect<T>() where T : class, new()
        {
            return new AffectedType<T>();
        }

        /// <summary>
        /// Affect the behavior of the service.
        /// </summary>
        /// <param name="factoryMethod">Used to create an instance of <typeparamref name="T"/></param>
        /// <typeparam name="T">Type of the service.</typeparam>
        public static AffectedType<T> Affect<T>(Func<T> factoryMethod) where T : class
        {
            return new AffectedType<T>(factoryMethod);
        }
    }
}
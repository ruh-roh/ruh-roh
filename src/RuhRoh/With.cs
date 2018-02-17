namespace RuhRoh
{
    /// <summary>
    /// Utility class to configure argument matchers when configuring affected methods.
    /// </summary>
    public static class With
    {
        /// <summary>
        /// Match any value of <typeparamref name="T"/> passed into the affected method.
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <returns>The default value of <typeparamref name="T"/></returns>
        public static T Any<T>()
        {
            return default;
        }
    }
}
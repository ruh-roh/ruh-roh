namespace RuhRoh.Core
{
    public static class ChaosEngine
    {
        public static AffectedType<T> Affect<T>() where T : class
        {
            return new AffectedType<T>();
        }
    }
}
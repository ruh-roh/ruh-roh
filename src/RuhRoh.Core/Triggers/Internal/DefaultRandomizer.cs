namespace RuhRoh.Core.Triggers.Internal
{
    internal class DefaultRandomizer : IRandomizer
    {
        private static readonly System.Random Rnd = new System.Random();

        public double Next()
        {
            return Rnd.NextDouble();
        }
    }
}
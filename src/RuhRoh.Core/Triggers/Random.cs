using RuhRoh.Core.Triggers.Internal;

namespace RuhRoh.Core.Triggers
{
    public class Random : ITrigger
    {
        private readonly IRandomizer _randomizer;

        public Random()
        {
            _randomizer = new DefaultRandomizer();
        }

        public Random(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public bool WillAffect()
        {
            var nextRnd = _randomizer.Next();
            return nextRnd >= 0.5;
        }
    }
}
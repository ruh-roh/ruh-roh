using RuhRoh.Triggers.Internal;

namespace RuhRoh.Triggers
{
    internal class RandomTrigger : ITrigger
    {
        private readonly IRandomizer _randomizer;

        public RandomTrigger()
        {
            _randomizer = new DefaultRandomizer();
        }

        public RandomTrigger(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        bool ITrigger.WillAffect()
        {
            var nextRnd = _randomizer.Next();
            return nextRnd >= 0.5;
        }
    }
}
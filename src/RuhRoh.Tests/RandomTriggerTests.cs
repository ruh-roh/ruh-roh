using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;
using Xunit;

namespace RuhRoh.Tests
{
    public class RandomTriggerTests
    {
        [Theory]
        [InlineData(0.5)]
        [InlineData(0.51)]
        [InlineData(0.5000000001)]
        [InlineData(0.6)]
        [InlineData(0.9999999999999999999999)]
        public void Random_Should_Trigger_When_Randomizer_Returns_Zero_Point_Five_Or_Higher(double rnd)
        {
            var randomizer = new TestRandomizer(rnd);
            ITrigger t = new RandomTrigger(randomizer);

            var result = t.WillAffect();

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.0000000000001)]
        [InlineData(0.23435345)]
        [InlineData(0.3)]
        [InlineData(0.499999999999999)]
        public void Random_Should_Not_Trigger_When_Randomizer_Returns_Less_Than_Zero_Point_Five(double rnd)
        {
            var randomizer = new TestRandomizer(rnd);
            ITrigger t = new RandomTrigger(randomizer);

            var result = t.WillAffect();

            Assert.False(result);
        }

        private class TestRandomizer : IRandomizer
        {
            private readonly double _next;

            public TestRandomizer(double next)
            {
                _next = next;
            }

            public double Next()
            {
                return _next;
            }
        }
    }
}
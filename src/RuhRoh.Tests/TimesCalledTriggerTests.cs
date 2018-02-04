using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;
using Xunit;

namespace RuhRoh.Tests
{
    public class TimesCalledTriggerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(99)]
        [InlineData(123409287)]
        public void After_Should_Affect_When_Actually_After(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.After, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void After_Should_Not_Affect_When_Not_After(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.After, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }

        [Fact]
        public void After_Should_Not_Affect_When_Not_Called()
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.After, 3);

            t.ActualTimesCalled = 0;
            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(99)]
        [InlineData(123409287)]
        public void Until_Should_Not_Affect_When_After(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.Until, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Until_Should_Affect_When_Actually_Before(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.Until, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.True(result);
        }

        [Fact]
        public void Until_Should_Not_Affect_When_Not_Called()
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.Until, 3);

            t.ActualTimesCalled = 0;
            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }

        [Fact]
        public void At_Should_Affect_When_Exact_Times_Called()
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.At, 3);

            t.ActualTimesCalled = 3;
            var result = ((ITrigger)t).WillAffect();

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(5)]
        public void At_Should_Not_Affect_When_Not_Exact_Times_Called(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.At, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(9)]
        public void EveryXCalls_Should_Affect_When_Multiple_Of_X(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.EveryXCalls, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(7)]
        public void EveryXCalls_Should_Not_Affect_When_Not_Multiple_Of_X(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.EveryXCalls, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }
    }
}
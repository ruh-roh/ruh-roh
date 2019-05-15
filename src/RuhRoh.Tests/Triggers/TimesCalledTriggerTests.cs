using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;
using Xunit;

namespace RuhRoh.Tests.Triggers
{
    public class TimesCalledTriggerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(99)]
        [InlineData(123_409_287)]
        public void After_Should_Affect_When_Actually_After(int timesCalled)
        {
            var t = new TimesCalledTrigger(TimesCalledOperation.After, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.True(result);
        }

        [Fact]
        public void After_Should_Work_When_Going_Beyond_MaxValue()
        {
            var timesCalled = int.MaxValue - 1;
            var t = new TimesCalledTrigger(TimesCalledOperation.After, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            ((IUpdateableTrigger)t).Update(); // int.MaxValue (if Update would still actually increase)
            result = ((ITrigger)t).WillAffect();

            ((IUpdateableTrigger)t).Update(); // int.MinValue
            result = ((ITrigger)t).WillAffect();

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
        [InlineData(123_409_287)]
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
        public void Until_Should_Work_When_Going_Beyond_MaxValue()
        {
            var timesCalled = int.MaxValue - 1;
            var t = new TimesCalledTrigger(TimesCalledOperation.Until, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            ((IUpdateableTrigger)t).Update(); // int.MaxValue (if Update would still actually increase)
            result = ((ITrigger)t).WillAffect();

            ((IUpdateableTrigger)t).Update(); // int.MinValue
            result = ((ITrigger)t).WillAffect();

            Assert.False(result);
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

        [Fact]
        public void EveryXCalls_Should_Affect_When_Multiple_Of_X_Beyond_MaxValue()
        {
            var timesCalled = int.MaxValue - 1;
            var t = new TimesCalledTrigger(TimesCalledOperation.EveryXCalls, 3);

            t.ActualTimesCalled = timesCalled;
            var result = ((ITrigger)t).WillAffect();

            Assert.True(result);

            ((IUpdateableTrigger)t).Update();
            ((ITrigger)t).WillAffect();

            ((IUpdateableTrigger)t).Update();
            ((ITrigger)t).WillAffect();

            ((IUpdateableTrigger)t).Update();
            result = ((ITrigger)t).WillAffect();

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

        [Fact]
        public void Affect_Should_Return_False_When_An_Invalid_Operation_Has_Been_Used()
        {
            var t = new TimesCalledTrigger((TimesCalledOperation)9999, 1);
            t.ActualTimesCalled = 1000;

            var result = ((ITrigger)t).WillAffect();

            Assert.False(result);
        }
    }
}
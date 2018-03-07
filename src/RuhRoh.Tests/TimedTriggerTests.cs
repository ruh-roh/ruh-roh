using System;
using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;
using Xunit;

namespace RuhRoh.Tests
{
    public class TimedTriggerTests
    {
        [Fact]
        public void Timed_Should_Affect_When_Time_Is_After_With_After_Operation()
        {
            ITrigger t = new TimedTrigger(DateTime.Now.AddHours(-1).TimeOfDay, TimedOperation.After);

            var result = t.WillAffect();

            Assert.True(result);
        }

        [Fact]
        public void Timed_Should_Not_Affect_When_Time_Is_Before_With_After_Operation()
        {
            ITrigger t = new TimedTrigger(DateTime.Now.AddHours(1).TimeOfDay, TimedOperation.After);

            var result = t.WillAffect();

            Assert.False(result);
        }

        [Fact]
        public void Timed_Should_Only_Affect_When_Time_Is_Before_With_Before_Operation()
        {
            ITrigger t = new TimedTrigger(DateTime.Now.AddHours(1).TimeOfDay, TimedOperation.Before);

            var result = t.WillAffect();

            Assert.True(result);
        }

        [Fact]
        public void Timed_Should_Not_Affect_When_Time_Is_After_With_Before_Operation()
        {
            ITrigger t = new TimedTrigger(DateTime.Now.AddHours(-1).TimeOfDay, TimedOperation.Before);

            var result = t.WillAffect();

            Assert.False(result);
        }

        [Fact]
        public void Timed_Should_Affect_When_Time_Is_Between()
        {
            ITrigger t = new TimedTrigger(DateTime.Now.AddHours(-1).TimeOfDay, DateTime.Now.AddHours(1).TimeOfDay);

            var result = t.WillAffect();

            Assert.True(result);
        }

        [Fact]
        public void Timed_Should_Not_Affect_When_Time_Is_Not_Between()
        {
            ITrigger t = new TimedTrigger(DateTime.Now.AddHours(1).TimeOfDay, DateTime.Now.AddHours(2).TimeOfDay);

            var result = t.WillAffect();

            Assert.False(result);
        }

        [Fact]
        public void Timed_Should_Throw_An_Exception_When_Until_Is_Before_From_When_Using_Between()
        {
            Assert.Throws<ArgumentException>(() => new TimedTrigger(DateTime.Now.AddHours(1).TimeOfDay, DateTime.Now.AddHours(-1).TimeOfDay));
        }
    }
}
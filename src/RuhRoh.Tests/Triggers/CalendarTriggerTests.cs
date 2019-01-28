using RuhRoh.Triggers;
using System;
using Xunit;

namespace RuhRoh.Tests.Triggers
{
    public class AgendaTriggerTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(-1_000_000)]
        public void Should_Throw_ArgumentOutOfRangeException_When_Duration_Is_Set_Out_Of_Bounds(long ticks)
        {
            var duration = new TimeSpan(ticks);

            Assert.Throws<ArgumentOutOfRangeException>(() => new AgendaTrigger(DateTimeOffset.Now.AddDays(10), duration));
        }

        [Fact]
        public void Should_Trigger_When_The_Time_Falls_Inside_The_Agenda_Item()
        {
            var trigger = new AgendaTrigger(DateTimeOffset.Now.AddMinutes(-20), TimeSpan.FromMinutes(30));
            var result = ((ITrigger)trigger).WillAffect();

            Assert.True(result);
        }

        [Fact]
        public void Should_Not_Trigger_When_The_Time_Falls_Before_The_Agenda_Item()
        {
            var trigger = new AgendaTrigger(DateTimeOffset.Now.AddMinutes(10), TimeSpan.FromMinutes(30));
            var result = ((ITrigger)trigger).WillAffect();

            Assert.False(result);
        }

        [Fact]
        public void Should_Not_Trigger_When_The_Time_Falls_After_The_Agenda_Item()
        {
            var trigger = new AgendaTrigger(DateTimeOffset.Now.AddMinutes(-20), TimeSpan.FromMinutes(15));
            var result = ((ITrigger)trigger).WillAffect();

            Assert.False(result);
        }
    }
}

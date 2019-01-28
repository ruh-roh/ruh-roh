using System;
using FakeItEasy;
using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;
using Xunit;

namespace RuhRoh.Tests.Triggers
{
    public class CombinedTriggerTests
    {
        [Fact]
        public void Should_Throw_ArgumentException_When_Using_The_Not_Operand_Wrongly()
        {
            var trigger = new RandomTrigger();

            Assert.Throws<ArgumentException>(() => new CombinedTrigger(Logical.Not, trigger));
        }

        [Theory]
        [InlineData(Logical.And)]
        [InlineData(Logical.Or)]
        [InlineData(Logical.Xor)]
        internal void Should_Throw_ArgumentNullException_When_The_First_Trigger_Is_Null(Logical operand)
        {
            Assert.Throws<ArgumentNullException>(() => new CombinedTrigger(operand, null));
        }

        [Fact]
        public void Should_Throw_InvalidOperationException_When_Missing_SecondTrigger_For_Not_Operand()
        {
            var trigger = new CombinedTrigger();

            Assert.Throws<InvalidOperationException>(() => trigger.WillAffect());
        }

        [Theory]
        [InlineData(Logical.And)]
        [InlineData(Logical.Or)]
        [InlineData(Logical.Xor)]
        internal void Should_Throw_InvalidOperationException_When_Missing_SecondTrigger_For_And_Or_Xor_Operands(Logical operand)
        {
            var trigger = new CombinedTrigger(operand, new RandomTrigger());

            Assert.Throws<InvalidOperationException>(() => trigger.WillAffect());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Apply_Not_Operand(bool value)
        {
            var fakeTrigger = A.Fake<ITrigger>();
            A.CallTo(() => fakeTrigger.WillAffect()).Returns(value);

            var trigger = new CombinedTrigger {Second = fakeTrigger};

            Assert.NotEqual(value, trigger.WillAffect());
        }

        [Theory]
        [InlineData(Logical.And, true, true, true)]
        [InlineData(Logical.And, true, false, false)]
        [InlineData(Logical.And, false, true, false)]
        [InlineData(Logical.And, false, false, false)]
        [InlineData(Logical.Or, true, true, true)]
        [InlineData(Logical.Or, true, false, true)]
        [InlineData(Logical.Or, false, true, true)]
        [InlineData(Logical.Or, false, false, false)]
        [InlineData(Logical.Xor, true, true, false)]
        [InlineData(Logical.Xor, true, false, true)]
        [InlineData(Logical.Xor, false, true, true)]
        [InlineData(Logical.Xor, false, false, false)]
        internal void Should_Apply_And_Or_Xor_Operands(Logical operand, bool firstTriggerValue, bool secondTriggerValue, bool expected)
        {
            var firstTrigger = A.Fake<ITrigger>();
            A.CallTo(() => firstTrigger.WillAffect()).Returns(firstTriggerValue);
            var secondTrigger = A.Fake<ITrigger>();
            A.CallTo(() => secondTrigger.WillAffect()).Returns(secondTriggerValue);

            var trigger = new CombinedTrigger(operand, firstTrigger) { Second = secondTrigger };

            Assert.Equal(expected, trigger.WillAffect());
        }

        [Fact]
        public void Should_Update_Updateable_Triggers()
        {
            var firstTrigger = A.Fake<IUpdateableTrigger>();
            var secondTrigger = A.Fake<IUpdateableTrigger>();

            var trigger = new CombinedTrigger(Logical.And, firstTrigger) { Second = secondTrigger };
            trigger.Update();

            A.CallTo(() => firstTrigger.Update()).MustHaveHappened(1, Times.Exactly);
            A.CallTo(() => secondTrigger.Update()).MustHaveHappened(1, Times.Exactly);
        }
    }
}
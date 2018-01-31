using System;
using System.Linq;
using RuhRoh.Core.Affectors;
using RuhRoh.Core.Tests.Services;
using Xunit;

namespace RuhRoh.Core.Tests
{
    public class AffectedMethodExtensionsTests
    {
        private readonly AffectedType<DummyService> _affectedType = new AffectedType<DummyService>();

        private AffectedMethod<DummyService, int> GetAffectedMethod()
        {
            return new AffectedMethod<DummyService, int>(_affectedType, null, null, null);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(int.MinValue)]
        public void Slow_Down_Should_Not_Add_A_Delay_When_The_Time_Is_Zero_Or_Lower(int seconds)
        {
            var affectedMethod = GetAffectedMethod();

            affectedMethod.SlowItDownBy(TimeSpan.FromSeconds(seconds));

            Assert.Equal(0, affectedMethod.Affectors.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(int.MaxValue)]
        public void Slow_Down_Should_Add_A_Delay_When_The_Time_Is_Larger_Than_Zero(int seconds)
        {
            var affectedMethod = GetAffectedMethod();

            affectedMethod.SlowItDownBy(TimeSpan.FromSeconds(seconds));

            Assert.Equal(1, affectedMethod.Affectors.Count);

            var affector = affectedMethod.Affectors.First();
            Assert.IsType<Delayer>(affector);
        }

        [Fact]
        public void Throw_Should_Only_Accept_Exceptions_When_Using_The_Type_Overload()
        {
            var affectedMethod = GetAffectedMethod();

            Assert.Throws<ArgumentException>(() => affectedMethod.Throw(typeof(string)));
        }

        [Fact]
        public void Throw_Should_Throw_ArgumentNullException_When_Passing_In_A_Null_Exception_Instance()
        {
            var affectedMethod = GetAffectedMethod();

            Assert.Throws<ArgumentNullException>(() => affectedMethod.Throw((Exception)null));
        }

        [Fact]
        public void Throw_Should_Add_An_ExceptionThrower_When_Using_The_Generic_Overload()
        {
            var affectedMethod = GetAffectedMethod();

            affectedMethod.Throw<InvalidOperationException>();

            Assert.Equal(1, affectedMethod.Affectors.Count);
            Assert.IsType<ExceptionThrower>(affectedMethod.Affectors.First());
        }

        [Fact]
        public void Throw_Should_Add_An_ExceptionThrower_When_Using_The_Generic_Overload_With_Exception_Itself()
        {
            var affectedMethod = GetAffectedMethod();

            affectedMethod.Throw<Exception>();

            Assert.Equal(1, affectedMethod.Affectors.Count);
            Assert.IsType<ExceptionThrower>(affectedMethod.Affectors.First());
        }

        [Fact]
        public void Throw_Should_Add_An_ExceptionThrower_When_Using_The_Type_Overload()
        {
            var affectedMethod = GetAffectedMethod();

            affectedMethod.Throw(typeof(InvalidOperationException));

            Assert.Equal(1, affectedMethod.Affectors.Count);
            Assert.IsType<ExceptionThrower>(affectedMethod.Affectors.First());
        }

        [Fact]
        public void Throw_Should_Add_An_ExceptionThrower_When_Using_The_Exception_Overload()
        {
            var affectedMethod = GetAffectedMethod();

            affectedMethod.Throw(new InvalidOperationException());

            Assert.Equal(1, affectedMethod.Affectors.Count);
            Assert.IsType<ExceptionThrower>(affectedMethod.Affectors.First());
        }
    }
}
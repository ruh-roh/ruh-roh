using System;
using RuhRoh.Tests.Exceptions;
using Xunit;
using RuhRoh.Tests.Services;
using Xunit.Sdk;
using System.Linq;

namespace RuhRoh.Tests
{
    public class AffectedTypeTests
    {
        [Fact]
        public void ChaosEngine_Affect_Should_Return_An_Instance()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();

            // Act
            var service = affectedService.Instance;

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void WhenCalling_Should_Return_The_Same_Affected_Method()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();

            // Act
            var affectedMethod1 = affectedService.WhenCalling(x => x.RetrieveData());
            var affectedMethod2 = affectedService.WhenCalling(x => x.RetrieveData());

            // Assert
            Assert.Equal(affectedMethod1, affectedMethod2);
            Assert.Equal(affectedMethod1.GetHashCode(), affectedMethod2.GetHashCode());
        }

        [Fact]
        public void WhenCalling_Should_Return_A_Different_Affected_Method_When_Arguments_Differ()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());

            // Act
            var affectedMethod1 = affectedService.WhenCalling(x => x.GetItemById(1));
            var affectedMethod2 = affectedService.WhenCalling(x => x.GetItemById(2));

            // Assert
            Assert.NotEqual(affectedMethod1, affectedMethod2);
            Assert.NotEqual(affectedMethod1.GetHashCode(), affectedMethod2.GetHashCode());
        }

        [Fact]
        public void WhenCalling_Should_Return_A_Different_Affected_Method_When_Arguments_Differ_Using_With()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());

            // Act
            var affectedMethod1 = affectedService.WhenCalling(x => x.GetItemById(1));
            var affectedMethod2 = affectedService.WhenCalling(x => x.GetItemById(With.Any<int>()));

            // Assert
            Assert.NotEqual(affectedMethod1, affectedMethod2);
            Assert.NotEqual(affectedMethod1.GetHashCode(), affectedMethod2.GetHashCode());
        }

        [Fact]
        public void ChaosEngine_Affect_Should_Return_An_Instance_When_Using_A_Custom_Factory_Method()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect(() => new DummyService());

            // Act
            var service = affectedService.Instance;

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void ChaosEngine_Affect_Should_Return_An_Instance_When_Using_A_Custom_Factory_Method_For_An_Interface()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());

            // Act
            var service = affectedService.Instance;

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void SlowItDownBy_Should_Slow_Down_A_Method_Call()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .SlowItDownBy(TimeSpan.FromSeconds(5));

            var service = affectedService.Instance;
            var result = 0;

            // Act
            var t = new ExecutionTimer();
            t.Aggregate(() => {
                result = service.RetrieveData();
            });

            // Assert
            Assert.Equal(1, result);
            Assert.InRange(t.Total, 4.8m, 5.2m);
        }

        [Fact]
        public void Affector_Should_Not_Affect_An_Unconfigured_Method()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>();

            var service = affectedService.Instance;

            // Act
            var result = service.RetrieveDataUnaffected();
            
            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>();

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.RetrieveData());
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_After_3rd_Call()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>()
                .AfterNCalls(3);

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Equal(1, service.RetrieveData()); // first call
            Assert.Equal(1, service.RetrieveData()); // second call
            Assert.Equal(1, service.RetrieveData()); // third call

            Assert.Throws<TestException>(() => service.RetrieveData()); // all next calls should throw
            Assert.Throws<TestException>(() => service.RetrieveData());
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_Until_3rd_Call_Occurs()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>()
                .UntilNCalls(3);

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.RetrieveData()); // first should throw
            Assert.Throws<TestException>(() => service.RetrieveData()); // second as well

            Assert.Equal(1, service.RetrieveData()); // third call should pass
            Assert.Equal(1, service.RetrieveData()); // fourth and next also
            Assert.Equal(1, service.RetrieveData());
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_At_3rd_Call()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>()
                .WhenCalledNTimes(3);

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Equal(1, service.RetrieveData()); // first call should pass
            Assert.Equal(1, service.RetrieveData()); // second call also

            Assert.Throws<TestException>(() => service.RetrieveData()); // third should throw

            Assert.Equal(1, service.RetrieveData()); // fourth and next should pass again
            Assert.Equal(1, service.RetrieveData());
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_Every_3rd_Call()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>()
                .EveryNCalls(3);

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Equal(1, service.RetrieveData()); // first call should pass
            Assert.Equal(1, service.RetrieveData()); // second call also

            Assert.Throws<TestException>(() => service.RetrieveData()); // third should throw

            Assert.Equal(1, service.RetrieveData()); // fourth and fifth should pass again
            Assert.Equal(1, service.RetrieveData());

            Assert.Throws<TestException>(() => service.RetrieveData()); // sixth should throw again

            Assert.Equal(1, service.RetrieveData()); // next should pass again
        }

        [Fact]
        public void Throw_Should_Throw_Two_Different_Exception_Types()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<DummyService>();
            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<TestException>()
                .UntilNCalls(3);

            affectedService
                .WhenCalling(x => x.RetrieveData())
                .Throw<SecondaryTestException>()
                .AfterNCalls(3);

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.RetrieveData()); // first and second should throw
            Assert.Throws<TestException>(() => service.RetrieveData());
            
            // Third call should go through
            Assert.Equal(1, service.RetrieveData());

            Assert.Throws<SecondaryTestException>(() => service.RetrieveData()); // fourth and next calls should throw a different exception
            Assert.Throws<SecondaryTestException>(() => service.RetrieveData());
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_For_An_Interface_Based_Service()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());
            affectedService
                .WhenCalling(x => x.GetItems())
                .Throw<TestException>();

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.GetItems());
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_For_An_Interface_Based_Service_Only_Once()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());
            affectedService
                .WhenCalling(x => x.GetItems())
                .Throw<TestException>()
                .UntilNCalls(2);

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.GetItems());

            var items = service.GetItems();
            Assert.NotNull(items);
            Assert.Equal(3, items.Count());
            Assert.Equal(1, items.First().Id);
        }

        [Fact]
        public void Throw_Should_Throw_An_Exception_When_Calling_A_Method_With_Parameters_When_Value_Matches_Exactly()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());
            affectedService
                .WhenCalling(x => x.GetItemById(1))
                .Throw<TestException>();

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.GetItemById(1));
        }

        [Fact]
        public void Throw_Should_Not_Throw_An_Exception_When_Calling_A_Method_With_Parameters_When_Value_Doesnt_Match()
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());
            affectedService
                .WhenCalling(x => x.GetItemById(1))
                .Throw<TestException>();

            var service = affectedService.Instance;

            // Act
            var item = service.GetItemById(2);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(2, item.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Throw_Should_Throw_An_Exception_When_Calling_A_Method_With_Parameters_Configured_With_Any(int id)
        {
            // Arrange
            var affectedService = ChaosEngine.Affect<ITestServiceContract>(() => new TestService());
            affectedService
                .WhenCalling(x => x.GetItemById(With.Any<int>()))
                .Throw<TestException>();

            var service = affectedService.Instance;

            // Act && Assert
            Assert.Throws<TestException>(() => service.GetItemById(id));
        }
    }
}

﻿using System;
using RuhRoh.Core.Tests.Exceptions;
using Xunit;
using RuhRoh.Core.Tests.Services;
using Xunit.Sdk;

namespace RuhRoh.Core.Tests
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
            service.RetrieveData(); // first call
            service.RetrieveData(); // second call
            service.RetrieveData(); // third call

            Assert.Throws<TestException>(() => service.RetrieveData()); // all next calls should throw
            Assert.Throws<TestException>(() => service.RetrieveData());
        }
    }
}

using System;
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
                .SlowItDownBy(TimeSpan.FromMinutes(5));

            var service = affectedService.Instance;
            var result = 0;

            // Act
            var t = new ExecutionTimer();
            t.Aggregate(() => {
                result = service.RetrieveData();
            });

            // Assert
            Assert.Equal(1, result);
            Assert.InRange(t.Total, (decimal)TimeSpan.FromMinutes(5).TotalSeconds, (decimal)TimeSpan.FromMinutes(6).TotalSeconds);
        }
    }
}

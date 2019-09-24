using System.Threading.Tasks;
using AutoFixture;
using Demo.NEO.EventProcessing.Activities;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Moq;

namespace Demo.NEO.EventProcessing.UnitTests.TestBuilders
{
    public static class DurableOrchestrationContextBaseBuilder
    {
        public static Mock<DurableOrchestrationContextBase> BuildContextWithSpecificTorinoImpactGreaterThan0()
        {
            var fixture = new Fixture();
            var contextMock = new Mock<DurableOrchestrationContextBase>(MockBehavior.Strict);
            
            var detectedNeoEvent = fixture.Create<DetectedNeoEvent>();
            contextMock.Setup(ctx => ctx.GetInput<DetectedNeoEvent>())
                .Returns(detectedNeoEvent);

            var kineticEnergyResult = fixture.Build<KineticEnergyResult>()
                .With(k => k.KineticEnergyInMegatonTnt, 1e10f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<KineticEnergyResult>(
                    nameof(EstimateKineticEnergyActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(kineticEnergyResult);
            
            var impactProbabilityResult = fixture.Build<ImpactProbabilityResult>()
                .With(k => k.ImpactProbability, 1f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<ImpactProbabilityResult>(
                    nameof(EstimateImpactProbabilityActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(impactProbabilityResult);

            var torinoImpact = 1;
            var torinoImpactResult = fixture.Build<TorinoIimpactResult>()
                .With(p => p.TorinoImpact, torinoImpact)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<TorinoIimpactResult>(
                    nameof(EstimateTorinoImpactActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<TorinoImpactRequest>()))
                .ReturnsAsync(torinoImpactResult);
            
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync(
                    nameof(StoreProcessedNeoEventActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<ProcessedNeoEvent>()))
                .Returns(Task.CompletedTask);

            return contextMock;
        }
        
         public static Mock<DurableOrchestrationContextBase> BuildContextWithSpecificTorinoImpactEqualTo0()
         {
             
            var fixture = new Fixture();
            var contextMock = new Mock<DurableOrchestrationContextBase>(MockBehavior.Strict);
            
            var detectedNeoEvent = fixture.Create<DetectedNeoEvent>();
            contextMock.Setup(ctx => ctx.GetInput<DetectedNeoEvent>())
                .Returns(detectedNeoEvent);

            var kineticEnergyResult = fixture.Build<KineticEnergyResult>()
                .With(k => k.KineticEnergyInMegatonTnt, 1e10f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<KineticEnergyResult>(
                    nameof(EstimateKineticEnergyActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(kineticEnergyResult);
            
            var impactProbabilityResult = fixture.Build<ImpactProbabilityResult>()
                .With(k => k.ImpactProbability, 1f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<ImpactProbabilityResult>(
                    nameof(EstimateImpactProbabilityActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(impactProbabilityResult);
            
            var torinoImpact = 0;
            var torinoImpactResult = fixture.Build<TorinoIimpactResult>()
                .With(p => p.TorinoImpact, torinoImpact)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<TorinoIimpactResult>(
                    nameof(EstimateTorinoImpactActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<TorinoImpactRequest>()))
                .ReturnsAsync(torinoImpactResult);

            return contextMock;
        }
    }
}
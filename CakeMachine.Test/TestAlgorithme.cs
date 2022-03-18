using System;
using System.Threading;
using System.Threading.Tasks;
using CakeMachine.Fabrication;
using CakeMachine.Simulation;
using Xunit;

namespace CakeMachine.Test
{
    public class TestAlgorithme
    {
        [Fact]
        public async Task TestAlgoOptimisé()
        {
            var singleThread = new Optimisée1Poste();
            var usine = new UsineBuilder().Build();

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await foreach(var _ in singleThread.ProduireAsync(usine, cancellationTokenSource.Token))
            {
            }
        }
    }
}
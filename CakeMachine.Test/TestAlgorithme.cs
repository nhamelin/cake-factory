using System;
using System.Linq;
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
        public void TestSingleThread()
        {
            var singleThread = new SingleThread();
            var usine = new UsineBuilder().Build();

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            singleThread.Produire(usine, cancellationTokenSource.Token).ToArray();
        }

        [Fact]
        public void TestDixParDix()
        {
            var algo = new DixParDix();
            var usine = new UsineBuilder().Build();

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            algo.Produire(usine, cancellationTokenSource.Token).ToArray();

        }
        
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
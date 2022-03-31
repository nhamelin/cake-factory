<<<<<<< HEAD
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CakeMachine.Fabrication;
using CakeMachine.Simulation;
using Xunit;
=======
﻿using System;
using System.Threading.Tasks;
using CakeMachine.Simulation;
using CakeMachine.Simulation.Algorithmes;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
>>>>>>> prof/optimisé

namespace CakeMachine.Test
{
    public class TestAlgorithme
    {
<<<<<<< HEAD
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
=======
        private readonly ITestOutputHelper _testOutputHelper;

        public TestAlgorithme(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(typeof(SingleThread), true)]
        [InlineData(typeof(SingleThread), false)]
        [InlineData(typeof(DeuxParDeux), true)]
        [InlineData(typeof(DeuxParDeux), false)]
        [InlineData(typeof(DixParDix), true)]
        [InlineData(typeof(DixParDix), false)]
        [InlineData(typeof(FourRempli), true)]
        [InlineData(typeof(FourRempli), false)]
        [InlineData(typeof(FourRempliSansRebut), true)]
        [InlineData(typeof(FourRempliSansRebut), false)]
        [InlineData(typeof(AntiRebut), true)]
        [InlineData(typeof(AntiRebut), false)]
        [InlineData(typeof(Optimisée1Poste), false)]
        [InlineData(typeof(UsineEtalon), false)]
        public async Task TestAlgoOptimisé(Type algorithme, bool sync)
        {
            var runner = new SingleAlgorithmRunner(algorithme);
            var result = await runner.ProduirePendantAsync(TimeSpan.FromSeconds(5), sync);
            
            if(result is null) throw new XunitException("No algorithm");
            _testOutputHelper.WriteLine(result.ToString());
>>>>>>> prof/optimisé
        }
    }
}
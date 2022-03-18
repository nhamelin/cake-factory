using System.Runtime.CompilerServices;
using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;
using CakeMachine.Fabrication.Opérations;
using CakeMachine.Utils;

namespace CakeMachine.Simulation
{
    public class SingleThread : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

        /// <inheritdoc />
        public override bool SupportsAsync => false;

        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            var builder = new UsineBuilder();
            builder.NombreEmballeuses = 10;
            builder.NombreFours = 2;
            builder.NombrePréparateurs = 10;
            var usine2 = builder.Build();
            
            var postePréparation = usine2.Préparateurs.Single();
            var posteCuisson = usine2.Fours.Single();
            var posteEmballage = usine2.Emballeuses.Single();

            while (!token.IsCancellationRequested)
            {
                var plats = Enumerable.Range(0, 10).Select(_ => new Plat());
                foreach (var plat in plats)
                {
                    var gâteauCru = plats.AsParallel().Select(postePréparation.Préparer);

                    foreach (var gat in gâteauCru)
                    {
                        var gâteauxCuits = CuireParLots(gat, posteCuisson, capacitéFour);
                    }
                }

                
                var gâteauCuit = posteCuisson.Cuire(gâteauCru).Single();
                var gâteauEmballé = posteEmballage.Emballer(gâteauCuit);
                
                yield return gâteauEmballé;
            }
        }

        /// <inheritdoc />
        public override async IAsyncEnumerable<GâteauEmballé> ProduireAsync(Usine usine, [EnumeratorCancellation] CancellationToken token)
        {
            var postePréparation = usine.Préparateurs.Single();
            var posteCuisson = usine.Fours.Single();
            var posteEmballage = usine.Emballeuses.Single();

            while (!token.IsCancellationRequested)
            {
                var plat = new Plat();

                var gâteauCru = await postePréparation.PréparerAsync(plat);
                var gâteauCuit = (await posteCuisson.CuireAsync(gâteauCru)).Single();
                var gâteauEmballé = await posteEmballage.EmballerAsync(gâteauCuit);

                yield return gâteauEmballé;
            }
        }
    }

    private static IEnumerable<GâteauCuit> CuireParLots(IEnumerable<GâteauCru> gâteaux, Cuisson four, uint capacitéFour)
    {
        var queue = new Queue<GâteauCru>(gâteaux);

        while (queue.Any())
        {
            var gâteauxCuits = four.Cuire(queue.Dequeue(capacitéFour).ToArray());
            foreach (var gâteauCuit in gâteauxCuits)
                yield return gâteauCuit;
        }
    }

}
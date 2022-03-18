using System.Runtime.CompilerServices;
using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;
using CakeMachine.Fabrication.Opérations;
using CakeMachine.Utils;

namespace CakeMachine.Simulation
{
    public class DixParDix : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

        /// <inheritdoc />
        public override bool SupportsAsync => true;

        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            var capacitéFour = usine.OrganisationUsine.ParamètresCuisson.NombrePlaces;

            var postePréparation = usine.Préparateurs.Single();
            var posteEmballage = usine.Emballeuses.Single();
            var posteCuisson = usine.Fours.Single();

            while (!token.IsCancellationRequested)
            {
                var plats = Enumerable.Range(0, 10).Select(_ => new Plat());

                var gâteauxCrus = plats.AsParallel().Select(postePréparation.Préparer);
                var gâteauxCuits = CuireParLots(gâteauxCrus, posteCuisson, capacitéFour);
                var gâteauxEmballés = gâteauxCuits.AsParallel().Select(posteEmballage.Emballer);

                foreach (var gâteauEmballé in gâteauxEmballés)
                    yield return gâteauEmballé;
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

        /// <inheritdoc />
        public override async IAsyncEnumerable<GâteauEmballé> ProduireAsync(Usine usine, [EnumeratorCancellation] CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                var plat = new Plat();
                List<Task<GâteauCru>> ListeGâteauCrusTask = new List<Task<GâteauCru>>();
                List<GâteauCuit> ListeGâteauCuits = new List<GâteauCuit>();
                List<Task<GâteauEmballé>> ListeGâteauEmballésTask = new List<Task<GâteauEmballé>>();

                do
                {
                    var gâteauCruTask = usine.Préparateurs.First().PréparerAsync(plat);
                    ListeGâteauCrusTask.Add(gâteauCruTask);


                } while (ListeGâteauCrusTask.Count < 10);

                do
                {

                    var gâteauxCrus = await Task.WhenAll(ListeGâteauCrusTask);
                    GâteauCru[] lot1 = gâteauxCrus.Take(5).ToArray();
                    GâteauCru[] lot2 = gâteauxCrus.TakeLast(5).ToArray();


                    //var gâteauCuits1 = await gâteauxCrus.Select(usine.Fours.Single().CuireAsync();
                    var gâteauCuits1 = await usine.Fours.Single().CuireAsync(lot1);
                    var gâteauCuits2 = await usine.Fours.Single().CuireAsync(lot2);

                    foreach (GâteauCuit gc in gâteauCuits1)
                    {
                        ListeGâteauCuits.Add(gc);

                    }
                    foreach (GâteauCuit gc in gâteauCuits2)
                    {
                        ListeGâteauCuits.Add(gc);

                    }




                } while (ListeGâteauCuits.Count < 10);

                do
                {
                    foreach (GâteauCuit gc in ListeGâteauCuits)
                    {
                        var gâteauEmballéTask = usine.Emballeuses.First().EmballerAsync(gc);
                        ListeGâteauEmballésTask.Add(gâteauEmballéTask);

                    }
                } while (ListeGâteauEmballésTask.Count < 10);
                var gâteauxEmballés = await Task.WhenAll(ListeGâteauEmballésTask);


                foreach (GâteauEmballé ge in gâteauxEmballés)
                {
                    yield return ge;
                }
            }

        }
    }
}

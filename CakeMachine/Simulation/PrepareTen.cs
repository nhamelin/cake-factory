using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;
using CakeMachine.Fabrication.Opérations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CakeMachine.Simulation
{
    internal class PrepareTen : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

        public override bool SupportsAsync => true;


        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
              
                List<GâteauCuit> ListeGâteauCuits = new List<GâteauCuit>();
                List<GâteauEmballé> ListeGâteauEmballés = new List<GâteauEmballé>();
 
             
                var plats = Enumerable.Range(0, 10).Select(_ => new Plat());
                var gâteauCrus = plats
                .Select(usine.Préparateurs.First().Préparer)
                .AsParallel()
                .ToArray();
                
                GâteauCru[] lot1 = gâteauCrus.Take(5).ToArray();
                GâteauCru[] lot2 = gâteauCrus.TakeLast(5).ToArray();
                var gâteauxCuits1 = usine.Fours.First().Cuire(lot1);
                var gâteauxCuits2 = usine.Fours.First().Cuire(lot2);
                foreach (GâteauCuit gc in gâteauxCuits1)
                {
                    ListeGâteauCuits.Add(gc);

                }
                foreach (GâteauCuit gc in gâteauxCuits2)
                {
                    ListeGâteauCuits.Add(gc);

                }
                var gâteauxEmballés = ListeGâteauCuits.Select(usine.Emballeuses.First().Emballer).ToArray();
                     
                foreach(GâteauEmballé ge in gâteauxEmballés)
                {
                    yield return ge;
                }
            }
        }

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


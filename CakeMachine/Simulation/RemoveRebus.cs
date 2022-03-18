using System.Runtime.CompilerServices;
using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;

namespace CakeMachine.Simulation
{
    internal class RemoveRebus : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;
        
        /// <inheritdoc />
        public override bool SupportsAsync => true;

        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            
            var postePréparation = usine.Préparateurs.Single();
            var posteCuisson = usine.Fours.Single();
            var posteEmballage = usine.Emballeuses.Single();
            
            while (!token.IsCancellationRequested)
            {
                var plats = new[] { new Plat(), new Plat() };
                
                foreach (var p in plats)
                {
                    if (p.EstConforme)
                    {
                        var gâteauxCrus = plats
                            .Select(postePréparation.Préparer)
                            .AsParallel()
                            .ToArray();

                        foreach (var gcru in gâteauxCrus)
                        {
                            if (gcru.EstConforme)
                            {
                                var gâteauxCuits = usine.Fours.First().Cuire(gâteauxCrus);

                                foreach (var gcuit in gâteauxCuits)
                                {
                                    if (gcuit.EstConforme)
                                    {
                                        var gâteauxEmballés = gâteauxCuits
                                            .Select(posteEmballage.Emballer)
                                            .AsParallel();

                                        foreach (var ge in gâteauxEmballés)
                                        {
                                            if (ge.EstConforme)
                                            {
                                                yield return ge;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }

        /// <inheritdoc />
        public override async IAsyncEnumerable<GâteauEmballé> ProduireAsync(Usine usine,
            [EnumeratorCancellation] CancellationToken token)
        {
            var postePréparation = usine.Préparateurs.Single();
            var posteCuisson = usine.Fours.Single();
            var posteEmballage = usine.Emballeuses.Single();
            
            while (!token.IsCancellationRequested)
            {
                var plat = new Plat();
                
                if (plat.EstConforme)
                {
                    var gâteauCruTask = postePréparation.PréparerAsync(plat);
                    
                    var gâteauxCrus = await Task.WhenAll(gâteauCruTask);

                    if (gâteauxCrus[0].EstConforme)
                    {
                        var gâteauxCuits = await posteCuisson.CuireAsync(gâteauxCrus);

                        if (gâteauxCuits[0].EstConforme)
                        {
                            var gâteauEmballéTask = posteEmballage.EmballerAsync(gâteauxCuits[0]);

                            var gâteauxEmballés = await Task.WhenAll(gâteauEmballéTask);
                            
                            if (gâteauxEmballés[0].EstConforme)
                            {
                                yield return gâteauxEmballés[0];
                            }
                        }
                    }
                }
            }
        }
    }
}



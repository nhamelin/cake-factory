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
            while (!token.IsCancellationRequested)
            {
                var plat = new Plat();

                if (plat.EstConforme)
                {
                    var gâteauCru = usine.Préparateurs.First().Préparer(plat);

                    if (gâteauCru.EstConforme)
                    {
                        var gâteauCuit = usine.Fours.First().Cuire(gâteauCru).Single();

                        if (gâteauCuit.EstConforme)
                        {
                            var gâteauEmballé = usine.Emballeuses.First().Emballer(gâteauCuit);

                            if (gâteauEmballé.EstConforme)
                            {
                                yield return gâteauEmballé;
                            }
                            else
                            {
                                plat = new Plat();
                            }
                        }
                        else
                        {
                            plat = new Plat();
                        }
                    }
                    else
                    {
                        plat = new Plat();
                    }
                }
                else
                {
                    plat = new Plat();
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
                    var gâteauCru = usine.Préparateurs.First().Préparer(plat);

                    if (gâteauCru.EstConforme)
                    {
                        var gâteauCuit = usine.Fours.First().Cuire(gâteauCru).Single();

                        if (gâteauCuit.EstConforme)
                        {
                            var gâteauEmballé = usine.Emballeuses.First().Emballer(gâteauCuit);

                            if (gâteauEmballé.EstConforme)
                            {
                                yield return gâteauEmballé;
                            }
                            else
                            {
                                plat = new Plat();
                            }
                        }
                        else
                        {
                            plat = new Plat();
                        }
                    }
                    else
                    {
                        plat = new Plat();
                    }
                }
                else
                {
                    plat = new Plat();
                }
            }
        }
    }
}



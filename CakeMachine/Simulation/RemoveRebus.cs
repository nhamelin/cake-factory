using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;

namespace CakeMachine.Simulation
{
    internal class RemoveRebus : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

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
    }
}

/*
 var plat = new Plat();
                do
                {
                    plat = new Plat();
                } while (!plat.EstConforme);
                var gâteauCru = usine.Préparateurs.First().Préparer(plat);
                do
                {
                    gâteauCru = usine.Préparateurs.First().Préparer(plat);
                } while (!gâteauCru.EstConforme);
                var gâteauCuit = usine.Fours.First().Cuire(gâteauCru).Single();
                do
                {
                    gâteauCuit = usine.Fours.First().Cuire(gâteauCru).Single();
                } while (!gâteauCuit.EstConforme);
                var gâteauEmballé = usine.Emballeuses.First().Emballer(gâteauCuit);
                do
                {
                    gâteauEmballé = usine.Emballeuses.First().Emballer(gâteauCuit);
                } while (!gâteauEmballé.EstConforme);
                yield return gâteauEmballé;
 */

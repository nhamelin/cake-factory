using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;

namespace CakeMachine.Simulation
{
    internal class DeuxParDeux : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            var postePréparation = usine.Préparateurs.Single();
            var posteCuisson = usine.Fours.Single();
            var posteEmballage = usine.Emballeuses.Single();

            while (!token.IsCancellationRequested)
            {
                var plat1 = new Plat();
                var plat2 = new Plat();

                var gâteauCru1 = postePréparation.Préparer(plat1);
                var gâteauCru2 = postePréparation.Préparer(plat2);

                var gâteauxCuits = posteCuisson.Cuire(gâteauCru1, gâteauCru2);

                var gâteauEmballé1 = posteEmballage.Emballer(gâteauxCuits.First());
                var gâteauEmballé2 = posteEmballage.Emballer(gâteauxCuits.Last());

                yield return gâteauEmballé1;
                yield return gâteauEmballé2;
            }
        }
    }
}

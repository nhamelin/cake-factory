using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;

namespace CakeMachine.Simulation
{
    internal class FourRempli : Algorithme
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
                var plats = Enumerable.Range(0, usine.OrganisationUsine.ParamètresCuisson.NombrePlaces)
                    .Select(_ => new Plat());

                var gâteauxCrus = plats.Select(postePréparation.Préparer);
                var gâteauxCuits = posteCuisson.Cuire(gâteauxCrus.ToArray());
                var gâteauxEmballés = gâteauxCuits.Select(posteEmballage.Emballer).ToArray();

                foreach (var gâteauEmballé in gâteauxEmballés)
                    yield return gâteauEmballé;
            }
        }
    }
}

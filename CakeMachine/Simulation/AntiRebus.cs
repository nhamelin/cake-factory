using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;

namespace CakeMachine.Simulation
{
    internal class AntiRebus : Algorithme
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
                var plat = new Plat();

                GâteauCru gâteauCru;
                do gâteauCru = postePréparation.Préparer(plat);
                while (!gâteauCru.EstConforme);
                
                var gâteauCuit = posteCuisson.Cuire(gâteauCru).Single();
                if(!gâteauCuit.EstConforme) continue;

                var gâteauEmballé = posteEmballage.Emballer(gâteauCuit);
                if (!gâteauEmballé.EstConforme) continue;

                yield return gâteauEmballé;
            }
        }
    }
}

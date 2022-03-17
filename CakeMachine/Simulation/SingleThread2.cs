using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;
using CakeMachine.Fabrication.Opérations;

namespace CakeMachine.Simulation
{
    internal class SingleThread2 : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var plat = new Plat();
                List<GâteauCru> L=new List<GâteauCru>();
                
                do
                {
                    var gâteauCru = usine.Préparateurs.First().Préparer(plat);
                    L.Add(gâteauCru);
                } while (L.Count < 5);


                foreach (GâteauCru g in L) {
                    var gâteauCuit = usine.Fours.First().Cuire(g);
                    foreach (GâteauCuit gateau in gâteauCuit)
                    {
                        var gâteauEmballé = usine.Emballeuses.First().Emballer(gateau);
                        yield return gâteauEmballé;
                    }

                   
                }

               
                
            }
        }
    }
}

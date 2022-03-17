using CakeMachine.Fabrication;
using CakeMachine.Fabrication.Elements;

namespace CakeMachine.Simulation
{
    internal class PrepareTen : Algorithme
    {
        /// <inheritdoc />
        public override bool SupportsSync => true;

        /// <inheritdoc />
        public override IEnumerable<GâteauEmballé> Produire(Usine usine, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var plat = new Plat();
                List<GâteauCru> ListeGâteauCrus = new List<GâteauCru>();
                List<GâteauCuit> ListeGâteauCuits = new List<GâteauCuit>();
                List<GâteauEmballé> ListeGâteauEmballés = new List<GâteauEmballé>();

                do
                {
                    var gâteauCru = usine.Préparateurs.First().Préparer(plat);
                    ListeGâteauCrus.Add(gâteauCru);
                } while (ListeGâteauCrus.Count == 10 );
                do
                {
                    foreach (GâteauCru g in ListeGâteauCrus)
                    {

                        var gâteauCuit = usine.Fours.First().Cuire(g).Single();
                        ListeGâteauCuits.Add(gâteauCuit);
                    }
                } while (ListeGâteauCuits.Count == 10 );
                do
                {
                    foreach (GâteauCuit gc in ListeGâteauCuits)
                    {
                        var gâteauEmballé = usine.Emballeuses.First().Emballer(gc);
                        ListeGâteauEmballés.Add(gâteauEmballé);
                    }
                } while (ListeGâteauCrus.Count == 10 );


             
                foreach(GâteauEmballé ge in ListeGâteauEmballés)
                {
                    yield return ge;
                }
                   
                

            }
        }
    }
}


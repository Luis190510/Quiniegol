using Quiniegol.Models;

namespace Quiniegol.Strategies
{
    public class ReglaSinAcierto : ReglaPuntaje
    {
        public override bool Aplica(
            Pronostico pronostico,
            Partido partido)
        {
            return true;
        }

        public override int ObtenerPuntos()
        {
            return 0;
        }
    }
}
using Quiniegol.Models;

namespace Quiniegol.Strategies
{
    public class ReglaMarcadorExacto : ReglaPuntaje
    {
        public override bool Aplica(
            Pronostico pronostico,
            Partido partido)
        {
            if (!partido.GolesLocal.HasValue ||
                !partido.GolesVisitante.HasValue)
            {
                return false;
            }

            return
                pronostico.GolesLocalPronosticados ==
                partido.GolesLocal.Value &&
                pronostico.GolesVisitantePronosticados ==
                partido.GolesVisitante.Value;
        }

        public override int ObtenerPuntos()
        {
            return 5;
        }
    }
}
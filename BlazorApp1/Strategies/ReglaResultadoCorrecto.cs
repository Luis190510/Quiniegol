using Quiniegol.Models;

namespace Quiniegol.Strategies
{
    public class ReglaResultadoCorrecto : ReglaPuntaje
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

            bool marcadorExacto =
                pronostico.GolesLocalPronosticados ==
                partido.GolesLocal.Value &&
                pronostico.GolesVisitantePronosticados ==
                partido.GolesVisitante.Value;

            if (marcadorExacto)
            {
                return false;
            }

            int resultadoPronosticado =
                ObtenerTipoResultado(
                    pronostico.GolesLocalPronosticados,
                    pronostico.GolesVisitantePronosticados
                );

            int resultadoReal =
                ObtenerTipoResultado(
                    partido.GolesLocal.Value,
                    partido.GolesVisitante.Value
                );

            return resultadoPronosticado ==
                   resultadoReal;
        }

        public override int ObtenerPuntos()
        {
            return 2;
        }
    }
}
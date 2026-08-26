using Quiniegol.Models;

namespace Quiniegol.Strategies
{
 
    /// Regla general para calcularlos puntos obtenidos por un pronóstico.

 
    public abstract class ReglaPuntaje
    {
        public abstract bool Aplica(
            Pronostico pronostico,
            Partido partido);

        public abstract int ObtenerPuntos();

        protected static int ObtenerTipoResultado(
            int golesLocal,
            int golesVisitante)
        {
            if (golesLocal > golesVisitante)
            {
                return 1;
            }

            if (golesVisitante > golesLocal)
            {
                return 2;
            }

            return 0;
        }
    }
}
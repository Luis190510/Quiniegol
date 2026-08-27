using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;

namespace Quiniegol.Strategies
{
    public class CalculadoraPuntajeService
    {
        private readonly List<ReglaPuntaje>
            _reglas;

        public CalculadoraPuntajeService()
        {
            _reglas = new List<ReglaPuntaje>
            {
                new ReglaMarcadorExacto(),
                new ReglaResultadoCorrecto(),
                new ReglaSinAcierto()
            };
        }

        public int Calcular(
            Pronostico pronostico,
            Partido partido)
        {
            ReglaPuntaje regla =
                _reglas.First(reglaActual =>
                    reglaActual.Aplica(
                        pronostico,
                        partido
                    )
                );

            return regla.ObtenerPuntos();
        }
    }
}
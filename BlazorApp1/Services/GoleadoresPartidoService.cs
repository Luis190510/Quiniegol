using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Aplica la fecha simulada indirectamente mediante el estado del partido.</summary>
    public static class GoleadoresPartidoService
    {
        /// <summary>
        /// Devuelve goles oficiales únicamente cuando el partido ya está finalizado.
        /// </summary>
        public static List<GoleadorReal> ObtenerVisibles(
            Partido partido,
            IEnumerable<GoleadorReal> goleadores)
        {
            ArgumentNullException.ThrowIfNull(partido);

            return partido.Estado == "Finalizado"
                ? goleadores
                    .Where(goleador => goleador.PartidoId == partido.Id)
                    .ToList()
                : new List<GoleadorReal>();
        }
    }
}

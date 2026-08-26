using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Calcula métricas reutilizables para las insignias de pronósticos.</summary>
    public static class MetricasInsigniasService
    {
        /// <summary>
        /// Cuenta cada cantidad de goles local o visitante acertada exactamente.
        /// Un marcador completamente exacto aporta dos aciertos.
        /// </summary>
        public static Dictionary<int, int> ContarGolesExactos(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos)
        {
            ArgumentNullException.ThrowIfNull(pronosticos);
            ArgumentNullException.ThrowIfNull(partidos);

            Dictionary<int, int> aciertosPorUsuario = new();
            foreach (Pronostico pronostico in pronosticos)
            {
                if (!partidos.TryGetValue(
                        pronostico.PartidoId,
                        out Partido? partido) ||
                    !PartidoTieneResultado(partido))
                {
                    continue;
                }

                int aciertos = 0;
                if (pronostico.GolesLocalPronosticados == partido.GolesLocal)
                {
                    aciertos++;
                }

                if (pronostico.GolesVisitantePronosticados == partido.GolesVisitante)
                {
                    aciertos++;
                }

                if (aciertos > 0)
                {
                    aciertosPorUsuario[pronostico.UsuarioId] =
                        aciertosPorUsuario.GetValueOrDefault(
                            pronostico.UsuarioId) + aciertos;
                }
            }

            return aciertosPorUsuario;
        }

        /// <summary>
        /// Cuenta los jugadores seleccionados que realmente anotaron en cada partido.
        /// Cada jugador acertado se cuenta una vez, aunque haya marcado varios goles.
        /// </summary>
        public static Dictionary<int, int> ContarGoleadoresAcertados(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IEnumerable<GoleadorReal> goleadores)
        {
            ArgumentNullException.ThrowIfNull(pronosticos);
            ArgumentNullException.ThrowIfNull(partidos);
            ArgumentNullException.ThrowIfNull(goleadores);

            Dictionary<(int PartidoId, int SeleccionId), HashSet<string>>
                goleadoresPorPartido = goleadores
                    .GroupBy(goleador => (
                        goleador.PartidoId,
                        goleador.SeleccionId))
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo
                            .Select(goleador => NormalizarNombre(goleador.Jugador))
                            .Where(nombre => nombre.Length > 0)
                            .ToHashSet(StringComparer.OrdinalIgnoreCase));
            Dictionary<int, int> aciertosPorUsuario = new();

            foreach (Pronostico pronostico in pronosticos)
            {
                if (!partidos.TryGetValue(
                        pronostico.PartidoId,
                        out Partido? partido) ||
                    !PartidoTieneResultado(partido))
                {
                    continue;
                }

                int aciertos = ContarCoincidencias(
                    pronostico.GoleadoresLocalPronosticados,
                    goleadoresPorPartido.GetValueOrDefault((
                        partido.Id,
                        partido.SeleccionLocalId))) +
                    ContarCoincidencias(
                        pronostico.GoleadoresVisitantePronosticados,
                        goleadoresPorPartido.GetValueOrDefault((
                            partido.Id,
                            partido.SeleccionVisitanteId)));

                if (aciertos > 0)
                {
                    aciertosPorUsuario[pronostico.UsuarioId] =
                        aciertosPorUsuario.GetValueOrDefault(
                            pronostico.UsuarioId) + aciertos;
                }
            }

            return aciertosPorUsuario;
        }

        private static int ContarCoincidencias(
            IEnumerable<string>? pronosticados,
            HashSet<string>? reales)
        {
            if (reales == null || reales.Count == 0)
            {
                return 0;
            }

            return (pronosticados ?? Enumerable.Empty<string>())
                .Select(NormalizarNombre)
                .Where(nombre => nombre.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count(reales.Contains);
        }

        private static string NormalizarNombre(string nombre)
        {
            return (nombre ?? string.Empty)
                .Replace(" (autogol)", string.Empty)
                .Replace(" (penal)", string.Empty)
                .Trim();
        }

        private static bool PartidoTieneResultado(Partido partido)
        {
            return partido.Estado == "Finalizado" &&
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue;
        }
    }
}

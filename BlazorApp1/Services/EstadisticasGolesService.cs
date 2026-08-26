using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Calcula los extremos de goles por selección en un conjunto de partidos.</summary>
    public static class EstadisticasGolesService
    {
        /// <summary>Obtiene la selección o selecciones con más goles.</summary>
        public static string ObtenerConMasGoles(
            IEnumerable<Partido> partidos,
            IEnumerable<Seleccion> selecciones)
        {
            return ObtenerExtremo(partidos, selecciones, buscarMayor: true);
        }

        /// <summary>Obtiene la selección o selecciones con menos goles.</summary>
        public static string ObtenerConMenosGoles(
            IEnumerable<Partido> partidos,
            IEnumerable<Seleccion> selecciones)
        {
            return ObtenerExtremo(partidos, selecciones, buscarMayor: false);
        }

        private static string ObtenerExtremo(
            IEnumerable<Partido> partidos,
            IEnumerable<Seleccion> selecciones,
            bool buscarMayor)
        {
            List<Partido> finalizados = (partidos ?? Array.Empty<Partido>())
                .Where(partido =>
                    partido.Estado == "Finalizado" &&
                    partido.GolesLocal.HasValue &&
                    partido.GolesVisitante.HasValue)
                .ToList();

            if (finalizados.Count == 0)
            {
                return "Sin partidos finalizados";
            }

            Dictionary<int, int> golesPorSeleccion = new();

            foreach (Partido partido in finalizados)
            {
                golesPorSeleccion.TryAdd(partido.SeleccionLocalId, 0);
                golesPorSeleccion.TryAdd(partido.SeleccionVisitanteId, 0);
                golesPorSeleccion[partido.SeleccionLocalId] +=
                    partido.GolesLocal ?? 0;
                golesPorSeleccion[partido.SeleccionVisitanteId] +=
                    partido.GolesVisitante ?? 0;
            }

            int cantidadObjetivo = buscarMayor
                ? golesPorSeleccion.Values.Max()
                : golesPorSeleccion.Values.Min();
            Dictionary<int, string> nombres =
                (selecciones ?? Array.Empty<Seleccion>())
                    .ToDictionary(
                        seleccion => seleccion.Id,
                        seleccion => seleccion.Nombre);
            List<string> equipos = golesPorSeleccion
                .Where(elemento => elemento.Value == cantidadObjetivo)
                .Select(elemento => nombres.GetValueOrDefault(
                    elemento.Key,
                    $"Selección {elemento.Key}"))
                .OrderBy(nombre => nombre)
                .ToList();

            string etiquetaGoles = cantidadObjetivo == 1
                ? "gol"
                : "goles";

            return $"{string.Join(", ", equipos)} " +
                   $"({cantidadObjetivo} {etiquetaGoles})";
        }
    }
}

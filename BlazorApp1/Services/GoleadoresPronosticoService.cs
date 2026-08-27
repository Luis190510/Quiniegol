using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Normaliza y presenta los goleadores elegidos en un pronóstico.</summary>
    public static class GoleadoresPronosticoService
    {
        /// <summary>Elimina nombres vacíos y repetidos sin alterar su orden.</summary>
        public static List<string> Normalizar(IEnumerable<string>? nombres)
        {
            List<string> resultado = new();
            HashSet<string> vistos = new(StringComparer.OrdinalIgnoreCase);

            foreach (string nombre in nombres ?? Array.Empty<string>())
            {
                string limpio = (nombre ?? string.Empty).Trim();

                if (limpio.Length > 0 && vistos.Add(limpio))
                {
                    resultado.Add(limpio);
                }
            }

            return resultado;
        }

        /// <summary>Indica si el pronóstico incluye al menos un goleador.</summary>
        public static bool TieneGoleadores(Pronostico pronostico)
        {
            ArgumentNullException.ThrowIfNull(pronostico);

            return (pronostico.GoleadoresLocalPronosticados?.Count ?? 0) > 0 ||
                   (pronostico.GoleadoresVisitantePronosticados?.Count ?? 0) > 0;
        }

        /// <summary>Construye un texto legible con los goleadores de ambos equipos.</summary>
        public static string Formatear(
            Pronostico pronostico,
            string nombreLocal,
            string nombreVisitante)
        {
            ArgumentNullException.ThrowIfNull(pronostico);

            List<string> partes = new();
            List<string> locales =
                pronostico.GoleadoresLocalPronosticados ?? new List<string>();
            List<string> visitantes =
                pronostico.GoleadoresVisitantePronosticados ?? new List<string>();

            if (locales.Count > 0)
            {
                partes.Add(
                    $"{nombreLocal}: " +
                    string.Join(", ", locales)
                );
            }

            if (visitantes.Count > 0)
            {
                partes.Add(
                    $"{nombreVisitante}: " +
                    string.Join(", ", visitantes)
                );
            }

            return partes.Count == 0
                ? "Sin goleadores elegidos"
                : string.Join(" | ", partes);
        }
    }
}

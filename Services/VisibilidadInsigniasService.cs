namespace Quiniegol.Services
{
    /// <summary>Separa insignias globales de las pertenecientes a una quiniela.</summary>
    public static class VisibilidadInsigniasService
    {
        private const string LiderPrivado = "Líder de quiniela:";
        private const string UltimoPrivado = "Peor de quiniela:";
        private const string PrecisionPrivada =
            "Precisión goleadora de quiniela:";
        private const string CazagoleadoresPrivado =
            "Cazagoleadores de quiniela:";

        /// <summary>Obtiene solamente insignias aptas para el ranking global.</summary>
        public static IEnumerable<string> ObtenerGlobales(
            IEnumerable<string>? insignias)
        {
            return (insignias ?? Enumerable.Empty<string>())
                .Where(insignia => !EsPrivada(insignia));
        }

        /// <summary>Obtiene las insignias creadas para una quiniela concreta.</summary>
        public static IEnumerable<string> ObtenerDeQuiniela(
            IEnumerable<string>? insignias,
            string nombreQuiniela)
        {
            string sufijo = $": {nombreQuiniela}";

            return (insignias ?? Enumerable.Empty<string>())
                .Where(insignia =>
                    EsPrivada(insignia) &&
                    insignia.EndsWith(
                        sufijo,
                        StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Obtiene todas las insignias pertenecientes a quinielas privadas.</summary>
        public static IEnumerable<string> ObtenerPrivadas(
            IEnumerable<string>? insignias)
        {
            return (insignias ?? Enumerable.Empty<string>())
                .Where(EsPrivada);
        }

        private static bool EsPrivada(string insignia)
        {
            return insignia.StartsWith(
                       LiderPrivado,
                       StringComparison.OrdinalIgnoreCase) ||
                   insignia.StartsWith(
                       UltimoPrivado,
                       StringComparison.OrdinalIgnoreCase) ||
                   insignia.StartsWith(
                       PrecisionPrivada,
                       StringComparison.OrdinalIgnoreCase) ||
                   insignia.StartsWith(
                       CazagoleadoresPrivado,
                       StringComparison.OrdinalIgnoreCase);
        }
    }
}

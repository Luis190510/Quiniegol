namespace Quiniegol.Services
{
    /// <summary>
    /// Resuelve los archivos internos de Quiniegol desde la raíz de la
    /// aplicación Blazor.
    /// </summary>
    public static class RutaDatosService
    {
        private static string? _carpetaAplicacion;

        /// <summary>
        /// Configura la carpeta que contiene Data y los demás recursos internos.
        /// </summary>
        public static void Configurar(string carpetaAplicacion)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(carpetaAplicacion);

            string rutaCompleta = Path.GetFullPath(carpetaAplicacion);
            string carpetaData = Path.Combine(rutaCompleta, "Data");

            if (!Directory.Exists(carpetaData))
            {
                throw new DirectoryNotFoundException(
                    $"No se encontró la carpeta de datos: {carpetaData}");
            }

            _carpetaAplicacion = rutaCompleta;
        }

        public static string ObtenerRuta(string nombreArchivo)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nombreArchivo);

            if (!string.Equals(
                    nombreArchivo,
                    Path.GetFileName(nombreArchivo),
                    StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "Debe indicar únicamente el nombre del archivo.",
                    nameof(nombreArchivo));
            }

            return Path.Combine(
                ObtenerCarpetaAplicacion(),
                "Data",
                nombreArchivo);
        }

        public static string ObtenerRutaRecurso(string rutaRelativa)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(rutaRelativa);

            string rutaNormalizada = rutaRelativa
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            return Path.Combine(
                ObtenerCarpetaAplicacion(),
                rutaNormalizada);
        }

        private static string ObtenerCarpetaAplicacion()
        {
            return _carpetaAplicacion
                ?? throw new InvalidOperationException(
                    "La ruta de datos no fue configurada al iniciar Blazor.");
        }
    }
}

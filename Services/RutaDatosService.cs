using System;
using System.IO;

namespace Quiniegol.Services
{
    public static class RutaDatosService
    {
        private static string ObtenerCarpetaProyecto()
        {
            return Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..",
                    "..",
                    ".."
                )
            );
        }

        public static string ObtenerRuta(
            string nombreArchivo)
        {
            string carpetaData =
                Path.Combine(
                    ObtenerCarpetaProyecto(),
                    "Data"
                );

            if (!Directory.Exists(carpetaData))
            {
                Directory.CreateDirectory(
                    carpetaData
                );
            }

            return Path.Combine(
                carpetaData,
                nombreArchivo
            );
        }

        public static string ObtenerRutaRecurso(
            string rutaRelativa)
        {
            string rutaNormalizada =
                rutaRelativa
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar
                    )
                    .Replace(
                        '\\',
                        Path.DirectorySeparatorChar
                    );

            return Path.Combine(
                ObtenerCarpetaProyecto(),
                rutaNormalizada
            );
        }
    }
}

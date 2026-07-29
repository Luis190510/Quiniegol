using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;

namespace Quiniegol.Services
{
    public static class RutaDatosService
    {
        public static string ObtenerRuta(
            string nombreArchivo)
        {
            string carpetaProyecto =
                Path.GetFullPath(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "..",
                        "..",
                        ".."
                    )
                );

            string carpetaData =
                Path.Combine(
                    carpetaProyecto,
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
    }
}
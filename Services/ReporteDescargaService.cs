using System.Text;
using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>
    /// Convierte los reportes visibles en archivos CSV o TXT codificados en UTF-8.
    /// </summary>
    public static class ReporteDescargaService
    {
        private static readonly Encoding Utf8ConMarca = new UTF8Encoding(
            encoderShouldEmitUTF8Identifier: true);

        /// <summary>Genera un documento CSV con encabezados y campos escapados.</summary>
        public static string GenerarCsv(IEnumerable<EstadisticaItem> reporte)
        {
            ArgumentNullException.ThrowIfNull(reporte);
            var lineas = new List<string> { "Reporte,Resultado" };
            lineas.AddRange(reporte.Select(item =>
                $"{EscaparCsv(item.Estadistica)},{EscaparCsv(item.Resultado)}"));
            return string.Join(Environment.NewLine, lineas);
        }

        /// <summary>Genera un documento TXT legible con cada indicador y resultado.</summary>
        public static string GenerarTxt(IEnumerable<EstadisticaItem> reporte)
        {
            ArgumentNullException.ThrowIfNull(reporte);
            List<EstadisticaItem> elementos = reporte.ToList();
            var texto = new StringBuilder();
            texto.AppendLine("REPORTE QUINEGOL");
            texto.AppendLine("=================");

            for (int indice = 0; indice < elementos.Count; indice++)
            {
                texto.AppendLine($"{indice + 1}. {elementos[indice].Estadistica}");
                texto.AppendLine($"   {elementos[indice].Resultado}");
                texto.AppendLine();
            }

            return texto.ToString();
        }

        /// <summary>Guarda un reporte CSV en la ruta elegida por el usuario.</summary>
        public static void GuardarCsv(string ruta, IEnumerable<EstadisticaItem> reporte)
        {
            ValidarRuta(ruta);
            File.WriteAllText(ruta, GenerarCsv(reporte), Utf8ConMarca);
        }

        /// <summary>Guarda un reporte TXT en la ruta elegida por el usuario.</summary>
        public static void GuardarTxt(string ruta, IEnumerable<EstadisticaItem> reporte)
        {
            ValidarRuta(ruta);
            File.WriteAllText(ruta, GenerarTxt(reporte), Utf8ConMarca);
        }

        private static string EscaparCsv(string? valor)
        {
            string seguro = valor ?? string.Empty;
            string contenidoSinEspacios = seguro.TrimStart();
            if (contenidoSinEspacios.StartsWith('=') ||
                contenidoSinEspacios.StartsWith('+') ||
                contenidoSinEspacios.StartsWith('-') ||
                contenidoSinEspacios.StartsWith('@'))
            {
                seguro = $"'{seguro}";
            }

            return $"\"{seguro.Replace("\"", "\"\"")}\"";
        }

        private static void ValidarRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                throw new ArgumentException("Debe seleccionar una ruta de destino.", nameof(ruta));
            }
        }
    }
}

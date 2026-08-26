using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>
    /// Convierte los reportes visibles en archivos CSV, TXT o PDF.
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

        /// <summary>Guarda un reporte sencillo en formato PDF.</summary>
        public static void GuardarPdf(
            string ruta,
            IEnumerable<EstadisticaItem> reporte)
        {
            ValidarRuta(ruta);
            ArgumentNullException.ThrowIfNull(reporte);

            List<EstadisticaItem> elementos = reporte.ToList();
            var documento = new Document();
            Section seccion = documento.AddSection();
            Style estiloNormal = documento.Styles["Normal"]
                ?? throw new InvalidOperationException(
                    "No se pudo preparar el formato del PDF.");

            documento.Info.Title = "Reporte Quiniegol";
            estiloNormal.Font.Name = "Arial";
            estiloNormal.Font.Size = 10;

            Paragraph titulo = seccion.AddParagraph("REPORTE QUINEGOL");
            titulo.Format.Font.Size = 18;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceAfter = Unit.FromCentimeter(0.8);

            for (int indice = 0; indice < elementos.Count; indice++)
            {
                EstadisticaItem elemento = elementos[indice];
                Paragraph parrafo = seccion.AddParagraph();
                parrafo.AddFormattedText(
                    $"{indice + 1}. {elemento.Estadistica}",
                    TextFormat.Bold);
                parrafo.AddLineBreak();
                parrafo.AddText(elemento.Resultado ?? string.Empty);
                parrafo.Format.SpaceAfter = Unit.FromCentimeter(0.5);
            }

            var renderer = new PdfDocumentRenderer
            {
                Document = documento
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(ruta);
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

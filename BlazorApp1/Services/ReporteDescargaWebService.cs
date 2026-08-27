using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>Convierte un reporte web en CSV, TXT o PDF.</summary>
    public static class ReporteDescargaWebService
    {
        public static byte[] GenerarCsv(IEnumerable<EstadisticaItem> reporte)
        {
            List<string> lineas = ["Reporte,Resultado"];
            lineas.AddRange(reporte.Select(item =>
                $"{EscaparCsv(item.Estadistica)},{EscaparCsv(item.Resultado)}"));
            return CrearUtf8(string.Join(Environment.NewLine, lineas));
        }

        public static byte[] GenerarTxt(IEnumerable<EstadisticaItem> reporte)
        {
            StringBuilder texto = new();
            texto.AppendLine("REPORTE QUINIEGOL");
            texto.AppendLine("=================");

            int numero = 1;
            foreach (EstadisticaItem item in reporte)
            {
                texto.AppendLine($"{numero}. {item.Estadistica}");
                texto.AppendLine($"   {item.Resultado}");
                texto.AppendLine();
                numero++;
            }
            return CrearUtf8(texto.ToString());
        }

        public static byte[] GenerarPdf(IEnumerable<EstadisticaItem> reporte)
        {
            Document documento = new();
            Section seccion = documento.AddSection();
            Paragraph titulo = seccion.AddParagraph("REPORTE QUINIEGOL");
            titulo.Format.Font.Size = 18;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceAfter = Unit.FromCentimeter(0.8);

            int numero = 1;
            foreach (EstadisticaItem item in reporte)
            {
                Paragraph parrafo = seccion.AddParagraph();
                parrafo.AddFormattedText(
                    $"{numero}. {item.Estadistica}",
                    TextFormat.Bold);
                parrafo.AddLineBreak();
                parrafo.AddText(item.Resultado ?? string.Empty);
                parrafo.Format.SpaceAfter = Unit.FromCentimeter(0.5);
                numero++;
            }

            PdfDocumentRenderer renderer = new() { Document = documento };
            renderer.RenderDocument();
            using MemoryStream archivo = new();
            renderer.PdfDocument.Save(archivo, false);
            return archivo.ToArray();
        }

        private static byte[] CrearUtf8(string contenido)
        {
            return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)
                .GetBytes(contenido);
        }

        private static string EscaparCsv(string? valor)
        {
            string seguro = valor ?? string.Empty;
            string sinEspacios = seguro.TrimStart();
            if (sinEspacios.StartsWith('=') ||
                sinEspacios.StartsWith('+') ||
                sinEspacios.StartsWith('-') ||
                sinEspacios.StartsWith('@'))
            {
                seguro = $"'{seguro}";
            }
            return $"\"{seguro.Replace("\"", "\"\"")}\"";
        }
    }
}

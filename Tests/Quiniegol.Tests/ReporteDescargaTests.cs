using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class ReporteDescargaTests
    {
        [TestMethod]
        public void CsvEscapaComasComillasYPosiblesFormulas()
        {
            List<EstadisticaItem> reporte = new()
            {
                new EstadisticaItem
                {
                    Estadistica = "Dato, especial",
                    Resultado = "Texto \"entre comillas\""
                },
                new EstadisticaItem
                {
                    Estadistica = "Fórmula",
                    Resultado = "=2+2"
                }
            };

            string csv = ReporteDescargaService.GenerarCsv(reporte);

            StringAssert.StartsWith(csv, "Reporte,Resultado");
            StringAssert.Contains(csv, "\"Dato, especial\"");
            StringAssert.Contains(csv, "\"Texto \"\"entre comillas\"\"\"");
            StringAssert.Contains(csv, "\"Fórmula\",\"'=2+2\"");
        }

        [TestMethod]
        public void TxtIncluyeCadaNombreYSuResultado()
        {
            List<EstadisticaItem> reporte = new()
            {
                new EstadisticaItem
                {
                    Estadistica = "Promedio de goles",
                    Resultado = "2.50 goles por partido"
                }
            };

            string txt = ReporteDescargaService.GenerarTxt(reporte);

            StringAssert.Contains(txt, "REPORTE QUINEGOL");
            StringAssert.Contains(txt, "1. Promedio de goles");
            StringAssert.Contains(txt, "2.50 goles por partido");
        }

        [TestMethod]
        public void GuardarCsvCreaArchivoUtf8ConMarca()
        {
            string ruta = Path.Combine(
                Path.GetTempPath(),
                $"reporte-quinegol-{Guid.NewGuid():N}.csv");
            try
            {
                ReporteDescargaService.GuardarCsv(
                    ruta,
                    new[]
                    {
                        new EstadisticaItem
                        {
                            Estadistica = "Equipo más apostado",
                            Resultado = "Costa Rica"
                        }
                    });

                byte[] contenido = File.ReadAllBytes(ruta);
                Assert.IsTrue(contenido.Length > 3);
                Assert.AreEqual(0xEF, contenido[0]);
                Assert.AreEqual(0xBB, contenido[1]);
                Assert.AreEqual(0xBF, contenido[2]);
            }
            finally
            {
                File.Delete(ruta);
            }
        }
    }
}

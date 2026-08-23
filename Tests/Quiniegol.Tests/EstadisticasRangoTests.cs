using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class EstadisticasRangoTests
    {
        [TestMethod]
        public void UsaPronosticosDelPartidoAunqueSeRegistraranAntesDelRango()
        {
            Partido partidoDentroDelRango = new()
            {
                Id = 20,
                FechaHora = new DateTime(2026, 6, 20)
            };

            List<Pronostico> pronosticos = new()
            {
                new()
                {
                    Id = 1,
                    PartidoId = 20,
                    FechaRegistro = new DateTime(2026, 6, 15)
                },
                new()
                {
                    Id = 2,
                    PartidoId = 5,
                    FechaRegistro = new DateTime(2026, 6, 20)
                }
            };

            List<Pronostico> resultado =
                EstadisticasService.FiltrarPronosticosDePartidos(
                    pronosticos,
                    new[] { partidoDentroDelRango }
                );

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(1, resultado.Single().Id);
        }
    }
}

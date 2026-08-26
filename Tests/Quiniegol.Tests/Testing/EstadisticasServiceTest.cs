using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for statistics services.
    /// </summary>
    [TestClass]
    public class EstadisticasServiceTest
    {
        /// <summary>
        /// Statistics should return the teams with most and least goals.
        /// </summary>
        [TestMethod]
        public void StatisticsShouldReturnTeamsWithMostAndLeastGoals()
        {
            // Arrange
            var selections = new List<Seleccion>
            {
                new Seleccion { Id = 1, Nombre = "Equipo A" },
                new Seleccion { Id = 2, Nombre = "Equipo B" },
                new Seleccion { Id = 3, Nombre = "Equipo C" }
            };
            var matches = new List<Partido>
            {
                CreateMatch(1, 2, 3, 0, "Finalizado"),
                CreateMatch(1, 3, 1, 2, "Finalizado")
            };

            // Act
            var mostGoals = EstadisticasGolesService.ObtenerConMasGoles(
                matches,
                selections);
            var leastGoals = EstadisticasGolesService.ObtenerConMenosGoles(
                matches,
                selections);

            // Assert
            Assert.AreEqual("Equipo A (4 goles)", mostGoals);
            Assert.AreEqual("Equipo B (0 goles)", leastGoals);
        }

        /// <summary>
        /// Statistics should ignore pending matches.
        /// </summary>
        [TestMethod]
        public void StatisticsShouldIgnorePendingMatches()
        {
            // Arrange
            var selections = new List<Seleccion>
            {
                new Seleccion { Id = 1, Nombre = "Equipo A" },
                new Seleccion { Id = 2, Nombre = "Equipo B" },
                new Seleccion { Id = 3, Nombre = "Equipo C" }
            };
            var matches = new List<Partido>
            {
                CreateMatch(1, 2, 1, 0, "Finalizado"),
                CreateMatch(3, 1, 10, 10, "Pendiente")
            };

            // Act
            var result = EstadisticasGolesService.ObtenerConMasGoles(
                matches,
                selections);

            // Assert
            Assert.AreEqual("Equipo A (1 gol)", result);
        }

        /// <summary>
        /// Date range should keep predictions from included matches.
        /// </summary>
        [TestMethod]
        public void DateRangeShouldKeepPredictionsFromIncludedMatches()
        {
            // Arrange
            var matches = new List<Partido>
            {
                new Partido { Id = 2 },
                new Partido { Id = 3 }
            };
            var predictions = new List<Pronostico>
            {
                new Pronostico { Id = 1, PartidoId = 1 },
                new Pronostico { Id = 2, PartidoId = 2 },
                new Pronostico { Id = 3, PartidoId = 3 }
            };

            // Act
            var result = EstadisticasService.FiltrarPronosticosDePartidos(
                predictions,
                matches);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Id);
            Assert.AreEqual(3, result[1].Id);
        }

        private static Partido CreateMatch(
            int localId,
            int visitorId,
            int localGoals,
            int visitorGoals,
            string status)
        {
            return new Partido
            {
                SeleccionLocalId = localId,
                SeleccionVisitanteId = visitorId,
                GolesLocal = localGoals,
                GolesVisitante = visitorGoals,
                Estado = status
            };
        }
    }
}

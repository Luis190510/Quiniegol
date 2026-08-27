using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for scorer services.
    /// </summary>
    [TestClass]
    public class GoleadoresServiceTest
    {
        /// <summary>
        /// Scorer names should remove empty and duplicated values.
        /// </summary>
        [TestMethod]
        public void ScorerNamesShouldRemoveEmptyAndDuplicatedValues()
        {
            // Arrange
            var names = new[] { " Ana ", "ana", "", "Brenda" };

            // Act
            var result = GoleadoresPronosticoService.Normalizar(names);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Ana", result[0]);
            Assert.AreEqual("Brenda", result[1]);
        }

        /// <summary>
        /// Prediction without scorers should return a readable message.
        /// </summary>
        [TestMethod]
        public void PredictionWithoutScorersShouldReturnReadableMessage()
        {
            // Arrange
            var prediction = new Pronostico();

            // Act
            var result = GoleadoresPronosticoService.Formatear(
                prediction,
                "Costa Rica",
                "México");

            // Assert
            Assert.AreEqual("Sin goleadores elegidos", result);
        }

        /// <summary>
        /// Pending match should not show real scorers.
        /// </summary>
        [TestMethod]
        public void PendingMatchShouldNotShowRealScorers()
        {
            // Arrange
            var match = new Partido { Id = 1, Estado = "Pendiente" };
            var scorers = new List<GoleadorReal>
            {
                new GoleadorReal { PartidoId = 1, Jugador = "Ana" }
            };

            // Act
            var result = GoleadoresPartidoService.ObtenerVisibles(
                match,
                scorers);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Finished match should show its real scorers.
        /// </summary>
        [TestMethod]
        public void FinishedMatchShouldShowRealScorers()
        {
            // Arrange
            var match = new Partido { Id = 1, Estado = "Finalizado" };
            var scorers = new List<GoleadorReal>
            {
                new GoleadorReal { PartidoId = 1, Jugador = "Ana" },
                new GoleadorReal { PartidoId = 2, Jugador = "Brenda" }
            };

            // Act
            var result = GoleadoresPartidoService.ObtenerVisibles(
                match,
                scorers);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Ana", result[0].Jugador);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="MetricasInsigniasService"/>.
    /// </summary>
    [TestClass]
    public class MetricasInsigniasServiceTest
    {
        /// <summary>
        /// Exact goals should count both sides of the score.
        /// </summary>
        [TestMethod]
        public void ExactGoalsShouldCountBothSides()
        {
            // Arrange
            var predictions = new List<Pronostico>
            {
                new() { UsuarioId = 1, PartidoId = 10,
                    GolesLocalPronosticados = 2, GolesVisitantePronosticados = 1 }
            };
            var matches = new Dictionary<int, Partido>
            {
                [10] = CreateFinishedMatch(10, 2, 1)
            };

            // Act
            var result = MetricasInsigniasService.ContarGolesExactos(
                predictions, matches);

            // Assert
            Assert.AreEqual(2, result[1]);
        }

        /// <summary>
        /// Pending matches should not count exact goals.
        /// </summary>
        [TestMethod]
        public void PendingMatchShouldNotCountExactGoals()
        {
            // Arrange
            var predictions = new List<Pronostico>
            {
                new() { UsuarioId = 1, PartidoId = 10,
                    GolesLocalPronosticados = 0, GolesVisitantePronosticados = 0 }
            };
            var matches = new Dictionary<int, Partido>
            {
                [10] = new Partido { Id = 10, Estado = "Pendiente" }
            };

            // Act
            var result = MetricasInsigniasService.ContarGolesExactos(
                predictions, matches);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Correct scorers should be counted only once.
        /// </summary>
        [TestMethod]
        public void CorrectScorersShouldBeCountedOnlyOnce()
        {
            // Arrange
            var predictions = new List<Pronostico>
            {
                new()
                {
                    UsuarioId = 1,
                    PartidoId = 10,
                    GoleadoresLocalPronosticados = new() { "Ana", "Ana" },
                    GoleadoresVisitantePronosticados = new() { "Luis" }
                }
            };
            var matches = new Dictionary<int, Partido>
            {
                [10] = new Partido
                {
                    Id = 10,
                    SeleccionLocalId = 5,
                    SeleccionVisitanteId = 6,
                    Estado = "Finalizado",
                    GolesLocal = 1,
                    GolesVisitante = 1
                }
            };
            var scorers = new List<GoleadorReal>
            {
                new() { PartidoId = 10, SeleccionId = 5, Jugador = "Ana (penal)" },
                new() { PartidoId = 10, SeleccionId = 6, Jugador = "Luis" }
            };

            // Act
            var result = MetricasInsigniasService.ContarGoleadoresAcertados(
                predictions, matches, scorers);

            // Assert
            Assert.AreEqual(2, result[1]);
        }

        /// <summary>
        /// An unknown match should not count scorers.
        /// </summary>
        [TestMethod]
        public void UnknownMatchShouldNotCountScorers()
        {
            // Arrange
            var predictions = new List<Pronostico>
            {
                new() { UsuarioId = 1, PartidoId = 99 }
            };
            var matches = new Dictionary<int, Partido>();
            var scorers = new List<GoleadorReal>();

            // Act
            var result = MetricasInsigniasService.ContarGoleadoresAcertados(
                predictions, matches, scorers);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        private static Partido CreateFinishedMatch(int id, int local, int visitor)
        {
            return new Partido
            {
                Id = id,
                Estado = "Finalizado",
                GolesLocal = local,
                GolesVisitante = visitor
            };
        }
    }
}

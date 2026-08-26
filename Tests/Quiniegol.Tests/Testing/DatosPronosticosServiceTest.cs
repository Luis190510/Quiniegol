using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="DatosPronosticosService"/>.
    /// </summary>
    [TestClass]
    public class DatosPronosticosServiceTest
    {
        private string _folder = "";
        private JsonRepository<Pronostico> _predictions = null!;
        private JsonRepository<Partido> _matches = null!;
        private JsonRepository<GoleadorReal> _scorers = null!;

        [TestInitialize]
        public void PrepareFiles()
        {
            _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _predictions = new JsonRepository<Pronostico>(
                Path.Combine(_folder, "pronosticos.json"));
            _matches = new JsonRepository<Partido>(
                Path.Combine(_folder, "partidos.json"));
            _scorers = new JsonRepository<GoleadorReal>(
                Path.Combine(_folder, "goleadores.json"));
        }

        [TestCleanup]
        public void RemoveFiles()
        {
            if (Directory.Exists(_folder))
            {
                Directory.Delete(_folder, true);
            }
        }

        /// <summary>
        /// A demonstration user should receive the missing match prediction.
        /// </summary>
        [TestMethod]
        public void MissingPredictionShouldBeAdded()
        {
            // Arrange
            var matches = CreateMatches(13);
            var predictions = matches.Take(12)
                .Select(match => new Pronostico
                {
                    Id = match.Id,
                    UsuarioId = 2,
                    PartidoId = match.Id
                }).ToList();
            _matches.GuardarTodos(matches);
            _predictions.GuardarTodos(predictions);
            var service = CreateService();

            // Act
            var result = service.CompletarCoberturaDelTorneo();

            // Assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(13, _predictions.ObtenerTodos().Count);
        }

        /// <summary>
        /// A regular new user should not receive automatic predictions.
        /// </summary>
        [TestMethod]
        public void NewUserShouldNotReceiveAutomaticPredictions()
        {
            // Arrange
            _matches.GuardarTodos(CreateMatches(13));
            _predictions.GuardarTodos(new List<Pronostico>
            {
                new() { Id = 1, UsuarioId = 8, PartidoId = 1 }
            });
            var service = CreateService();

            // Act
            var result = service.CompletarCoberturaDelTorneo();

            // Assert
            Assert.AreEqual(0, result);
            Assert.AreEqual(1, _predictions.ObtenerTodos().Count);
        }

        /// <summary>
        /// Historical predictions should receive an available scorer.
        /// </summary>
        [TestMethod]
        public void HistoricalPredictionShouldReceiveScorer()
        {
            // Arrange
            _matches.GuardarTodos(new List<Partido>
            {
                new() { Id = 1, SeleccionLocalId = 5, SeleccionVisitanteId = 6 }
            });
            _predictions.GuardarTodos(new List<Pronostico>
            {
                new()
                {
                    Id = 1, PartidoId = 1, UsuarioId = 2,
                    GolesLocalPronosticados = 1,
                    GolesVisitantePronosticados = 0,
                    GoleadoresConfirmados = false
                }
            });
            _scorers.GuardarTodos(new List<GoleadorReal>
            {
                new() { PartidoId = 1, SeleccionId = 5, Jugador = "Ana (penal)" }
            });
            var service = CreateService();

            // Act
            var result = service.CompletarGoleadoresHistoricos();

            // Assert
            var saved = _predictions.ObtenerTodos().Single();
            Assert.AreEqual(1, result);
            Assert.AreEqual("Ana", saved.GoleadoresLocalPronosticados.Single());
            Assert.IsTrue(saved.GoleadoresConfirmados);
        }

        private DatosPronosticosService CreateService()
        {
            return new DatosPronosticosService(_predictions, _matches, _scorers);
        }

        private static List<Partido> CreateMatches(int amount)
        {
            return Enumerable.Range(1, amount)
                .Select(id => new Partido
                {
                    Id = id,
                    FechaHora = new DateTime(2026, 6, 1).AddDays(id)
                }).ToList();
        }
    }
}

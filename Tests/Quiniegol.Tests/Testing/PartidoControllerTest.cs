using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="PartidoController"/>.
    /// </summary>
    [TestClass]
    public class PartidoControllerTest
    {
        private string _folder = "";
        private JsonRepository<Partido> _matches = null!;
        private JsonRepository<ResultadoPartido> _results = null!;
        private PartidoController _controller = null!;

        [TestInitialize]
        public void PrepareController()
        {
            _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _matches = new JsonRepository<Partido>(Path.Combine(_folder, "partidos.json"));
            _results = new JsonRepository<ResultadoPartido>(Path.Combine(_folder, "resultados.json"));
            SesionUsuarioService.IniciarSesion(CreateAdmin());
            FechaSimuladaService.Instancia.CambiarFecha(new DateTime(2026, 6, 15, 12, 0, 0));
            _controller = new PartidoController(
                _matches, _results, FechaSimuladaService.Instancia);
        }

        [TestCleanup]
        public void RemoveFiles()
        {
            SesionUsuarioService.CerrarSesion();
            if (Directory.Exists(_folder))
            {
                Directory.Delete(_folder, true);
            }
        }

        /// <summary>
        /// Match states should follow the simulated date.
        /// </summary>
        [TestMethod]
        public void MatchStatesShouldFollowSimulatedDate()
        {
            // Arrange
            _matches.GuardarTodos(new List<Partido>
            {
                CreateMatch(1, new DateTime(2026, 6, 16, 12, 0, 0)),
                CreateMatch(2, new DateTime(2026, 6, 15, 11, 0, 0)),
                CreateMatch(3, new DateTime(2026, 6, 14, 12, 0, 0)),
                CreateMatch(4, new DateTime(2026, 6, 13, 12, 0, 0))
            });
            _results.GuardarTodos(new List<ResultadoPartido>
            {
                new() { PartidoId = 3, GolesLocal = 2, GolesVisitante = 1 }
            });

            // Act
            var result = _controller.ObtenerPartidos();

            // Assert
            Assert.AreEqual("Pendiente", result.Single(x => x.Id == 1).Estado);
            Assert.AreEqual("En curso", result.Single(x => x.Id == 2).Estado);
            Assert.AreEqual("Finalizado", result.Single(x => x.Id == 3).Estado);
            Assert.AreEqual("Pendiente de resultado", result.Single(x => x.Id == 4).Estado);
            Assert.AreEqual(2, result.Single(x => x.Id == 3).GolesLocal);
        }

        /// <summary>
        /// A valid match should be registered by the administrator.
        /// </summary>
        [TestMethod]
        public void ValidMatchShouldBeRegistered()
        {
            // Act
            _controller.RegistrarPartido(1, 2, new DateTime(2026, 7, 1));

            // Assert
            var saved = _matches.ObtenerTodos().Single();
            Assert.AreEqual(1, saved.Id);
            Assert.AreEqual("Pendiente", saved.Estado);
        }

        /// <summary>
        /// A team should not play against itself.
        /// </summary>
        [TestMethod]
        public void SameTeamMatchShouldBeRejected()
        {
            // Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                _controller.RegistrarPartido(2, 2, new DateTime(2026, 7, 1)));
        }

        /// <summary>
        /// An official result should be added and later updated.
        /// </summary>
        [TestMethod]
        public void OfficialResultShouldBeAddedAndUpdated()
        {
            // Act
            _controller.GuardarResultado(1, 1, 0);
            _controller.GuardarResultado(1, 3, 2);

            // Assert
            var saved = _results.ObtenerTodos().Single();
            Assert.AreEqual(3, saved.GolesLocal);
            Assert.AreEqual(2, saved.GolesVisitante);
        }

        /// <summary>
        /// Deleting a match should also delete its result.
        /// </summary>
        [TestMethod]
        public void DeletingMatchShouldDeleteItsResult()
        {
            // Arrange
            _matches.GuardarTodos(new List<Partido>
            {
                CreateMatch(1, new DateTime(2026, 6, 10))
            });
            _results.GuardarTodos(new List<ResultadoPartido>
            {
                new() { PartidoId = 1, GolesLocal = 1, GolesVisitante = 0 }
            });

            // Act
            _controller.EliminarPartido(1);

            // Assert
            Assert.AreEqual(0, _matches.ObtenerTodos().Count);
            Assert.AreEqual(0, _results.ObtenerTodos().Count);
        }

        /// <summary>
        /// A participant should not change the match calendar.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldNotChangeMatches()
        {
            // Arrange
            SesionUsuarioService.IniciarSesion(CreateParticipant());

            // Act and Assert
            Assert.ThrowsException<UnauthorizedAccessException>(() =>
                _controller.RegistrarPartido(1, 2, new DateTime(2026, 7, 1)));
        }

        private static Partido CreateMatch(int id, DateTime date)
        {
            return new Partido
            {
                Id = id,
                SeleccionLocalId = 1,
                SeleccionVisitanteId = 2,
                FechaHora = date
            };
        }

        private static Usuario CreateAdmin()
        {
            return new Usuario
            {
                Id = 1, Nombre = "Admin", Rol = RolUsuario.Administrador, Activo = true
            };
        }

        private static Usuario CreateParticipant()
        {
            return new Usuario
            {
                Id = 2, Nombre = "Daniel", Rol = RolUsuario.Usuario, Activo = true
            };
        }
    }
}

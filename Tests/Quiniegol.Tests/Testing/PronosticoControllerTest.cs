using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="PronosticoController"/>.
    /// </summary>
    [TestClass]
    public class PronosticoControllerTest
    {
        private string _folder = "";
        private JsonRepository<Pronostico> _predictions = null!;
        private PronosticoController _controller = null!;
        private Usuario _participant = null!;

        [TestInitialize]
        public void PrepareController()
        {
            _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _predictions = new JsonRepository<Pronostico>(Path.Combine(_folder, "pronosticos.json"));
            var users = new JsonRepository<Usuario>(Path.Combine(_folder, "usuarios.json"));
            var matches = new JsonRepository<Partido>(Path.Combine(_folder, "partidos.json"));
            var results = new JsonRepository<ResultadoPartido>(Path.Combine(_folder, "resultados.json"));
            _participant = new Usuario
            {
                Id = 2, Nombre = "Daniel", NombreUsuario = "daniel",
                Correo = "daniel@quinegol.com", ContrasenaHash = "hash",
                Rol = RolUsuario.Usuario, Activo = true
            };
            users.GuardarTodos(new List<Usuario> { CreateAdmin(), _participant });
            matches.GuardarTodos(new List<Partido>
            {
                new()
                {
                    Id = 1, SeleccionLocalId = 1, SeleccionVisitanteId = 2,
                    FechaHora = new DateTime(2026, 6, 20)
                }
            });
            SesionUsuarioService.IniciarSesion(CreateAdmin());
            FechaSimuladaService.Instancia.CambiarFecha(new DateTime(2026, 6, 15));
            SesionUsuarioService.IniciarSesion(_participant);
            var matchController = new PartidoController(
                matches, results, FechaSimuladaService.Instancia);
            _controller = new PronosticoController(
                _predictions,
                new UsuarioController(users),
                matchController,
                FechaSimuladaService.Instancia);
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
        /// A participant should register its own prediction and scorers.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldRegisterOwnPrediction()
        {
            // Act
            _controller.RegistrarPronostico(
                2, 1, 2, 1,
                new[] { " Ana ", "Ana" },
                new[] { "Luis" });

            // Assert
            var saved = _predictions.ObtenerTodos().Single();
            Assert.AreEqual(2, saved.GolesLocalPronosticados);
            Assert.AreEqual(1, saved.GoleadoresLocalPronosticados.Count);
            Assert.IsTrue(saved.GoleadoresConfirmados);
        }

        /// <summary>
        /// A participant should see only its predictions.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldSeeOnlyOwnPredictions()
        {
            // Arrange
            _predictions.GuardarTodos(new List<Pronostico>
            {
                new() { Id = 1, UsuarioId = 2 },
                new() { Id = 2, UsuarioId = 3 }
            });

            // Act
            var result = _controller.ObtenerPronosticos();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].UsuarioId);
        }

        /// <summary>
        /// The administrator should not register predictions.
        /// </summary>
        [TestMethod]
        public void AdministratorShouldNotRegisterPrediction()
        {
            // Arrange
            SesionUsuarioService.IniciarSesion(CreateAdmin());

            // Act and Assert
            Assert.ThrowsException<UnauthorizedAccessException>(() =>
                _controller.RegistrarPronostico(1, 1, 1, 0));
        }

        /// <summary>
        /// A duplicated prediction should be rejected.
        /// </summary>
        [TestMethod]
        public void DuplicatedPredictionShouldBeRejected()
        {
            // Arrange
            _controller.RegistrarPronostico(2, 1, 1, 0);

            // Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                _controller.RegistrarPronostico(2, 1, 2, 0));
        }

        private static Usuario CreateAdmin()
        {
            return new Usuario
            {
                Id = 1, Nombre = "Admin", NombreUsuario = "admin",
                Correo = "admin@quinegol.com", ContrasenaHash = "hash",
                Rol = RolUsuario.Administrador, Activo = true
            };
        }
    }
}

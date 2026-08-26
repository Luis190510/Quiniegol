using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="NotificacionPronosticoController"/>.
    /// </summary>
    [TestClass]
    public class NotificacionControllerTest
    {
        private string _folder = "";
        private JsonRepository<Partido> _matches = null!;
        private JsonRepository<Pronostico> _predictions = null!;
        private JsonRepository<Seleccion> _selections = null!;
        private DateTime _oldDate;

        /// <summary>
        /// Prepare simple data before every test.
        /// </summary>
        [TestInitialize]
        public void PrepareData()
        {
            _folder = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-notifications-{Guid.NewGuid():N}");
            Directory.CreateDirectory(_folder);

            _matches = new JsonRepository<Partido>(
                Path.Combine(_folder, "partidos.json"));
            _predictions = new JsonRepository<Pronostico>(
                Path.Combine(_folder, "pronosticos.json"));
            _selections = new JsonRepository<Seleccion>(
                Path.Combine(_folder, "selecciones.json"));

            _selections.GuardarTodos(new List<Seleccion>
            {
                new Seleccion { Id = 1, Nombre = "Costa Rica" },
                new Seleccion { Id = 2, Nombre = "México" }
            });
            _predictions.GuardarTodos(new List<Pronostico>());

            _oldDate = FechaSimuladaService.Instancia.FechaActual;
            SesionUsuarioService.IniciarSesion(new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Rol = RolUsuario.Administrador,
                Activo = true
            });
        }

        /// <summary>
        /// Remove simple data after every test.
        /// </summary>
        [TestCleanup]
        public void CleanData()
        {
            FechaSimuladaService.Instancia.CambiarFecha(_oldDate);
            SesionUsuarioService.CerrarSesion();
            Directory.Delete(_folder, recursive: true);
        }

        /// <summary>
        /// User should receive matches from the next twenty-four hours.
        /// </summary>
        [TestMethod]
        public void UserShouldReceiveMatchesFromNextTwentyFourHours()
        {
            // Arrange
            var currentDate = new DateTime(2026, 6, 10, 12, 0, 0);
            FechaSimuladaService.Instancia.CambiarFecha(currentDate);
            _matches.GuardarTodos(new List<Partido>
            {
                CreateMatch(1, currentDate.AddHours(2)),
                CreateMatch(2, currentDate.AddHours(25))
            });
            var user = new Usuario
            {
                Id = 2,
                Nombre = "Luis",
                Rol = RolUsuario.Usuario,
                Activo = true
            };
            var controller = CreateController();

            // Act
            var result = controller.ObtenerPendientes(user);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].PartidoId);
        }

        /// <summary>
        /// Administrator should not receive prediction notifications.
        /// </summary>
        [TestMethod]
        public void AdministratorShouldNotReceivePredictionNotifications()
        {
            // Arrange
            var currentDate = new DateTime(2026, 6, 10, 12, 0, 0);
            FechaSimuladaService.Instancia.CambiarFecha(currentDate);
            _matches.GuardarTodos(new List<Partido>
            {
                CreateMatch(1, currentDate.AddHours(2))
            });
            var administrator = new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Rol = RolUsuario.Administrador,
                Activo = true
            };
            var controller = CreateController();

            // Act
            var result = controller.ObtenerPendientes(administrator);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        private NotificacionPronosticoController CreateController()
        {
            return new NotificacionPronosticoController(
                _matches,
                _predictions,
                _selections,
                FechaSimuladaService.Instancia);
        }

        private static Partido CreateMatch(int id, DateTime date)
        {
            return new Partido
            {
                Id = id,
                SeleccionLocalId = 1,
                SeleccionVisitanteId = 2,
                FechaHora = date,
                Estado = "Pendiente"
            };
        }
    }
}

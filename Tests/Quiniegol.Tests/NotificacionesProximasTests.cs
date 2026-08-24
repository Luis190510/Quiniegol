using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class NotificacionesProximasTests
    {
        private string _carpetaTemporal = "";
        private JsonRepository<Partido> _partidos = null!;
        private JsonRepository<Pronostico> _pronosticos = null!;
        private JsonRepository<Seleccion> _selecciones = null!;
        private DateTime _fechaAnterior;

        [TestInitialize]
        public void PrepararDatos()
        {
            _carpetaTemporal = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-notificaciones-{Guid.NewGuid():N}");
            Directory.CreateDirectory(_carpetaTemporal);

            _partidos = new JsonRepository<Partido>(
                Path.Combine(_carpetaTemporal, "partidos.json"));
            _pronosticos = new JsonRepository<Pronostico>(
                Path.Combine(_carpetaTemporal, "pronosticos.json"));
            _selecciones = new JsonRepository<Seleccion>(
                Path.Combine(_carpetaTemporal, "selecciones.json"));

            _selecciones.GuardarTodos(new List<Seleccion>
            {
                new() { Id = 1, Nombre = "Costa Rica" },
                new() { Id = 2, Nombre = "México" }
            });
            _pronosticos.GuardarTodos(new List<Pronostico>());
            _fechaAnterior = FechaSimuladaService.Instancia.FechaActual;
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            FechaSimuladaService.Instancia.CambiarFecha(_fechaAnterior);

            if (Directory.Exists(_carpetaTemporal))
            {
                Directory.Delete(_carpetaTemporal, true);
            }
        }

        [TestMethod]
        public void IncluyeSoloPartidosFuturosQueComienzanEn24HorasOMenos()
        {
            DateTime fechaSimulada = new(2026, 6, 10, 12, 0, 0);
            FechaSimuladaService.Instancia.CambiarFecha(fechaSimulada);
            _partidos.GuardarTodos(new List<Partido>
            {
                CrearPartido(1, fechaSimulada),
                CrearPartido(2, fechaSimulada.AddHours(1)),
                CrearPartido(3, fechaSimulada.AddHours(24)),
                CrearPartido(4, fechaSimulada.AddHours(24).AddSeconds(1))
            });

            List<NotificacionPronosticoItem> resultado =
                CrearController().ObtenerPendientes(CrearUsuario());

            CollectionAssert.AreEqual(
                new[] { 2, 3 },
                resultado.Select(item => item.PartidoId).ToArray());
            Assert.AreEqual("Costa Rica vs México", resultado[0].Partido);
        }

        [TestMethod]
        public void ExcluyeSoloLosPartidosPronosticadosPorElUsuarioActual()
        {
            DateTime fechaSimulada = new(2026, 6, 10, 12, 0, 0);
            FechaSimuladaService.Instancia.CambiarFecha(fechaSimulada);
            _partidos.GuardarTodos(new List<Partido>
            {
                CrearPartido(1, fechaSimulada.AddHours(2)),
                CrearPartido(2, fechaSimulada.AddHours(3))
            });
            _pronosticos.GuardarTodos(new List<Pronostico>
            {
                new() { Id = 1, UsuarioId = 7, PartidoId = 1 },
                new() { Id = 2, UsuarioId = 99, PartidoId = 2 }
            });

            List<NotificacionPronosticoItem> resultado =
                CrearController().ObtenerPendientes(CrearUsuario());

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(2, resultado[0].PartidoId);
        }

        [TestMethod]
        public void UsaLaFechaSimuladaParaCalcularLaVentanaDeNotificacion()
        {
            DateTime fechaPartido = new(2026, 6, 11, 18, 0, 0);
            _partidos.GuardarTodos(new List<Partido>
            {
                CrearPartido(1, fechaPartido)
            });

            FechaSimuladaService.Instancia.CambiarFecha(
                new DateTime(2026, 6, 10, 12, 0, 0));
            Assert.AreEqual(
                0,
                CrearController().ObtenerPendientes(CrearUsuario()).Count);

            FechaSimuladaService.Instancia.CambiarFecha(
                new DateTime(2026, 6, 10, 18, 0, 0));
            Assert.AreEqual(
                1,
                CrearController().ObtenerPendientes(CrearUsuario()).Count);
        }

        [TestMethod]
        public void AdministradorNoRecibeNotificacionesDePronosticos()
        {
            DateTime fechaSimulada = new(2026, 6, 10, 12, 0, 0);
            FechaSimuladaService.Instancia.CambiarFecha(fechaSimulada);
            _partidos.GuardarTodos(new List<Partido>
            {
                CrearPartido(1, fechaSimulada.AddHours(1))
            });
            Usuario administrador = CrearUsuario();
            administrador.Rol = RolUsuario.Administrador;

            List<NotificacionPronosticoItem> resultado =
                CrearController().ObtenerPendientes(administrador);

            Assert.AreEqual(0, resultado.Count);
        }

        private NotificacionPronosticoController CrearController()
        {
            return new NotificacionPronosticoController(
                _partidos,
                _pronosticos,
                _selecciones,
                FechaSimuladaService.Instancia);
        }

        private static Partido CrearPartido(int id, DateTime fechaHora)
        {
            return new Partido
            {
                Id = id,
                SeleccionLocalId = 1,
                SeleccionVisitanteId = 2,
                FechaHora = fechaHora,
                Estado = "Pendiente"
            };
        }

        private static Usuario CrearUsuario()
        {
            return new Usuario
            {
                Id = 7,
                Nombre = "Participante",
                Rol = RolUsuario.Usuario,
                Activo = true
            };
        }
    }
}

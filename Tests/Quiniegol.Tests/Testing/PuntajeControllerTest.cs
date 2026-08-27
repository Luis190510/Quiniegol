using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;
using Quiniegol.Strategies;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="PuntajeController"/>.
    /// </summary>
    [TestClass]
    public class PuntajeControllerTest
    {
        /// <summary>
        /// Finished matches should update predictions and user totals.
        /// </summary>
        [TestMethod]
        public void FinishedMatchesShouldUpdatePoints()
        {
            // Arrange
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                var users = new JsonRepository<Usuario>(Path.Combine(folder, "usuarios.json"));
                var predictions = new JsonRepository<Pronostico>(Path.Combine(folder, "pronosticos.json"));
                var matches = new JsonRepository<Partido>(Path.Combine(folder, "partidos.json"));
                var results = new JsonRepository<ResultadoPartido>(Path.Combine(folder, "resultados.json"));
                users.GuardarTodos(new List<Usuario>
                {
                    new() { Id = 2, Nombre = "Daniel", Rol = RolUsuario.Usuario }
                });
                predictions.GuardarTodos(new List<Pronostico>
                {
                    new()
                    {
                        Id = 1, UsuarioId = 2, PartidoId = 1,
                        GolesLocalPronosticados = 2, GolesVisitantePronosticados = 1
                    }
                });
                matches.GuardarTodos(new List<Partido>
                {
                    new() { Id = 1, FechaHora = new DateTime(2026, 6, 10) }
                });
                results.GuardarTodos(new List<ResultadoPartido>
                {
                    new() { PartidoId = 1, GolesLocal = 2, GolesVisitante = 1 }
                });
                var admin = new Usuario
                {
                    Id = 1, Rol = RolUsuario.Administrador, Activo = true
                };
                SesionUsuarioService.IniciarSesion(admin);
                FechaSimuladaService.Instancia.CambiarFecha(new DateTime(2026, 6, 15));
                var matchController = new PartidoController(
                    matches, results, FechaSimuladaService.Instancia);
                var controller = new PuntajeController(
                    users, predictions, matchController,
                    new CalculadoraPuntajeService());

                // Act
                controller.CalcularTodosLosPuntajes();

                // Assert
                Assert.AreEqual(5, predictions.ObtenerTodos().Single().PuntosObtenidos);
                Assert.AreEqual(5, users.ObtenerTodos().Single().Puntos);
            }
            finally
            {
                SesionUsuarioService.CerrarSesion();
                if (Directory.Exists(folder))
                {
                    Directory.Delete(folder, true);
                }
            }
        }
    }
}

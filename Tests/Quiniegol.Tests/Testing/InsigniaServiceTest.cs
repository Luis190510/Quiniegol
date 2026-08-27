using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;
using Quiniegol.Strategies;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="InsigniaService"/>.
    /// </summary>
    [TestClass]
    public class InsigniaServiceTest
    {
        /// <summary>
        /// Results should assign global and private badges.
        /// </summary>
        [TestMethod]
        public void ResultsShouldAssignGlobalAndPrivateBadges()
        {
            // Arrange
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                var users = new JsonRepository<Usuario>(Path.Combine(folder, "usuarios.json"));
                var predictions = new JsonRepository<Pronostico>(Path.Combine(folder, "pronosticos.json"));
                var pools = new JsonRepository<Quiniela>(Path.Combine(folder, "quinielas.json"));
                var scorers = new JsonRepository<GoleadorReal>(Path.Combine(folder, "goleadores.json"));
                var matches = new JsonRepository<Partido>(Path.Combine(folder, "partidos.json"));
                var results = new JsonRepository<ResultadoPartido>(Path.Combine(folder, "resultados.json"));
                users.GuardarTodos(CreateUsers());
                matches.GuardarTodos(CreateMatches());
                results.GuardarTodos(CreateResults());
                predictions.GuardarTodos(CreatePredictions());
                pools.GuardarTodos(new List<Quiniela>
                {
                    new()
                    {
                        Id = 1, Nombre = "Amigos", CreadorUsuarioId = 2,
                        IntegrantesIds = new() { 2, 3 }
                    }
                });
                scorers.GuardarTodos(Enumerable.Range(1, 10)
                    .Select(id => new GoleadorReal
                    {
                        PartidoId = id, SeleccionId = 1, Jugador = "Ana"
                    }).ToList());
                SesionUsuarioService.IniciarSesion(new Usuario
                {
                    Id = 1, Rol = RolUsuario.Administrador, Activo = true
                });
                FechaSimuladaService.Instancia.CambiarFecha(new DateTime(2026, 7, 1));
                var matchController = new PartidoController(
                    matches, results, FechaSimuladaService.Instancia);
                var pointsController = new PuntajeController(
                    users, predictions, matchController,
                    new CalculadoraPuntajeService());
                var service = new InsigniaService(
                    users, predictions, pools, scorers,
                    matchController, pointsController);

                // Act
                service.RecalcularInsignias();

                // Assert
                var winnerBadges = service.ObtenerInsigniasDeUsuario(2);
                var lastBadges = service.ObtenerInsigniasDeUsuario(3);
                CollectionAssert.Contains(winnerBadges, "Líder global");
                CollectionAssert.Contains(winnerBadges, "Racha de 10 aciertos");
                CollectionAssert.Contains(winnerBadges, "Precisión goleadora");
                CollectionAssert.Contains(winnerBadges, "Cazagoleadores");
                CollectionAssert.Contains(winnerBadges, "Líder de quiniela: Amigos");
                CollectionAssert.Contains(lastBadges, "Peor del ranking global");
                CollectionAssert.Contains(lastBadges, "Peor de quiniela: Amigos");
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

        /// <summary>
        /// An unknown user should return an empty badge list.
        /// </summary>
        [TestMethod]
        public void UnknownUserShouldReturnEmptyBadgeList()
        {
            // Arrange
            var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var users = new JsonRepository<Usuario>(Path.Combine(folder, "usuarios.json"));
            var service = new InsigniaService(
                users,
                new JsonRepository<Pronostico>(Path.Combine(folder, "pronosticos.json")),
                new JsonRepository<Quiniela>(Path.Combine(folder, "quinielas.json")),
                new JsonRepository<GoleadorReal>(Path.Combine(folder, "goleadores.json")),
                CreateEmptyMatchController(folder),
                CreateEmptyPointsController(folder));

            // Act
            var result = service.ObtenerInsigniasDeUsuario(99);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        private static List<Usuario> CreateUsers()
        {
            return new List<Usuario>
            {
                new() { Id = 2, Nombre = "Daniel", Rol = RolUsuario.Usuario,
                    Insignias = new() { "Fundador" } },
                new() { Id = 3, Nombre = "Luis", Rol = RolUsuario.Usuario }
            };
        }

        private static List<Partido> CreateMatches()
        {
            return Enumerable.Range(1, 10).Select(id => new Partido
            {
                Id = id,
                SeleccionLocalId = 1,
                SeleccionVisitanteId = 2,
                FechaHora = new DateTime(2026, 6, 1).AddDays(id)
            }).ToList();
        }

        private static List<ResultadoPartido> CreateResults()
        {
            return Enumerable.Range(1, 10).Select(id => new ResultadoPartido
            {
                PartidoId = id,
                GolesLocal = 1,
                GolesVisitante = 1
            }).ToList();
        }

        private static List<Pronostico> CreatePredictions()
        {
            var predictions = new List<Pronostico>();
            foreach (var id in Enumerable.Range(1, 10))
            {
                predictions.Add(new Pronostico
                {
                    Id = id,
                    UsuarioId = 2,
                    PartidoId = id,
                    GolesLocalPronosticados = 1,
                    GolesVisitantePronosticados = 1,
                    GoleadoresLocalPronosticados = new() { "Ana" }
                });
                predictions.Add(new Pronostico
                {
                    Id = id + 10,
                    UsuarioId = 3,
                    PartidoId = id,
                    GolesLocalPronosticados = 2,
                    GolesVisitantePronosticados = 0
                });
            }
            return predictions;
        }

        private static PartidoController CreateEmptyMatchController(string folder)
        {
            return new PartidoController(
                new JsonRepository<Partido>(Path.Combine(folder, "partidos-vacios.json")),
                new JsonRepository<ResultadoPartido>(Path.Combine(folder, "resultados-vacios.json")),
                FechaSimuladaService.Instancia);
        }

        private static PuntajeController CreateEmptyPointsController(string folder)
        {
            return new PuntajeController(
                new JsonRepository<Usuario>(Path.Combine(folder, "usuarios-vacios.json")),
                new JsonRepository<Pronostico>(Path.Combine(folder, "pronosticos-vacios.json")),
                CreateEmptyMatchController(folder),
                new CalculadoraPuntajeService());
        }
    }
}

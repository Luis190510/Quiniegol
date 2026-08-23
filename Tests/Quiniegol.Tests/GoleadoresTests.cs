using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class GoleadoresTests
    {
        [TestMethod]
        public void PartidoPendienteNoExponeGoleadoresReales()
        {
            Partido partido = new() { Id = 1, Estado = "Pendiente" };
            List<GoleadorReal> goles = new()
            {
                new() { PartidoId = 1, SeleccionId = 1, Jugador = "Jugador", Minuto = "9'" }
            };

            Assert.AreEqual(
                0,
                GoleadoresPartidoService.ObtenerVisibles(partido, goles).Count
            );

            partido.Estado = "Finalizado";

            Assert.AreEqual(
                1,
                GoleadoresPartidoService.ObtenerVisibles(partido, goles).Count
            );
        }

        [TestMethod]
        public void MigracionCompletaHistoricosSinModificarPronosticoNuevo()
        {
            string baseTemporal = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-goleadores-{Guid.NewGuid():N}");
            Directory.CreateDirectory(baseTemporal);

            try
            {
                JsonRepository<Pronostico> pronosticos = new(
                    Path.Combine(baseTemporal, "pronosticos.json"));
                JsonRepository<Partido> partidos = new(
                    Path.Combine(baseTemporal, "partidos.json"));
                JsonRepository<GoleadorReal> goles = new(
                    Path.Combine(baseTemporal, "goles.json"));

                pronosticos.GuardarTodos(new List<Pronostico>
                {
                    new()
                    {
                        Id = 1,
                        PartidoId = 1,
                        GolesLocalPronosticados = 1,
                        GoleadoresConfirmados = false
                    },
                    new()
                    {
                        Id = 2,
                        PartidoId = 1,
                        GolesLocalPronosticados = 1,
                        GoleadoresConfirmados = true
                    }
                });
                partidos.GuardarTodos(new List<Partido>
                {
                    new() { Id = 1, SeleccionLocalId = 10, SeleccionVisitanteId = 20 }
                });
                goles.GuardarTodos(new List<GoleadorReal>
                {
                    new() { PartidoId = 1, SeleccionId = 10, Jugador = "Delantero", Minuto = "10'" }
                });

                DatosPronosticosService migracion = new(
                    pronosticos,
                    partidos,
                    goles);

                Assert.AreEqual(1, migracion.CompletarGoleadoresHistoricos());

                List<Pronostico> resultado = pronosticos.ObtenerTodos();
                Assert.AreEqual(
                    "Delantero",
                    resultado[0].GoleadoresLocalPronosticados.Single());
                Assert.AreEqual(
                    0,
                    resultado[1].GoleadoresLocalPronosticados.Count);
            }
            finally
            {
                Directory.Delete(baseTemporal, true);
            }
        }

        [TestMethod]
        public void CoberturaCompletaPartidosSoloParaUsuariosDeDemostracion()
        {
            string baseTemporal = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-cobertura-{Guid.NewGuid():N}");
            Directory.CreateDirectory(baseTemporal);

            try
            {
                JsonRepository<Pronostico> pronosticos = new(
                    Path.Combine(baseTemporal, "pronosticos.json"));
                JsonRepository<Partido> partidos = new(
                    Path.Combine(baseTemporal, "partidos.json"));
                JsonRepository<GoleadorReal> goles = new(
                    Path.Combine(baseTemporal, "goles.json"));

                pronosticos.GuardarTodos(
                    Enumerable.Range(1, 12)
                        .Select(indice => new Pronostico
                        {
                            Id = indice,
                            UsuarioId = 8,
                            PartidoId = indice,
                            FechaRegistro =
                                new DateTime(2026, 6, 1).AddDays(indice)
                        })
                        .ToList());
                partidos.GuardarTodos(
                    Enumerable.Range(1, 13)
                        .Select(indice => new Partido
                        {
                            Id = indice,
                            FechaHora =
                                new DateTime(2026, 6, 10).AddDays(indice)
                        })
                        .ToList());
                goles.GuardarTodos(new List<GoleadorReal>());

                DatosPronosticosService migracion = new(
                    pronosticos,
                    partidos,
                    goles);

                Assert.AreEqual(1, migracion.CompletarCoberturaDelTorneo());
                Assert.AreEqual(0, migracion.CompletarCoberturaDelTorneo());

                List<Pronostico> resultado = pronosticos.ObtenerTodos();
                Assert.AreEqual(13, resultado.Count);
                Assert.IsTrue(resultado.All(elemento => elemento.UsuarioId == 8));
                Assert.IsTrue(resultado.Single(elemento => elemento.PartidoId == 13)
                    .FechaRegistro < new DateTime(2026, 6, 23));
            }
            finally
            {
                Directory.Delete(baseTemporal, true);
            }
        }
    }
}

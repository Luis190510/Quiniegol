using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class MetricasInsigniasTests
    {
        [TestMethod]
        public void PrecisionGoleadoraCuentaCadaEquipoAcertadoExactamente()
        {
            Dictionary<int, Partido> partidos = new()
            {
                [1] = CrearPartido(1, 2, 1, "Finalizado"),
                [2] = CrearPartido(2, null, null, "Pendiente")
            };
            List<Pronostico> pronosticos = new()
            {
                CrearPronostico(1, 1, 2, 0),
                CrearPronostico(2, 1, 2, 1),
                CrearPronostico(1, 2, 0, 0)
            };

            Dictionary<int, int> resultado =
                MetricasInsigniasService.ContarGolesExactos(
                    pronosticos,
                    partidos);

            Assert.AreEqual(1, resultado[1]);
            Assert.AreEqual(2, resultado[2]);
        }

        [TestMethod]
        public void CazagoleadoresCuentaJugadoresRealesSinDuplicarlos()
        {
            Dictionary<int, Partido> partidos = new()
            {
                [1] = CrearPartido(1, 2, 1, "Finalizado")
            };
            Pronostico pronostico = CrearPronostico(7, 1, 2, 1);
            pronostico.GoleadoresLocalPronosticados =
                new List<string> { "Ana Gol", "Ana Gol", "No anotó" };
            pronostico.GoleadoresVisitantePronosticados =
                new List<string> { "Bea Gol" };
            List<GoleadorReal> goleadores = new()
            {
                new()
                {
                    PartidoId = 1,
                    SeleccionId = 10,
                    Jugador = "Ana Gol (penal)"
                },
                new()
                {
                    PartidoId = 1,
                    SeleccionId = 10,
                    Jugador = "Ana Gol"
                },
                new()
                {
                    PartidoId = 1,
                    SeleccionId = 20,
                    Jugador = "Bea Gol"
                }
            };

            Dictionary<int, int> resultado =
                MetricasInsigniasService.ContarGoleadoresAcertados(
                    new[] { pronostico },
                    partidos,
                    goleadores);

            Assert.AreEqual(2, resultado[7]);
        }

        [TestMethod]
        public void CatalogoIncluyeLasDosNuevasReglasGlobales()
        {
            List<string> nombres = InsigniaService.ObtenerCatalogo()
                .Select(insignia => insignia.Nombre)
                .ToList();

            CollectionAssert.Contains(nombres, "Precisión goleadora");
            CollectionAssert.Contains(nombres, "Cazagoleadores");
        }

        private static Partido CrearPartido(
            int id,
            int? golesLocal,
            int? golesVisitante,
            string estado)
        {
            return new Partido
            {
                Id = id,
                SeleccionLocalId = 10,
                SeleccionVisitanteId = 20,
                GolesLocal = golesLocal,
                GolesVisitante = golesVisitante,
                Estado = estado
            };
        }

        private static Pronostico CrearPronostico(
            int usuarioId,
            int partidoId,
            int golesLocal,
            int golesVisitante)
        {
            return new Pronostico
            {
                UsuarioId = usuarioId,
                PartidoId = partidoId,
                GolesLocalPronosticados = golesLocal,
                GolesVisitantePronosticados = golesVisitante
            };
        }
    }
}

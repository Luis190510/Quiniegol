using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class EstadisticasGolesTests
    {
        [TestMethod]
        public void CalculaEquiposConMasYMenosGolesIncluyendoCero()
        {
            List<Seleccion> selecciones = new()
            {
                new() { Id = 1, Nombre = "Equipo A" },
                new() { Id = 2, Nombre = "Equipo B" },
                new() { Id = 3, Nombre = "Equipo C" }
            };
            List<Partido> partidos = new()
            {
                new()
                {
                    SeleccionLocalId = 1,
                    SeleccionVisitanteId = 2,
                    GolesLocal = 3,
                    GolesVisitante = 0,
                    Estado = "Finalizado"
                },
                new()
                {
                    SeleccionLocalId = 1,
                    SeleccionVisitanteId = 3,
                    GolesLocal = 1,
                    GolesVisitante = 2,
                    Estado = "Finalizado"
                }
            };

            Assert.AreEqual(
                "Equipo A (4 goles)",
                EstadisticasGolesService.ObtenerConMasGoles(
                    partidos,
                    selecciones));
            Assert.AreEqual(
                "Equipo B (0 goles)",
                EstadisticasGolesService.ObtenerConMenosGoles(
                    partidos,
                    selecciones));
        }

        [TestMethod]
        public void IgnoraPartidosPendientes()
        {
            List<Seleccion> selecciones = new()
            {
                new() { Id = 1, Nombre = "Equipo A" },
                new() { Id = 2, Nombre = "Equipo B" }
            };
            List<Partido> partidos = new()
            {
                new()
                {
                    SeleccionLocalId = 1,
                    SeleccionVisitanteId = 2,
                    GolesLocal = null,
                    GolesVisitante = null,
                    Estado = "Pendiente"
                }
            };

            Assert.AreEqual(
                "Sin partidos finalizados",
                EstadisticasGolesService.ObtenerConMasGoles(
                    partidos,
                    selecciones));
        }
    }
}

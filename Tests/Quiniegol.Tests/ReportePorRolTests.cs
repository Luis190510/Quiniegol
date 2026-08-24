using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class ReportePorRolTests
    {
        [TestMethod]
        public void AdministradorRecibeTodosLosReportesDeSuRol()
        {
            DatosReporte datos = CrearDatos();
            List<EstadisticaItem> reporte = ReportePorRolService.CrearReporte(
                datos.Administrador,
                datos.Pronosticos,
                datos.Partidos,
                datos.Usuarios,
                datos.Selecciones);
            HashSet<string> nombres = reporte
                .Select(item => item.Estadistica)
                .ToHashSet();

            Assert.IsTrue(nombres.Contains("Resultado más repetido"));
            Assert.IsTrue(nombres.Contains("Partido con más aciertos"));
            Assert.IsTrue(nombres.Contains("Usuarios con más aciertos (Top 1)"));
            Assert.IsTrue(nombres.Contains("Usuarios con más aciertos (Top 3)"));
            Assert.IsTrue(nombres.Contains("Usuarios con más aciertos (Top 5)"));
            Assert.IsTrue(nombres.Contains("Partido con más pronósticos"));
            Assert.IsTrue(nombres.Contains("Promedio de goles"));
            Assert.IsTrue(nombres.Contains("Partidos sin aciertos"));
            Assert.IsFalse(nombres.Contains("Probabilidad histórica de acierto"));
        }

        [TestMethod]
        public void ParticipanteRecibeSoloReportesPermitidosYSuProbabilidad()
        {
            DatosReporte datos = CrearDatos();
            Usuario ana = datos.Usuarios.Single(usuario => usuario.Nombre == "Ana");
            List<EstadisticaItem> reporte = ReportePorRolService.CrearReporte(
                ana,
                datos.Pronosticos,
                datos.Partidos,
                datos.Usuarios,
                datos.Selecciones);
            HashSet<string> nombres = reporte
                .Select(item => item.Estadistica)
                .ToHashSet();

            Assert.IsTrue(nombres.Contains("Equipo más apostado"));
            Assert.IsTrue(nombres.Contains("Equipo sorpresa (resultado y estadística)"));
            Assert.IsTrue(nombres.Contains("Probabilidad histórica de acierto"));
            Assert.IsFalse(nombres.Contains("Resultado más repetido"));
            Assert.IsFalse(nombres.Contains("Usuarios con más aciertos (Top 5)"));

            string probabilidad = reporte.Single(item =>
                item.Estadistica == "Probabilidad histórica de acierto").Resultado;
            StringAssert.Contains(probabilidad, "66.67");
            StringAssert.Contains(probabilidad, "2 de 3");
        }

        [TestMethod]
        public void EquipoSorpresaIncluyeResultadoYCantidadDePronosticos()
        {
            DatosReporte datos = CrearDatos();
            Usuario ana = datos.Usuarios.Single(usuario => usuario.Nombre == "Ana");
            string sorpresa = ReportePorRolService.CrearReporte(
                    ana,
                    datos.Pronosticos,
                    datos.Partidos,
                    datos.Usuarios,
                    datos.Selecciones)
                .Single(item =>
                    item.Estadistica == "Equipo sorpresa (resultado y estadística)")
                .Resultado;

            StringAssert.Contains(sorpresa, "Equipo A vs Equipo B");
            StringAssert.Contains(sorpresa, "1 - 0");
            StringAssert.Contains(sorpresa, "3 de 3");
            StringAssert.Contains(sorpresa, "100.00");
        }

        [TestMethod]
        public void AdministradorVePartidoSinAciertosYTopDeUsuarios()
        {
            DatosReporte datos = CrearDatos();
            List<EstadisticaItem> reporte = ReportePorRolService.CrearReporte(
                datos.Administrador,
                datos.Pronosticos,
                datos.Partidos,
                datos.Usuarios,
                datos.Selecciones);

            string sinAciertos = reporte.Single(item =>
                item.Estadistica == "Partidos sin aciertos").Resultado;
            string topTres = reporte.Single(item =>
                item.Estadistica == "Usuarios con más aciertos (Top 3)").Resultado;

            StringAssert.Contains(sinAciertos, "Equipo A vs Equipo B");
            StringAssert.Contains(topTres, "Ana");
            StringAssert.Contains(topTres, "Bruno");
            StringAssert.Contains(topTres, "Carla");
        }

        private static DatosReporte CrearDatos()
        {
            Usuario administrador = new()
            {
                Id = 100,
                Nombre = "Administrador",
                Rol = RolUsuario.Administrador,
                Activo = true
            };
            List<Usuario> usuarios = new()
            {
                administrador,
                new Usuario { Id = 1, Nombre = "Ana", Rol = RolUsuario.Usuario },
                new Usuario { Id = 2, Nombre = "Bruno", Rol = RolUsuario.Usuario },
                new Usuario { Id = 3, Nombre = "Carla", Rol = RolUsuario.Usuario }
            };
            List<Seleccion> selecciones = new()
            {
                new Seleccion { Id = 1, Nombre = "Equipo A" },
                new Seleccion { Id = 2, Nombre = "Equipo B" },
                new Seleccion { Id = 3, Nombre = "Equipo C" },
                new Seleccion { Id = 4, Nombre = "Equipo D" }
            };
            List<Partido> partidos = new()
            {
                CrearPartido(1, 1, 2, 1, 0),
                CrearPartido(2, 3, 4, 0, 0),
                CrearPartido(3, 1, 3, 2, 1)
            };
            List<Pronostico> pronosticos = new()
            {
                CrearPronostico(1, 1, 1, 0, 1, 0),
                CrearPronostico(2, 2, 1, 0, 1, 0),
                CrearPronostico(3, 3, 1, 0, 1, 0),
                CrearPronostico(4, 1, 2, 0, 0, 5),
                CrearPronostico(5, 2, 2, 0, 0, 5),
                CrearPronostico(6, 3, 2, 1, 0, 2),
                CrearPronostico(7, 1, 3, 2, 1, 5),
                CrearPronostico(8, 2, 3, 2, 0, 2),
                CrearPronostico(9, 3, 3, 0, 1, 0)
            };

            return new DatosReporte(
                administrador,
                usuarios,
                selecciones,
                partidos,
                pronosticos);
        }

        private static Partido CrearPartido(
            int id,
            int localId,
            int visitanteId,
            int golesLocal,
            int golesVisitante)
        {
            return new Partido
            {
                Id = id,
                SeleccionLocalId = localId,
                SeleccionVisitanteId = visitanteId,
                FechaHora = new DateTime(2026, 6, 10 + id),
                GolesLocal = golesLocal,
                GolesVisitante = golesVisitante,
                Estado = "Finalizado"
            };
        }

        private static Pronostico CrearPronostico(
            int id,
            int usuarioId,
            int partidoId,
            int golesLocal,
            int golesVisitante,
            int puntos)
        {
            return new Pronostico
            {
                Id = id,
                UsuarioId = usuarioId,
                PartidoId = partidoId,
                GolesLocalPronosticados = golesLocal,
                GolesVisitantePronosticados = golesVisitante,
                PuntosObtenidos = puntos
            };
        }

        private sealed record DatosReporte(
            Usuario Administrador,
            List<Usuario> Usuarios,
            List<Seleccion> Selecciones,
            List<Partido> Partidos,
            List<Pronostico> Pronosticos);
    }
}

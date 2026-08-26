using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for report services.
    /// </summary>
    [TestClass]
    public class ReporteServiceTest
    {
        /// <summary>
        /// CSV report should include its header and data.
        /// </summary>
        [TestMethod]
        public void CsvReportShouldIncludeHeaderAndData()
        {
            // Arrange
            var report = new List<EstadisticaItem>
            {
                new EstadisticaItem
                {
                    Estadistica = "Promedio de goles",
                    Resultado = "2.50"
                }
            };

            // Act
            var result = ReporteDescargaService.GenerarCsv(report);

            // Assert
            StringAssert.Contains(result, "Reporte,Resultado");
            StringAssert.Contains(result, "Promedio de goles");
            StringAssert.Contains(result, "2.50");
        }

        /// <summary>
        /// TXT report should include its title and data.
        /// </summary>
        [TestMethod]
        public void TxtReportShouldIncludeTitleAndData()
        {
            // Arrange
            var report = new List<EstadisticaItem>
            {
                new EstadisticaItem
                {
                    Estadistica = "Equipo más apostado",
                    Resultado = "Costa Rica"
                }
            };

            // Act
            var result = ReporteDescargaService.GenerarTxt(report);

            // Assert
            StringAssert.Contains(result, "REPORTE QUINEGOL");
            StringAssert.Contains(result, "Equipo más apostado");
            StringAssert.Contains(result, "Costa Rica");
        }

        /// <summary>
        /// PDF report should create a valid file.
        /// </summary>
        [TestMethod]
        public void PdfReportShouldCreateValidFile()
        {
            // Arrange
            var path = Path.Combine(
                Path.GetTempPath(),
                $"reporte-{Guid.NewGuid():N}.pdf");
            var report = new List<EstadisticaItem>
            {
                new EstadisticaItem
                {
                    Estadistica = "Equipo más apostado",
                    Resultado = "Costa Rica"
                }
            };

            try
            {
                // Act
                ReporteDescargaService.GuardarPdf(path, report);
                var result = File.ReadAllBytes(path);

                // Assert
                Assert.IsTrue(result.Length > 4);
                Assert.AreEqual('%', (char)result[0]);
                Assert.AreEqual('P', (char)result[1]);
                Assert.AreEqual('D', (char)result[2]);
                Assert.AreEqual('F', (char)result[3]);
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Administrator report should include administrative information.
        /// </summary>
        [TestMethod]
        public void AdministratorReportShouldIncludeAdministrativeInformation()
        {
            // Arrange
            var administrator = new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Rol = RolUsuario.Administrador
            };

            // Act
            var result = ReportePorRolService.CrearReporte(
                administrator,
                new List<Pronostico>(),
                new List<Partido>(),
                new List<Usuario> { administrator },
                new List<Seleccion>());

            // Assert
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Resultado más repetido"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Partido con más aciertos"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Usuarios con más aciertos (Top 1)"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Usuarios con más aciertos (Top 3)"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Usuarios con más aciertos (Top 5)"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Partido con más pronósticos"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Promedio de goles"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Partidos sin aciertos"));
            Assert.IsFalse(result.Any(item =>
                item.Estadistica == "Probabilidad histórica de acierto"));
        }

        /// <summary>
        /// Participant report should include personal information.
        /// </summary>
        [TestMethod]
        public void ParticipantReportShouldIncludePersonalInformation()
        {
            // Arrange
            var user = new Usuario
            {
                Id = 2,
                Nombre = "Luis",
                Rol = RolUsuario.Usuario
            };

            // Act
            var result = ReportePorRolService.CrearReporte(
                user,
                new List<Pronostico>(),
                new List<Partido>(),
                new List<Usuario> { user },
                new List<Seleccion>());

            // Assert
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Equipo más apostado"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Equipo sorpresa (resultado y estadística)"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Pronósticos anteriores evaluados"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Aciertos obtenidos"));
            Assert.IsTrue(result.Any(item =>
                item.Estadistica == "Probabilidad histórica de acierto"));
            Assert.IsFalse(result.Any(item =>
                item.Estadistica == "Resultado más repetido"));
        }

        /// <summary>
        /// Participant report should only use forecasts from that participant.
        /// </summary>
        [TestMethod]
        public void ParticipantReportShouldOnlyUseOwnForecasts()
        {
            // Arrange
            var user = new Usuario { Id = 2, Nombre = "Luis", Rol = RolUsuario.Usuario };
            var otherUser = new Usuario { Id = 3, Nombre = "Ana", Rol = RolUsuario.Usuario };
            var match = new Partido
            {
                Id = 1,
                SeleccionLocalId = 10,
                SeleccionVisitanteId = 20
            };
            var forecasts = new List<Pronostico>
            {
                new Pronostico
                {
                    UsuarioId = user.Id,
                    PartidoId = match.Id,
                    GolesLocalPronosticados = 2,
                    GolesVisitantePronosticados = 0
                },
                new Pronostico
                {
                    UsuarioId = otherUser.Id,
                    PartidoId = match.Id,
                    GolesLocalPronosticados = 0,
                    GolesVisitantePronosticados = 2
                },
                new Pronostico
                {
                    UsuarioId = otherUser.Id,
                    PartidoId = match.Id,
                    GolesLocalPronosticados = 0,
                    GolesVisitantePronosticados = 1
                }
            };
            var teams = new List<Seleccion>
            {
                new Seleccion { Id = 10, Nombre = "México" },
                new Seleccion { Id = 20, Nombre = "Canadá" }
            };

            // Act
            var result = ReportePorRolService.CrearReporte(
                user,
                forecasts,
                new List<Partido> { match },
                new List<Usuario> { user, otherUser },
                teams);
            var mostBetTeam = result.First(item =>
                item.Estadistica == "Equipo más apostado");

            // Assert
            StringAssert.Contains(mostBetTeam.Resultado, "México");
            Assert.IsFalse(mostBetTeam.Resultado.Contains("Canadá"));
        }
    }
}

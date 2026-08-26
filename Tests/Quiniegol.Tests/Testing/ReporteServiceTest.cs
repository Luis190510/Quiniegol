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
                item.Estadistica == "Probabilidad histórica de acierto"));
            Assert.IsFalse(result.Any(item =>
                item.Estadistica == "Resultado más repetido"));
        }
    }
}

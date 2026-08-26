using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="FechaSimuladaService"/>.
    /// </summary>
    [TestClass]
    public class FechaSimuladaServiceTest
    {
        /// <summary>
        /// Close session after every test.
        /// </summary>
        [TestCleanup]
        public void CloseSession()
        {
            SesionUsuarioService.CerrarSesion();
        }

        /// <summary>
        /// Regular user should not change the simulated date.
        /// </summary>
        [TestMethod]
        public void RegularUserShouldNotChangeSimulatedDate()
        {
            // Arrange
            var user = new Usuario
            {
                Id = 2,
                Nombre = "Luis",
                Rol = RolUsuario.Usuario,
                Activo = true
            };
            SesionUsuarioService.IniciarSesion(user);

            // Act
            Action result = () => FechaSimuladaService.Instancia.CambiarFecha(
                new DateTime(2026, 6, 10));

            // Assert
            Assert.ThrowsException<UnauthorizedAccessException>(result);
        }

        /// <summary>
        /// Administrator should change the simulated date.
        /// </summary>
        [TestMethod]
        public void AdministratorShouldChangeSimulatedDate()
        {
            // Arrange
            var administrator = new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Rol = RolUsuario.Administrador,
                Activo = true
            };
            var oldDate = FechaSimuladaService.Instancia.FechaActual;
            var newDate = new DateTime(2026, 6, 10);
            SesionUsuarioService.IniciarSesion(administrator);

            try
            {
                // Act
                FechaSimuladaService.Instancia.CambiarFecha(newDate);

                // Assert
                Assert.AreEqual(
                    newDate,
                    FechaSimuladaService.Instancia.FechaActual);
            }
            finally
            {
                FechaSimuladaService.Instancia.CambiarFecha(oldDate);
            }
        }
    }
}

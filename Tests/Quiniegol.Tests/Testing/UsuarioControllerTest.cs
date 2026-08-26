using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="UsuarioController"/>.
    /// </summary>
    [TestClass]
    public class UsuarioControllerTest
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
        /// Regular user should not manage users.
        /// </summary>
        [TestMethod]
        public void RegularUserShouldNotManageUsers()
        {
            // Arrange
            var path = CreateFileWithUsers();

            try
            {
                var controller = new UsuarioController(
                    new JsonRepository<Usuario>(path));
                var user = controller.ObtenerUsuarios().Single(item =>
                    item.Rol == RolUsuario.Usuario);
                SesionUsuarioService.IniciarSesion(user);

                // Act
                Action result = () =>
                    controller.ObtenerUsuariosParaAdministracion();

                // Assert
                Assert.ThrowsException<UnauthorizedAccessException>(result);
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Administrator should deactivate a regular user.
        /// </summary>
        [TestMethod]
        public void AdministratorShouldDeactivateRegularUser()
        {
            // Arrange
            var path = CreateFileWithUsers();

            try
            {
                var controller = new UsuarioController(
                    new JsonRepository<Usuario>(path));
                var administrator = controller.ObtenerUsuarios().Single(item =>
                    item.Rol == RolUsuario.Administrador);
                var user = controller.ObtenerUsuarios().Single(item =>
                    item.Rol == RolUsuario.Usuario);
                SesionUsuarioService.IniciarSesion(administrator);

                // Act
                controller.CambiarEstadoCuenta(user.Id, activar: false);
                var result = controller.ObtenerUsuarios().Single(item =>
                    item.Id == user.Id);

                // Assert
                Assert.IsFalse(result.Activo);
            }
            finally
            {
                File.Delete(path);
            }
        }

        private static string CreateFileWithUsers()
        {
            var path = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-users-{Guid.NewGuid():N}.json");
            var repository = new JsonRepository<Usuario>(path);
            repository.GuardarTodos(new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Nombre = "Administrador",
                    NombreUsuario = "admin",
                    Correo = "admin@example.com",
                    ContrasenaHash = ContrasenaService.CrearHash("Admin123!"),
                    Rol = RolUsuario.Administrador,
                    Activo = true
                },
                new Usuario
                {
                    Id = 2,
                    Nombre = "Luis",
                    NombreUsuario = "luis",
                    Correo = "luis@example.com",
                    ContrasenaHash = ContrasenaService.CrearHash("Clave123!"),
                    Rol = RolUsuario.Usuario,
                    Activo = true
                }
            });
            return path;
        }
    }
}

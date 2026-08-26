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

        /// <summary>
        /// Public registration should create a regular participant.
        /// </summary>
        [TestMethod]
        public void PublicRegistrationShouldCreateParticipant()
        {
            // Arrange
            var path = CreateFileWithUsers();

            try
            {
                var controller = new UsuarioController(
                    new JsonRepository<Usuario>(path));

                // Act
                var result = controller.RegistrarUsuarioPublico(
                    "Daniel Espinoza",
                    "Costa Rica",
                    "daniel.espinoza",
                    "daniel@example.com",
                    "Clave123!");

                // Assert
                Assert.AreEqual(RolUsuario.Usuario, result.Rol);
                Assert.IsTrue(result.Activo);
                Assert.IsFalse(result.DebeCambiarContrasena);
                Assert.AreEqual(3, controller.ObtenerUsuarios().Count);
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// A duplicated username should not be registered.
        /// </summary>
        [TestMethod]
        public void DuplicatedUsernameShouldNotBeRegistered()
        {
            // Arrange
            var path = CreateFileWithUsers();

            try
            {
                var controller = new UsuarioController(
                    new JsonRepository<Usuario>(path));

                // Act and Assert
                Assert.ThrowsException<InvalidOperationException>(() =>
                    controller.RegistrarUsuarioPublico(
                        "Daniel", "Costa Rica", "luis",
                        "daniel@example.com", "Clave123!"));
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Password reset should assign the temporary participant password.
        /// </summary>
        [TestMethod]
        public void PasswordResetShouldAssignTemporaryPassword()
        {
            // Arrange
            var path = CreateFileWithUsers();

            try
            {
                var controller = new UsuarioController(
                    new JsonRepository<Usuario>(path));
                var admin = controller.ObtenerUsuarios().Single(user =>
                    user.Rol == RolUsuario.Administrador);
                SesionUsuarioService.IniciarSesion(admin);

                // Act
                var result = controller.RestablecerContrasena(2);
                var saved = controller.ObtenerUsuarios().Single(user => user.Id == 2);

                // Assert
                Assert.AreEqual(
                    UsuarioController.ContrasenaTemporalUsuario, result);
                Assert.IsTrue(saved.DebeCambiarContrasena);
                Assert.IsTrue(ContrasenaService.Verificar(result, saved.ContrasenaHash));
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Temporary password should be replaced by a new password.
        /// </summary>
        [TestMethod]
        public void TemporaryPasswordShouldBeReplaced()
        {
            // Arrange
            var path = CreateFileWithUsers();

            try
            {
                var controller = new UsuarioController(
                    new JsonRepository<Usuario>(path));
                var admin = controller.ObtenerUsuarios().Single(user =>
                    user.Rol == RolUsuario.Administrador);
                SesionUsuarioService.IniciarSesion(admin);
                var temporaryPassword = controller.RestablecerContrasena(2);

                // Act
                var result = controller.CompletarCambioObligatorio(
                    2, temporaryPassword, "NuevaClave123!");

                // Assert
                Assert.IsFalse(result.DebeCambiarContrasena);
                Assert.IsTrue(ContrasenaService.Verificar(
                    "NuevaClave123!", result.ContrasenaHash));
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Old users should receive missing credentials and an administrator.
        /// </summary>
        [TestMethod]
        public void OldUsersShouldReceiveMissingCredentials()
        {
            // Arrange
            var path = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-old-users-{Guid.NewGuid():N}.json");
            var repository = new JsonRepository<Usuario>(path);
            repository.GuardarTodos(new List<Usuario>
            {
                new()
                {
                    Id = 5,
                    Nombre = "José Pérez",
                    Rol = RolUsuario.Usuario,
                    Activo = true
                }
            });

            try
            {
                // Act
                var controller = new UsuarioController(repository);
                var users = controller.ObtenerUsuarios();
                var migrated = users.Single(user => user.Id == 5);

                // Assert
                Assert.AreEqual("jose.perez", migrated.NombreUsuario);
                Assert.AreEqual("jose.perez@quinegol.local", migrated.Correo);
                Assert.IsTrue(migrated.DebeCambiarContrasena);
                Assert.IsTrue(users.Any(user =>
                    user.Rol == RolUsuario.Administrador));
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

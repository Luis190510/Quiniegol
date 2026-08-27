using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="LoginController"/>.
    /// </summary>
    [TestClass]
    public class LoginControllerTest
    {
        /// <summary>
        /// Login should return the user when credentials are correct.
        /// </summary>
        [TestMethod]
        public void LoginShouldReturnUserWhenCredentialsAreCorrect()
        {
            // Arrange
            var path = CreateFileWithUser(active: true);

            try
            {
                var repository = new JsonRepository<Usuario>(path);
                var login = new LoginController(
                    new UsuarioController(repository));

                // Act
                var result = login.Autenticar("luis", "Clave123!");

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Luis", result.Nombre);
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Login should return null when the account is inactive.
        /// </summary>
        [TestMethod]
        public void LoginShouldReturnNullWhenAccountIsInactive()
        {
            // Arrange
            var path = CreateFileWithUser(active: false);

            try
            {
                var repository = new JsonRepository<Usuario>(path);
                var login = new LoginController(
                    new UsuarioController(repository));

                // Act
                var result = login.Autenticar("luis", "Clave123!");

                // Assert
                Assert.IsNull(result);
            }
            finally
            {
                File.Delete(path);
            }
        }

        private static string CreateFileWithUser(bool active)
        {
            var path = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-login-{Guid.NewGuid():N}.json");
            var repository = new JsonRepository<Usuario>(path);
            repository.GuardarTodos(new List<Usuario>
            {
                new Usuario
                {
                    Id = 2,
                    Nombre = "Luis",
                    NombreUsuario = "luis",
                    Correo = "luis@example.com",
                    ContrasenaHash = ContrasenaService.CrearHash("Clave123!"),
                    Rol = RolUsuario.Usuario,
                    Activo = active
                }
            });
            return path;
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="AccesoQuinielaService"/>.
    /// </summary>
    [TestClass]
    public class AccesoQuinielaServiceTest
    {
        /// <summary>
        /// Member should have access to the private pool.
        /// </summary>
        [TestMethod]
        public void MemberShouldHaveAccessToPrivatePool()
        {
            // Arrange
            var pool = CreatePool();
            var user = CreateUser(2, RolUsuario.Usuario);

            // Act
            var result = AccesoQuinielaService.PuedeConsultar(pool, user);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// User outside the pool should not have access.
        /// </summary>
        [TestMethod]
        public void OutsideUserShouldNotHaveAccessToPrivatePool()
        {
            // Arrange
            var pool = CreatePool();
            var user = CreateUser(9, RolUsuario.Usuario);

            // Act
            var result = AccesoQuinielaService.PuedeConsultar(pool, user);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Administrator should have access to every private pool.
        /// </summary>
        [TestMethod]
        public void AdministratorShouldHaveAccessToEveryPrivatePool()
        {
            // Arrange
            var pool = CreatePool();
            var administrator = CreateUser(100, RolUsuario.Administrador);

            // Act
            var result = AccesoQuinielaService.PuedeConsultar(
                pool,
                administrator);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Creator should be able to manage the private pool.
        /// </summary>
        [TestMethod]
        public void CreatorShouldManagePrivatePool()
        {
            // Arrange
            var pool = CreatePool();
            var creator = CreateUser(1, RolUsuario.Usuario);

            // Act
            var result = AccesoQuinielaService.PuedeAdministrar(pool, creator);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// User outside the pool should be able to join it.
        /// </summary>
        [TestMethod]
        public void OutsideUserShouldBeAbleToJoinPrivatePool()
        {
            // Arrange
            var pool = CreatePool();
            var user = CreateUser(9, RolUsuario.Usuario);

            // Act
            var result = AccesoQuinielaService.PuedeUnirse(pool, user);

            // Assert
            Assert.IsTrue(result);
        }

        private static Quiniela CreatePool()
        {
            return new Quiniela
            {
                Id = 1,
                Nombre = "Amigos",
                CreadorUsuarioId = 1,
                IntegrantesIds = new List<int> { 1, 2 }
            };
        }

        private static Usuario CreateUser(int id, RolUsuario role)
        {
            return new Usuario
            {
                Id = id,
                Nombre = $"Usuario {id}",
                Rol = role,
                Activo = true
            };
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="RankingService"/>.
    /// </summary>
    [TestClass]
    public class RankingServiceTest
    {
        /// <summary>
        /// Equal points should return the same ranking position.
        /// </summary>
        [TestMethod]
        public void EqualPointsShouldReturnSamePosition()
        {
            // Arrange
            var users = new List<Usuario>
            {
                new Usuario { Id = 1, Nombre = "Ana", Puntos = 8 },
                new Usuario { Id = 2, Nombre = "Bruno", Puntos = 8 },
                new Usuario { Id = 3, Nombre = "Carla", Puntos = 4 }
            };

            // Act
            var result = RankingService.Crear(
                users,
                user => user.Insignias);

            // Assert
            Assert.AreEqual(1, result[0].Posicion);
            Assert.AreEqual(1, result[1].Posicion);
            Assert.AreEqual(3, result[2].Posicion);
        }
    }
}

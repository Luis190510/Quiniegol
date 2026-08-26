using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="ContrasenaService"/>.
    /// </summary>
    [TestClass]
    public class ContrasenaServiceTest
    {
        /// <summary>
        /// Password should return true when the password is correct.
        /// </summary>
        [TestMethod]
        public void PasswordShouldReturnTrueWhenItIsCorrect()
        {
            // Arrange
            var password = "ClaveSegura123!";
            var hash = ContrasenaService.CrearHash(password);

            // Act
            var result = ContrasenaService.Verificar(password, hash);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Password should return false when the password is incorrect.
        /// </summary>
        [TestMethod]
        public void PasswordShouldReturnFalseWhenItIsIncorrect()
        {
            // Arrange
            var hash = ContrasenaService.CrearHash("ClaveSegura123!");

            // Act
            var result = ContrasenaService.Verificar("OtraClave!", hash);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Password hash should be different from the original password.
        /// </summary>
        [TestMethod]
        public void HashShouldBeDifferentFromOriginalPassword()
        {
            // Arrange
            var password = "ClaveSegura123!";

            // Act
            var result = ContrasenaService.CrearHash(password);

            // Assert
            Assert.AreNotEqual(password, result);
        }
    }
}

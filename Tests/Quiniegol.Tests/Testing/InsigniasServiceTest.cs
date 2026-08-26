using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for badge services.
    /// </summary>
    [TestClass]
    public class InsigniasServiceTest
    {
        /// <summary>
        /// Global ranking should not show private badges.
        /// </summary>
        [TestMethod]
        public void GlobalRankingShouldNotShowPrivateBadges()
        {
            // Arrange
            var badges = new List<string>
            {
                "Líder global",
                "Líder de quiniela: Amigos"
            };

            // Act
            var result = VisibilidadInsigniasService
                .ObtenerGlobales(badges)
                .ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Líder global", result[0]);
        }

        /// <summary>
        /// Private ranking should show badges from its pool only.
        /// </summary>
        [TestMethod]
        public void PrivateRankingShouldShowItsPoolBadgesOnly()
        {
            // Arrange
            var badges = new List<string>
            {
                "Líder de quiniela: Amigos",
                "Líder de quiniela: Oficina"
            };

            // Act
            var result = VisibilidadInsigniasService
                .ObtenerDeQuiniela(badges, "Amigos")
                .ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Líder de quiniela: Amigos", result[0]);
        }

        /// <summary>
        /// Badge catalog should include goal precision and scorer badges.
        /// </summary>
        [TestMethod]
        public void BadgeCatalogShouldIncludeGoalBadges()
        {
            // Arrange
            var precisionBadge = "Precisión goleadora";
            var scorerBadge = "Cazagoleadores";

            // Act
            var result = InsigniaService.ObtenerCatalogo()
                .Select(badge => badge.Nombre)
                .ToList();

            // Assert
            CollectionAssert.Contains(result, precisionBadge);
            CollectionAssert.Contains(result, scorerBadge);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class VisibilidadInsigniasTests
    {
        private static readonly List<string> Insignias = new()
        {
            "Líder global",
            "Rey de los empates",
            "Precisión goleadora",
            "Cazagoleadores",
            "Líder de quiniela: Amigos",
            "Peor de quiniela: Oficina",
            "Precisión goleadora de quiniela: Amigos",
            "Cazagoleadores de quiniela: Oficina"
        };

        [TestMethod]
        public void RankingGlobalNoExponeInsigniasPrivadas()
        {
            List<string> visibles = VisibilidadInsigniasService
                .ObtenerGlobales(Insignias)
                .ToList();

            CollectionAssert.AreEquivalent(
                new[]
                {
                    "Líder global",
                    "Rey de los empates",
                    "Precisión goleadora",
                    "Cazagoleadores"
                },
                visibles
            );
        }

        [TestMethod]
        public void RankingPrivadoSoloMuestraInsigniasDeSuQuiniela()
        {
            List<string> visibles = VisibilidadInsigniasService
                .ObtenerDeQuiniela(Insignias, "Amigos")
                .ToList();

            CollectionAssert.AreEqual(
                new[]
                {
                    "Líder de quiniela: Amigos",
                    "Precisión goleadora de quiniela: Amigos"
                },
                visibles
            );
        }

        [TestMethod]
        public void DashboardPuedeSepararInsigniasGlobalesYPrivadas()
        {
            List<string> globales = VisibilidadInsigniasService
                .ObtenerGlobales(Insignias)
                .ToList();
            List<string> privadas = VisibilidadInsigniasService
                .ObtenerPrivadas(Insignias)
                .ToList();

            Assert.IsFalse(globales.Any(insignia =>
                insignia.Contains("de quiniela:")));
            Assert.IsTrue(privadas.All(insignia =>
                insignia.Contains("de quiniela:")));
            Assert.AreEqual(4, globales.Count);
            Assert.AreEqual(4, privadas.Count);
        }
    }
}

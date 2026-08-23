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
            "Líder de quiniela: Amigos",
            "Peor de quiniela: Oficina"
        };

        [TestMethod]
        public void RankingGlobalNoExponeInsigniasPrivadas()
        {
            List<string> visibles = VisibilidadInsigniasService
                .ObtenerGlobales(Insignias)
                .ToList();

            CollectionAssert.AreEquivalent(
                new[] { "Líder global", "Rey de los empates" },
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
                new[] { "Líder de quiniela: Amigos" },
                visibles
            );
        }
    }
}

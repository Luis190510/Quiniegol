using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class RankingServiceTests
    {
        [TestMethod]
        public void PuntajesIgualesCompartenPosicion()
        {
            List<Usuario> usuarios = new()
            {
                new() { Id = 1, Nombre = "Ana", Puntos = 8 },
                new() { Id = 2, Nombre = "Bruno", Puntos = 8 },
                new() { Id = 3, Nombre = "Carla", Puntos = 4 }
            };

            List<RankingItem> ranking = RankingService.Crear(
                usuarios,
                usuario => usuario.Insignias
            );

            CollectionAssert.AreEqual(
                new[] { 1, 1, 3 },
                ranking.Select(fila => fila.Posicion).ToArray()
            );
        }
    }
}

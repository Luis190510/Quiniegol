using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Strategies;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="CalculadoraPuntajeService"/>.
    /// </summary>
    [TestClass]
    public class CalculadoraPuntajeServiceTest
    {
        /// <summary>
        /// Exact score should return five points.
        /// </summary>
        [TestMethod]
        public void ExactScoreShouldReturnFivePoints()
        {
            // Arrange
            var prediction = CreatePrediction(2, 1);
            var match = CreateMatch(2, 1);
            var calculator = new CalculadoraPuntajeService();

            // Act
            var result = calculator.Calcular(prediction, match);

            // Assert
            Assert.AreEqual(5, result);
        }

        /// <summary>
        /// Correct winner should return two points.
        /// </summary>
        [TestMethod]
        public void CorrectWinnerShouldReturnTwoPoints()
        {
            // Arrange
            var prediction = CreatePrediction(3, 0);
            var match = CreateMatch(2, 1);
            var calculator = new CalculadoraPuntajeService();

            // Act
            var result = calculator.Calcular(prediction, match);

            // Assert
            Assert.AreEqual(2, result);
        }

        /// <summary>
        /// Incorrect result should return zero points.
        /// </summary>
        [TestMethod]
        public void IncorrectResultShouldReturnZeroPoints()
        {
            // Arrange
            var prediction = CreatePrediction(0, 2);
            var match = CreateMatch(2, 1);
            var calculator = new CalculadoraPuntajeService();

            // Act
            var result = calculator.Calcular(prediction, match);

            // Assert
            Assert.AreEqual(0, result);
        }

        private static Pronostico CreatePrediction(int local, int visitor)
        {
            return new Pronostico
            {
                GolesLocalPronosticados = local,
                GolesVisitantePronosticados = visitor
            };
        }

        private static Partido CreateMatch(int local, int visitor)
        {
            return new Partido
            {
                GolesLocal = local,
                GolesVisitante = visitor,
                Estado = "Finalizado"
            };
        }
    }
}

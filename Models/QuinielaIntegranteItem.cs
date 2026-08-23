namespace Quiniegol.Models
{
    /// <summary>Resumen visible de un integrante de una quiniela autorizada.</summary>
    public class QuinielaIntegranteItem
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string PaisPreferido { get; set; } = "";

        public int Puntos { get; set; }

        public int PronosticosConGoleadores { get; set; }
    }
}

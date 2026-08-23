namespace Quiniegol.Models
{
    /// <summary>
    /// Gol oficial de un partido del Mundial 2026 obtenido de FIFA.
    /// </summary>
    public class GoleadorReal
    {
        public int PartidoId { get; set; }

        public int SeleccionId { get; set; }

        public string Jugador { get; set; } = "";

        public string Minuto { get; set; } = "";
    }
}

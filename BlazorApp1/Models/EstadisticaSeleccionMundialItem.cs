namespace Quiniegol.Models
{
    /// <summary>Resumen calculado de una selección en el Mundial 2026.</summary>
    public sealed class EstadisticaSeleccionMundialItem
    {
        public int SeleccionId { get; set; }
        public string Seleccion { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public int PartidosJugados { get; set; }
        public int Victorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolesFavor { get; set; }
        public int GolesContra { get; set; }
        public int DiferenciaGoles => GolesFavor - GolesContra;
        public int Puntos => Victorias * 3 + Empates;
    }
}

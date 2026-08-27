namespace Quiniegol.Models
{
    /// <summary>Datos de un partido listos para mostrarse en Blazor.</summary>
    public sealed class PartidoMundialItem
    {
        public int Id { get; set; }
        public int SeleccionLocalId { get; set; }
        public int SeleccionVisitanteId { get; set; }
        public string EquipoLocal { get; set; } = string.Empty;
        public string EquipoVisitante { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Fase { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool Finalizado { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }
    }
}

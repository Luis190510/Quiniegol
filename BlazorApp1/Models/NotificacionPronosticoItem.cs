namespace Quiniegol.Models
{
    /// <summary>
    /// Describe un partido próximo que el usuario todavía debe pronosticar.
    /// </summary>
    public class NotificacionPronosticoItem
    {
        /// <summary>Obtiene o establece el identificador del partido.</summary>
        public int PartidoId { get; set; }

        /// <summary>Obtiene o establece la fecha de inicio del partido.</summary>
        public DateTime FechaHora { get; set; }

        /// <summary>Obtiene o establece el nombre legible del encuentro.</summary>
        public string Partido { get; set; } = "";
    }
}

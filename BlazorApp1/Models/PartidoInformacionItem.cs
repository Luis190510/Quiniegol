using System;

namespace Quiniegol.Models
{
    public class PartidoInformacionItem
    {
        public int PartidoId { get; set; }

        public DateTime FechaHora { get; set; }

        public string Partido { get; set; } = "";

        public string Estado { get; set; } = "";

        public string Marcador { get; set; } = "";
    }
}

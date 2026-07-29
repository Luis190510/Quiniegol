using System;
using System.Collections.Generic;

namespace Quiniegol.Models
{
    public class PartidoDetalleItem
    {
        public int PartidoId { get; set; }

        public int SeleccionLocalId { get; set; }

        public int SeleccionVisitanteId { get; set; }

        public string Local { get; set; } = "";

        public string Visitante { get; set; } = "";

        public string RutaBanderaLocal { get; set; } = "";

        public string RutaBanderaVisitante { get; set; } = "";

        public DateTime FechaHora { get; set; }

        public string Estado { get; set; } = "";

        public string Marcador { get; set; } = "";

        public List<AnotadorVistaItem> Anotadores { get; set; } =
            new List<AnotadorVistaItem>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Partido
    {
        public int Id { get; set; }

        public int SeleccionLocalId { get; set; }
        public int SeleccionVisitanteId { get; set; }

        public DateTime FechaHora { get; set; }

        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }

        public string Estado { get; set; } = "Pendiente";

        public List<Anotador> Anotadores { get; set; } = new();
    }
}
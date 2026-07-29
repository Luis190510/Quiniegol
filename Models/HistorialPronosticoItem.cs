using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class HistorialPronosticoItem
    {
        public int PronosticoId { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string Partido { get; set; } = "";

        public string MarcadorPronosticado { get; set; } = "";

        public string ResultadoReal { get; set; } = "";

        public string Estado { get; set; } = "";

        public string Puntos { get; set; } = "";
    }
}
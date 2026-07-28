using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Pronostico
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PartidoId { get; set; }

        public int GolesLocalPronosticados { get; set; }
        public int GolesVisitantePronosticados { get; set; }

        public int PuntosObtenidos { get; set; }
    }
}
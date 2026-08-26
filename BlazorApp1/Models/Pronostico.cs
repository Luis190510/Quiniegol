using System;
using System.Collections.Generic;
namespace Quiniegol.Models
{
    /// <summary>Representa el marcador y los goleadores elegidos por un usuario.</summary>
    public class Pronostico
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int PartidoId { get; set; }

        public int GolesLocalPronosticados { get; set; }

        public int GolesVisitantePronosticados { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int? PuntosObtenidos { get; set; }

        /// <summary>Posibles goleadores elegidos para la selección local.</summary>
        public List<string> GoleadoresLocalPronosticados { get; set; } = new();

        /// <summary>Posibles goleadores elegidos para la selección visitante.</summary>
        public List<string> GoleadoresVisitantePronosticados { get; set; } = new();

        /// <summary>
        /// Distingue los pronósticos históricos pendientes de migración de los
        /// pronósticos nuevos donde el usuario decidió sus goleadores.
        /// </summary>
        public bool GoleadoresConfirmados { get; set; }
    }
}

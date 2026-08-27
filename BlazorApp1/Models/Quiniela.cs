using System;
using System.Text;
using System.Collections.Generic;

namespace Quiniegol.Models
{
    public class Quiniela
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public string Tipo { get; set; } = "Privada";

        /// <summary>Identifica a quien puede administrar la quiniela.</summary>
        public int CreadorUsuarioId { get; set; }

        public List<int> IntegrantesIds { get; set; } =
            new List<int>();
    }
}

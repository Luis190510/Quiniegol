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

        public List<int> IntegrantesIds { get; set; } =
            new List<int>();
    }
}

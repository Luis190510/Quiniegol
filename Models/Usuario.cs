using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string PaisPreferido { get; set; } = "";

        public int Puntos { get; set; }

        public List<string> Insignias { get; set; } = new();
    }
}

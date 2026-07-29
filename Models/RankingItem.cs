using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class RankingItem
    {
        public int Posicion { get; set; }

        public int UsuarioId { get; set; }

        public string Nombre { get; set; } = "";

        public string PaisPreferido { get; set; } = "";

        public int Puntos { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Anotador
    {
        public string NombreJugador { get; set; } = string.Empty;
        public string Seleccion { get; set; } = string.Empty;
        public int Minuto { get; set; }
    }
}
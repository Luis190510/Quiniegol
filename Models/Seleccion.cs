using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Seleccion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string RutaBandera { get; set; } = string.Empty;
    }
}
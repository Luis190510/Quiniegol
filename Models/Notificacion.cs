using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Notificacion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
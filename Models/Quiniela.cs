using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Models
{
    public class Quiniela
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EsPrivada { get; set; }

        public List<int> IntegrantesIds { get; set; } = new();
        public List<Notificacion> Timeline { get; set; } = new();
    }
}
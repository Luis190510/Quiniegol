using System;
using System.Collections.Generic;
using System.Text;

namespace Quiniegol.Services
{
    public sealed class FechaSimuladaService
    {
        private static readonly FechaSimuladaService _instancia = new();

        public static FechaSimuladaService Instancia
        {
            get
            {
                return _instancia;
            }
        }

        public DateTime FechaActual { get; private set; }

        private FechaSimuladaService()
        {
            FechaActual = DateTime.Now;
        }

        public void CambiarFecha(DateTime nuevaFecha)
        {
            FechaActual = nuevaFecha;
        }
    }
}
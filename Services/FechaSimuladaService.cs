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

        /// <summary>
        /// Cambia la fecha usada para probar pronósticos y resultados sin
        /// modificar la fecha real del equipo. Solo una sesión administrativa
        /// puede realizar este cambio.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">
        /// Se produce cuando la sesión actual no pertenece a un administrador.
        /// </exception>
        public void CambiarFecha(DateTime nuevaFecha)
        {
            SesionUsuarioService.ExigirAdministrador();
            FechaActual = nuevaFecha;
        }
    }
}

namespace Quiniegol.Services
{
    /// <summary>
    /// Mantiene la fecha oficial simulada compartida por toda la aplicación web.
    /// </summary>
    public sealed class FechaSimuladaService
    {
        private readonly object _bloqueo = new();
        private DateTime _fechaActual = DateTime.Now;

        /// <summary>Obtiene la fecha simulada de manera segura.</summary>
        public DateTime FechaActual
        {
            get
            {
                lock (_bloqueo)
                {
                    return _fechaActual;
                }
            }
        }

        /// <summary>
        /// Cambia la fecha global cuando la sesión pertenece a un administrador.
        /// </summary>
        public void CambiarFecha(
            DateTime nuevaFecha,
            SesionUsuarioService sesion)
        {
            ArgumentNullException.ThrowIfNull(sesion);
            sesion.ExigirAdministrador();

            lock (_bloqueo)
            {
                _fechaActual = nuevaFecha;
            }
        }
    }
}

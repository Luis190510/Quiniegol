using Quiniegol.Models;

namespace Quiniegol.Services
{
    /// <summary>
    /// Mantiene la identidad autenticada mientras la aplicación está abierta.
    /// </summary>
    public static class SesionUsuarioService
    {
        private static Usuario? _usuarioActual;

        /// <summary>Obtiene el usuario autenticado.</summary>
        public static Usuario UsuarioActual =>
            _usuarioActual ?? throw new InvalidOperationException(
                "Debe iniciar sesión para realizar esta operación."
            );

        /// <summary>Indica si existe una sesión autenticada.</summary>
        public static bool EstaAutenticado => _usuarioActual != null;

        /// <summary>Indica si la sesión corresponde a un administrador.</summary>
        public static bool EsAdministrador =>
            _usuarioActual?.Rol == RolUsuario.Administrador;

        /// <summary>
        /// Inicia la sesión con una cuenta activa.
        /// </summary>
        /// <param name="usuario">Cuenta autenticada.</param>
        public static void IniciarSesion(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (!usuario.Activo)
            {
                throw new InvalidOperationException(
                    "La cuenta seleccionada está desactivada."
                );
            }

            _usuarioActual = usuario;
        }

        /// <summary>Elimina los datos de la sesión actual.</summary>
        public static void CerrarSesion()
        {
            _usuarioActual = null;
        }

        /// <summary>Exige que la operación sea realizada por un administrador.</summary>
        /// <exception cref="UnauthorizedAccessException">
        /// Se produce cuando no existe una sesión administrativa.
        /// </exception>
        public static void ExigirAdministrador()
        {
            if (!EsAdministrador)
            {
                throw new UnauthorizedAccessException(
                    "Esta operación requiere permisos de administrador."
                );
            }
        }
    }
}

using Quiniegol.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Quiniegol.Services
{
    /// <summary>
    /// Mantiene la identidad autenticada durante un circuito de Blazor.
    /// Cada conexión recibe una instancia independiente mediante inyección.
    /// </summary>
    public sealed class SesionUsuarioService : AuthenticationStateProvider
    {
        private Usuario? _usuarioActual;
        private Usuario? _usuarioPendienteCambio;

        /// <summary>Obtiene el usuario autenticado.</summary>
        public Usuario UsuarioActual =>
            _usuarioActual ?? throw new InvalidOperationException(
                "Debe iniciar sesión para realizar esta operación.");

        /// <summary>Indica si existe una sesión autenticada.</summary>
        public bool EstaAutenticado => _usuarioActual is not null;

        /// <summary>Indica si la cuenta autenticada es administradora.</summary>
        public bool EsAdministrador =>
            _usuarioActual?.Rol == RolUsuario.Administrador;

        /// <summary>Cuenta que debe sustituir una contraseña temporal.</summary>
        public Usuario? UsuarioPendienteCambio => _usuarioPendienteCambio;

        /// <summary>Inicia la sesión con una cuenta activa.</summary>
        public void IniciarSesion(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (!usuario.Activo)
            {
                throw new InvalidOperationException(
                    "La cuenta seleccionada está desactivada.");
            }

            _usuarioActual = usuario;
            _usuarioPendienteCambio = null;
            NotificarCambioDeAutenticacion();
        }

        /// <summary>
        /// Conserva una cuenta validada que todavía no puede iniciar sesión.
        /// </summary>
        public void IniciarCambioObligatorio(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (!usuario.Activo || !usuario.DebeCambiarContrasena)
            {
                throw new InvalidOperationException(
                    "La cuenta no tiene un cambio obligatorio pendiente.");
            }

            _usuarioActual = null;
            _usuarioPendienteCambio = usuario;
            NotificarCambioDeAutenticacion();
        }

        /// <summary>Elimina los datos de la sesión actual.</summary>
        public void CerrarSesion()
        {
            _usuarioActual = null;
            _usuarioPendienteCambio = null;
            NotificarCambioDeAutenticacion();
        }

        /// <summary>Exige una sesión administrativa.</summary>
        public void ExigirAdministrador()
        {
            if (!EsAdministrador)
            {
                throw new UnauthorizedAccessException(
                    "Esta operación requiere permisos de administrador.");
            }
        }

        /// <summary>Construye la identidad utilizada por la autorización de Blazor.</summary>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identidad = _usuarioActual is null
                ? new ClaimsIdentity()
                : new ClaimsIdentity(
                    new[]
                    {
                        new Claim(
                            ClaimTypes.NameIdentifier,
                            _usuarioActual.Id.ToString()),
                        new Claim(ClaimTypes.Name, _usuarioActual.NombreUsuario),
                        new Claim(ClaimTypes.Email, _usuarioActual.Correo),
                        new Claim(ClaimTypes.Role, _usuarioActual.Rol.ToString())
                    },
                    "Quiniegol");

            return Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(identidad)));
        }

        private void NotificarCambioDeAutenticacion()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Valida las credenciales y registra nuevas cuentas.
    /// </summary>
    public class LoginController
    {
        private readonly UsuarioController _usuarioController;

        /// <summary>Inicializa el controlador con el archivo del proyecto.</summary>
        public LoginController()
            : this(new UsuarioController())
        {
        }

        /// <summary>
        /// Inicializa el controlador con una fuente de usuarios específica.
        /// </summary>
        /// <param name="usuarioController">Controlador que administra las cuentas.</param>
        public LoginController(UsuarioController usuarioController)
        {
            _usuarioController = usuarioController ??
                throw new ArgumentNullException(nameof(usuarioController));
        }

        /// <summary>
        /// Obtiene la cuenta activa asociada con las credenciales indicadas.
        /// </summary>
        /// <param name="identificador">Nombre de usuario o correo.</param>
        /// <param name="contrasena">Contraseña proporcionada.</param>
        /// <returns>La cuenta autenticada o <see langword="null"/>.</returns>
        public Usuario? Autenticar(
            string identificador,
            string contrasena)
        {
            if (string.IsNullOrWhiteSpace(identificador) ||
                string.IsNullOrEmpty(contrasena))
            {
                return null;
            }

            Usuario? usuario = _usuarioController
                .ObtenerUsuarios()
                .FirstOrDefault(elemento =>
                    elemento.Activo &&
                    (elemento.NombreUsuario.Equals(
                        identificador.Trim(),
                        StringComparison.OrdinalIgnoreCase) ||
                     elemento.Correo.Equals(
                        identificador.Trim(),
                        StringComparison.OrdinalIgnoreCase)));

            return usuario != null &&
                ContrasenaService.Verificar(
                    contrasena,
                    usuario.ContrasenaHash)
                ? usuario
                : null;
        }

        /// <summary>Indica si las credenciales son válidas.</summary>
        public bool Login(
            string identificador,
            string contrasena)
        {
            return Autenticar(identificador, contrasena) != null;
        }

        /// <summary>Registra una cuenta participante con contraseña elegida.</summary>
        public Usuario RegistrarCuenta(
            string nombre,
            string paisPreferido,
            string nombreUsuario,
            string correo,
            string contrasena)
        {
            return _usuarioController.RegistrarUsuarioPublico(
                nombre,
                paisPreferido,
                nombreUsuario,
                correo,
                contrasena
            );
        }

        /// <summary>
        /// Sustituye la contraseña temporal antes de iniciar la sesión definitiva.
        /// </summary>
        public Usuario CompletarCambioObligatorio(
            int usuarioId,
            string contrasenaTemporal,
            string nuevaContrasena)
        {
            return _usuarioController.CompletarCambioObligatorio(
                usuarioId,
                contrasenaTemporal,
                nuevaContrasena);
        }
    }
}

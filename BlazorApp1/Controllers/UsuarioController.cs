using System.Globalization;
using System.Net.Mail;
using System.Text;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Coordina el registro, consulta y migración de cuentas de usuario.
    /// </summary>
    public class UsuarioController
    {
        /// <summary>Contraseña inicial para la cuenta administrativa migrada.</summary>
        public const string ContrasenaTemporalAdministrador = "Admin123!";

        /// <summary>Contraseña inicial para usuarios migrados.</summary>
        public const string ContrasenaTemporalUsuario = "Quiniegol123!";

        private readonly JsonRepository<Usuario> _usuarioRepository;
        private readonly SesionUsuarioService _sesion;

        /// <summary>
        /// Inicializa el controlador con un repositorio específico.
        /// </summary>
        /// <param name="usuarioRepository">Repositorio utilizado para persistir cuentas.</param>
        public UsuarioController(
            JsonRepository<Usuario> usuarioRepository,
            SesionUsuarioService sesion)
        {
            _usuarioRepository = usuarioRepository ??
                throw new ArgumentNullException(nameof(usuarioRepository));
            _sesion = sesion ??
                throw new ArgumentNullException(nameof(sesion));

            AsegurarCredencialesIniciales();
        }

        /// <summary>Obtiene todas las cuentas registradas.</summary>
        /// <returns>Lista de cuentas persistidas.</returns>
        public List<Usuario> ObtenerUsuarios()
        {
            return _usuarioRepository.ObtenerTodos();
        }

        /// <summary>
        /// Obtiene las cuentas para la pantalla administrativa.
        /// </summary>
        public List<Usuario> ObtenerUsuariosParaAdministracion()
        {
            _sesion.ExigirAdministrador();
            return _usuarioRepository.ObtenerTodos();
        }

        /// <summary>
        /// Asigna una contraseña temporal y obliga a reemplazarla en el siguiente acceso.
        /// </summary>
        /// <returns>Contraseña temporal que debe entregarse al titular de la cuenta.</returns>
        public string RestablecerContrasena(int usuarioId)
        {
            _sesion.ExigirAdministrador();
            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();
            Usuario usuario = ObtenerUsuarioExistente(usuarios, usuarioId);
            string contrasenaTemporal = usuario.Rol == RolUsuario.Administrador
                ? ContrasenaTemporalAdministrador
                : ContrasenaTemporalUsuario;

            usuario.ContrasenaHash = ContrasenaService.CrearHash(contrasenaTemporal);
            usuario.DebeCambiarContrasena = true;
            _usuarioRepository.GuardarTodos(usuarios);
            return contrasenaTemporal;
        }

        /// <summary>
        /// Activa o desactiva una cuenta sin permitir que el administrador se bloquee a sí mismo.
        /// </summary>
        public void CambiarEstadoCuenta(int usuarioId, bool activar)
        {
            _sesion.ExigirAdministrador();
            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();
            Usuario usuario = ObtenerUsuarioExistente(usuarios, usuarioId);

            if (!activar && usuario.Id == _sesion.UsuarioActual.Id)
            {
                throw new InvalidOperationException(
                    "No puede desactivar la cuenta con la que inició sesión.");
            }

            if (!activar &&
                usuario.Rol == RolUsuario.Administrador &&
                usuarios.Count(cuenta =>
                    cuenta.Rol == RolUsuario.Administrador && cuenta.Activo) <= 1)
            {
                throw new InvalidOperationException(
                    "Debe permanecer al menos una cuenta administrativa activa.");
            }

            if (usuario.Activo == activar)
            {
                throw new InvalidOperationException(
                    activar ? "La cuenta ya está activa." : "La cuenta ya está desactivada.");
            }

            usuario.Activo = activar;
            _usuarioRepository.GuardarTodos(usuarios);
        }

        /// <summary>
        /// Reemplaza una contraseña temporal después de comprobar la contraseña actual.
        /// </summary>
        public Usuario CompletarCambioObligatorio(
            int usuarioId,
            string contrasenaActual,
            string nuevaContrasena)
        {
            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();
            Usuario usuario = ObtenerUsuarioExistente(usuarios, usuarioId);
            if (!usuario.Activo || !usuario.DebeCambiarContrasena)
            {
                throw new InvalidOperationException(
                    "La cuenta no tiene un cambio obligatorio de contraseña pendiente.");
            }

            if (!ContrasenaService.Verificar(contrasenaActual, usuario.ContrasenaHash))
            {
                throw new UnauthorizedAccessException(
                    "La contraseña temporal no es válida.");
            }

            ValidarContrasena(nuevaContrasena);
            if (ContrasenaService.Verificar(nuevaContrasena, usuario.ContrasenaHash))
            {
                throw new ArgumentException(
                    "La nueva contraseña debe ser diferente de la contraseña temporal.");
            }

            usuario.ContrasenaHash = ContrasenaService.CrearHash(nuevaContrasena);
            usuario.DebeCambiarContrasena = false;
            _usuarioRepository.GuardarTodos(usuarios);
            return usuario;
        }

        /// <summary>
        /// Registra una cuenta participante desde la pantalla de acceso.
        /// </summary>
        /// <remarks>
        /// El rol siempre es <see cref="RolUsuario.Usuario"/>; el registro
        /// público nunca puede crear administradores.
        /// </remarks>
        public Usuario RegistrarUsuarioPublico(
            string nombre,
            string paisPreferido,
            string nombreUsuario,
            string correo,
            string contrasena)
        {
            return GuardarUsuario(
                nombre,
                paisPreferido,
                nombreUsuario,
                correo,
                contrasena
            );
        }

        private Usuario GuardarUsuario(
            string nombre,
            string paisPreferido,
            string nombreUsuario,
            string correo,
            string contrasena)
        {
            ValidarRegistro(
                nombre,
                paisPreferido,
                nombreUsuario,
                correo,
                contrasena
            );

            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();

            if (usuarios.Any(usuario =>
                    usuario.Nombre.Equals(
                        nombre.Trim(),
                        StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ya existe un usuario con ese nombre."
                );
            }

            if (usuarios.Any(usuario =>
                    usuario.NombreUsuario.Equals(
                        nombreUsuario.Trim(),
                        StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ese nombre de usuario ya está en uso."
                );
            }

            if (usuarios.Any(usuario =>
                    usuario.Correo.Equals(
                        correo.Trim(),
                        StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ese correo ya está en uso."
                );
            }

            Usuario nuevoUsuario = new()
            {
                Id = usuarios.Count == 0
                    ? 1
                    : usuarios.Max(usuario => usuario.Id) + 1,
                Nombre = nombre.Trim(),
                NombreUsuario = nombreUsuario.Trim(),
                Correo = correo.Trim(),
                ContrasenaHash = ContrasenaService.CrearHash(contrasena),
                Rol = RolUsuario.Usuario,
                Activo = true,
                DebeCambiarContrasena = false,
                PaisPreferido = paisPreferido.Trim(),
                Puntos = 0,
                Insignias = new List<string>()
            };

            usuarios.Add(nuevoUsuario);
            _usuarioRepository.GuardarTodos(usuarios);

            return nuevoUsuario;
        }

        /// <summary>
        /// Completa las credenciales que no existían en los datos de la Parte 1.
        /// </summary>
        public void AsegurarCredencialesIniciales()
        {
            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();
            bool huboCambios = false;
            HashSet<string> nombresUtilizados = new(
                usuarios
                    .Where(usuario =>
                        !string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                    .Select(usuario => usuario.NombreUsuario),
                StringComparer.OrdinalIgnoreCase
            );

            foreach (Usuario usuario in usuarios.OrderBy(usuario => usuario.Id))
            {
                if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                {
                    usuario.NombreUsuario = HacerUnico(
                        NormalizarNombreUsuario(usuario.Nombre),
                        usuario.Id,
                        nombresUtilizados
                    );
                    nombresUtilizados.Add(usuario.NombreUsuario);
                    huboCambios = true;
                }

                if (string.IsNullOrWhiteSpace(usuario.Correo))
                {
                    usuario.Correo = $"{usuario.NombreUsuario}@quinegol.local";
                    huboCambios = true;
                }

                if (string.IsNullOrWhiteSpace(usuario.ContrasenaHash))
                {
                    usuario.ContrasenaHash =
                        ContrasenaService.CrearHash(
                            usuario.Rol == RolUsuario.Administrador
                                ? ContrasenaTemporalAdministrador
                                : ContrasenaTemporalUsuario
                        );
                    usuario.DebeCambiarContrasena = true;
                    huboCambios = true;
                }

                usuario.Insignias ??= new List<string>();
            }

            if (!usuarios.Any(usuario =>
                    usuario.Rol == RolUsuario.Administrador))
            {
                int nuevoId = usuarios.Count == 0
                    ? 1
                    : usuarios.Max(usuario => usuario.Id) + 1;
                string nombreAdministrador = HacerUnico(
                    "admin",
                    nuevoId,
                    nombresUtilizados
                );

                usuarios.Add(new Usuario
                {
                    Id = nuevoId,
                    Nombre = "Administrador",
                    NombreUsuario = nombreAdministrador,
                    Correo = $"{nombreAdministrador}@quinegol.local",
                    ContrasenaHash = ContrasenaService.CrearHash(
                        ContrasenaTemporalAdministrador
                    ),
                    Rol = RolUsuario.Administrador,
                    Activo = true,
                    DebeCambiarContrasena = true,
                    PaisPreferido = "Sin definir",
                    Insignias = new List<string>()
                });
                huboCambios = true;
            }

            if (huboCambios)
            {
                _usuarioRepository.GuardarTodos(usuarios);
            }
        }

        private static string HacerUnico(
            string baseNombre,
            int sufijo,
            HashSet<string> utilizados)
        {
            string candidato = string.IsNullOrWhiteSpace(baseNombre)
                ? $"usuario{sufijo}"
                : baseNombre;

            return utilizados.Contains(candidato)
                ? $"{candidato}.{sufijo}"
                : candidato;
        }

        private static string NormalizarNombreUsuario(string nombre)
        {
            string normalizado = (nombre ?? string.Empty)
                .Trim()
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD);
            StringBuilder resultado = new();
            bool ultimoFueSeparador = false;

            foreach (char caracter in normalizado)
            {
                UnicodeCategory categoria =
                    CharUnicodeInfo.GetUnicodeCategory(caracter);

                if (categoria == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                if (char.IsLetterOrDigit(caracter))
                {
                    resultado.Append(caracter);
                    ultimoFueSeparador = false;
                }
                else if (!ultimoFueSeparador && resultado.Length > 0)
                {
                    resultado.Append('.');
                    ultimoFueSeparador = true;
                }
            }

            return resultado
                .ToString()
                .Trim('.')
                .Normalize(NormalizationForm.FormC);
        }

        private static void ValidarRegistro(
            string nombre,
            string paisPreferido,
            string nombreUsuario,
            string correo,
            string contrasena)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException(
                    "Debe ingresar el nombre del usuario."
                );
            }

            if (string.IsNullOrWhiteSpace(paisPreferido))
            {
                throw new ArgumentException(
                    "Debe seleccionar un país."
                );
            }

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentException(
                    "Debe ingresar un nombre de usuario."
                );
            }

            string usuarioNormalizado = nombreUsuario.Trim();

            if (usuarioNormalizado.Length < 3 ||
                usuarioNormalizado.Any(caracter =>
                    !char.IsLetterOrDigit(caracter) &&
                    caracter is not '.' and not '_' and not '-'))
            {
                throw new ArgumentException(
                    "El nombre de usuario debe tener al menos 3 caracteres " +
                    "y solo puede contener letras, números, punto, guion o " +
                    "guion bajo."
                );
            }

            if (string.IsNullOrWhiteSpace(correo))
            {
                throw new ArgumentException(
                    "Debe ingresar un correo válido.",
                    nameof(correo)
                );
            }

            try
            {
                _ = new MailAddress(correo);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(
                    "Debe ingresar un correo válido.",
                    nameof(correo),
                    ex
                );
            }

            ValidarContrasena(contrasena);
        }

        private static void ValidarContrasena(string contrasena)
        {
            if (string.IsNullOrEmpty(contrasena) || contrasena.Length < 8)
            {
                throw new ArgumentException(
                    "La contraseña debe tener al menos 8 caracteres.");
            }
        }

        private static Usuario ObtenerUsuarioExistente(
            IEnumerable<Usuario> usuarios,
            int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("Debe seleccionar una cuenta.");
            }

            return usuarios.FirstOrDefault(usuario => usuario.Id == usuarioId)
                ?? throw new InvalidOperationException(
                    "No se encontró la cuenta seleccionada.");
        }
    }
}

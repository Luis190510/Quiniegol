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

        /// <summary>
        /// Inicializa el controlador con el archivo de usuarios del proyecto.
        /// </summary>
        public UsuarioController()
            : this(
                new JsonRepository<Usuario>(
                    RutaDatosService.ObtenerRuta("usuarios.json")
                )
            )
        {
        }

        /// <summary>
        /// Inicializa el controlador con un repositorio específico.
        /// </summary>
        /// <param name="usuarioRepository">Repositorio utilizado para persistir cuentas.</param>
        public UsuarioController(
            JsonRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ??
                throw new ArgumentNullException(nameof(usuarioRepository));

            AsegurarCredencialesIniciales();
        }

        /// <summary>Obtiene todas las cuentas registradas.</summary>
        /// <returns>Lista de cuentas persistidas.</returns>
        public List<Usuario> ObtenerUsuarios()
        {
            return _usuarioRepository.ObtenerTodos();
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

            if (string.IsNullOrEmpty(contrasena) ||
                contrasena.Length < 8)
            {
                throw new ArgumentException(
                    "La contraseña debe tener al menos 8 caracteres."
                );
            }
        }
    }
}

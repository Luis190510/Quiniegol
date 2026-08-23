using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>Administra quinielas privadas respetando membresía y propiedad.</summary>
    public class QuinielaController
    {
        private readonly JsonRepository<Quiniela> _quinielaRepository;
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly UsuarioController _usuarioController;

        public QuinielaController()
            : this(
                new JsonRepository<Quiniela>(
                    RutaDatosService.ObtenerRuta("quinielas.json")),
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta("pronosticos.json")),
                new UsuarioController())
        {
        }

        /// <summary>Inicializa el controlador con repositorios específicos.</summary>
        public QuinielaController(
            JsonRepository<Quiniela> quinielaRepository,
            JsonRepository<Pronostico> pronosticoRepository,
            UsuarioController usuarioController)
        {
            _quinielaRepository = quinielaRepository ??
                throw new ArgumentNullException(nameof(quinielaRepository));
            _pronosticoRepository = pronosticoRepository ??
                throw new ArgumentNullException(nameof(pronosticoRepository));
            _usuarioController = usuarioController ??
                throw new ArgumentNullException(nameof(usuarioController));
            AsegurarCreadores();
        }

        /// <summary>Obtiene solo las quinielas visibles para la sesión.</summary>
        public List<Quiniela> ObtenerQuinielas()
        {
            Usuario usuarioActual = SesionUsuarioService.UsuarioActual;
            return _quinielaRepository
                .ObtenerTodos()
                .Where(quiniela =>
                    AccesoQuinielaService.PuedeConsultar(
                        quiniela,
                        usuarioActual))
                .OrderBy(quiniela => quiniela.Nombre)
                .ToList();
        }

        /// <summary>
        /// Obtiene solo identificador y nombre de las quinielas a las que el
        /// participante todavía puede unirse.
        /// </summary>
        public List<QuinielaDisponibleItem> ObtenerQuinielasDisponibles()
        {
            Usuario usuario = SesionUsuarioService.UsuarioActual;

            if (usuario.Rol == RolUsuario.Administrador)
            {
                return new List<QuinielaDisponibleItem>();
            }

            return _quinielaRepository
                .ObtenerTodos()
                .Where(quiniela =>
                    AccesoQuinielaService.PuedeUnirse(quiniela, usuario))
                .OrderBy(quiniela => quiniela.Nombre)
                .Select(quiniela => new QuinielaDisponibleItem
                {
                    QuinielaId = quiniela.Id,
                    Nombre = quiniela.Nombre
                })
                .ToList();
        }

        /// <summary>Crea una quiniela y agrega al creador como integrante.</summary>
        public void CrearQuiniela(
            string nombre,
            string descripcion,
            List<int> integrantesIds)
        {
            Usuario creador = SesionUsuarioService.UsuarioActual;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException(
                    "Debe escribir el nombre de la quiniela."
                );
            }

            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            if (quinielas.Any(quiniela =>
                    quiniela.Nombre.Equals(
                        nombre.Trim(),
                        StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ya existe una quiniela con ese nombre."
                );
            }

            List<Usuario> usuarios =
                _usuarioController.ObtenerUsuarios();
            List<int> integrantes = (integrantesIds ?? new List<int>())
                .Distinct()
                .ToList();

            if (!SesionUsuarioService.EsAdministrador &&
                !integrantes.Contains(creador.Id))
            {
                integrantes.Add(creador.Id);
            }

            if (integrantes.Any(usuarioId =>
                    !usuarios.Any(usuario =>
                        usuario.Id == usuarioId &&
                        usuario.Rol == RolUsuario.Usuario)))
            {
                throw new InvalidOperationException(
                    "Uno de los integrantes no existe o no es participante."
                );
            }

            Quiniela nuevaQuiniela = new()
            {
                Id = quinielas.Count == 0
                    ? 1
                    : quinielas.Max(quiniela => quiniela.Id) + 1,
                Nombre = nombre.Trim(),
                Descripcion = descripcion?.Trim() ?? string.Empty,
                Tipo = "Privada",
                CreadorUsuarioId = creador.Id,
                IntegrantesIds = integrantes
            };

            quinielas.Add(nuevaQuiniela);
            _quinielaRepository.GuardarTodos(quinielas);
        }

        /// <summary>
        /// Inscribe al usuario actual en la quiniela seleccionada.
        /// La lista previa solo expone identificador y nombre.
        /// </summary>
        public void UnirseAQuiniela(int quinielaId)
        {
            if (quinielaId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar una quiniela disponible."
                );
            }

            Usuario usuario = SesionUsuarioService.UsuarioActual;
            List<Quiniela> quinielas = _quinielaRepository.ObtenerTodos();
            Quiniela quiniela = quinielas.FirstOrDefault(elemento =>
                elemento.Id == quinielaId)
                ?? throw new InvalidOperationException(
                    "No se encontró la quiniela seleccionada."
                );

            if (!AccesoQuinielaService.PuedeUnirse(quiniela, usuario))
            {
                throw new InvalidOperationException(
                    usuario.Rol == RolUsuario.Administrador
                        ? "El administrador consulta las quinielas sin inscribirse."
                        : "El usuario ya pertenece a esta quiniela."
                );
            }

            quiniela.IntegrantesIds.Add(usuario.Id);
            _quinielaRepository.GuardarTodos(quinielas);
        }

        /// <summary>Agrega un integrante si la sesión administra la quiniela.</summary>
        public void AgregarIntegrante(int quinielaId, int usuarioId)
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();
            Quiniela quiniela = ObtenerExistente(quinielas, quinielaId);
            ExigirAdministracion(quiniela);

            if (!_usuarioController.ObtenerUsuarios()
                .Any(usuario =>
                    usuario.Id == usuarioId &&
                    usuario.Rol == RolUsuario.Usuario))
            {
                throw new InvalidOperationException(
                    "No se encontró un participante con ese identificador."
                );
            }

            if (quiniela.IntegrantesIds.Contains(usuarioId))
            {
                throw new InvalidOperationException(
                    "El usuario ya pertenece a esta quiniela."
                );
            }

            quiniela.IntegrantesIds.Add(usuarioId);
            _quinielaRepository.GuardarTodos(quinielas);
        }

        /// <summary>Retira un integrante sin permitir eliminar al creador.</summary>
        public void EliminarIntegrante(int quinielaId, int usuarioId)
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();
            Quiniela quiniela = ObtenerExistente(quinielas, quinielaId);
            ExigirAdministracion(quiniela);

            if (usuarioId == quiniela.CreadorUsuarioId)
            {
                throw new InvalidOperationException(
                    "El creador no puede retirarse de su propia quiniela."
                );
            }

            if (!quiniela.IntegrantesIds.Remove(usuarioId))
            {
                throw new InvalidOperationException(
                    "El usuario no pertenece a esta quiniela."
                );
            }

            _quinielaRepository.GuardarTodos(quinielas);
        }

        /// <summary>Obtiene integrantes si la sesión pertenece a la quiniela.</summary>
        public List<Usuario> ObtenerIntegrantes(int quinielaId)
        {
            Quiniela quiniela = ObtenerExistente(
                _quinielaRepository.ObtenerTodos(),
                quinielaId
            );
            ExigirAcceso(quiniela);

            return _usuarioController
                .ObtenerUsuarios()
                .Where(usuario =>
                    quiniela.IntegrantesIds.Contains(usuario.Id))
                .OrderBy(usuario => usuario.Nombre)
                .ToList();
        }

        /// <summary>
        /// Obtiene el resumen privado de integrantes, incluidos sus pronósticos
        /// que contienen posibles goleadores.
        /// </summary>
        public List<QuinielaIntegranteItem> ObtenerResumenIntegrantes(
            int quinielaId)
        {
            Quiniela quiniela = ObtenerExistente(
                _quinielaRepository.ObtenerTodos(),
                quinielaId
            );
            ExigirAcceso(quiniela);

            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            return _usuarioController
                .ObtenerUsuarios()
                .Where(usuario =>
                    quiniela.IntegrantesIds.Contains(usuario.Id))
                .OrderBy(usuario => usuario.Nombre)
                .Select(usuario => new QuinielaIntegranteItem
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    PaisPreferido = usuario.PaisPreferido,
                    Puntos = usuario.Puntos,
                    PronosticosConGoleadores = pronosticos.Count(pronostico =>
                        pronostico.UsuarioId == usuario.Id &&
                        GoleadoresPronosticoService.TieneGoleadores(pronostico))
                })
                .ToList();
        }

        /// <summary>Elimina una quiniela administrada por la sesión.</summary>
        public void EliminarQuiniela(int quinielaId)
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();
            Quiniela quiniela = ObtenerExistente(quinielas, quinielaId);
            ExigirAdministracion(quiniela);

            quinielas.Remove(quiniela);
            _quinielaRepository.GuardarTodos(quinielas);
        }

        private void AsegurarCreadores()
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();
            bool huboCambios = false;

            foreach (Quiniela quiniela in quinielas)
            {
                quiniela.IntegrantesIds ??= new List<int>();

                if (quiniela.CreadorUsuarioId == 0 &&
                    quiniela.IntegrantesIds.Count > 0)
                {
                    quiniela.CreadorUsuarioId =
                        quiniela.IntegrantesIds[0];
                    huboCambios = true;
                }
            }

            if (huboCambios)
            {
                _quinielaRepository.GuardarTodos(quinielas);
            }
        }

        private static Quiniela ObtenerExistente(
            List<Quiniela> quinielas,
            int quinielaId)
        {
            return quinielas.FirstOrDefault(quiniela =>
                       quiniela.Id == quinielaId)
                   ?? throw new InvalidOperationException(
                       "No se encontró la quiniela."
                   );
        }

        private static void ExigirAcceso(Quiniela quiniela)
        {
            AccesoQuinielaService.ExigirConsulta(
                quiniela,
                SesionUsuarioService.UsuarioActual
            );
        }

        private static void ExigirAdministracion(Quiniela quiniela)
        {
            AccesoQuinielaService.ExigirAdministracion(
                quiniela,
                SesionUsuarioService.UsuarioActual
            );
        }
    }
}

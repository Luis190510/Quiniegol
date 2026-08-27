using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Administra los pronósticos del participante autenticado.
    /// </summary>
    public class PronosticoController
    {
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly UsuarioController _usuarioController;
        private readonly PartidoController _partidoController;
        private readonly FechaSimuladaService _fechaService;

        public PronosticoController()
            : this(
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta("pronosticos.json")),
                new UsuarioController(),
                new PartidoController(),
                FechaSimuladaService.Instancia)
        {
        }

        /// <summary>Inicializa el controlador con datos específicos.</summary>
        public PronosticoController(
            JsonRepository<Pronostico> pronosticoRepository,
            UsuarioController usuarioController,
            PartidoController partidoController,
            FechaSimuladaService fechaService)
        {
            _pronosticoRepository = pronosticoRepository ??
                throw new ArgumentNullException(nameof(pronosticoRepository));
            _usuarioController = usuarioController ??
                throw new ArgumentNullException(nameof(usuarioController));
            _partidoController = partidoController ??
                throw new ArgumentNullException(nameof(partidoController));
            _fechaService = fechaService ??
                throw new ArgumentNullException(nameof(fechaService));
        }

        /// <summary>
        /// Obtiene todos los pronósticos para el administrador y solo los propios
        /// para un participante.
        /// </summary>
        public List<Pronostico> ObtenerPronosticos()
        {
            Usuario usuarioActual = SesionUsuarioService.UsuarioActual;
            List<Pronostico> pronosticos = _pronosticoRepository.ObtenerTodos();
            return SesionUsuarioService.EsAdministrador
                ? pronosticos
                : pronosticos
                    .Where(pronostico => pronostico.UsuarioId == usuarioActual.Id)
                    .ToList();
        }

        /// <summary>
        /// Registra el marcador y los posibles goleadores elegidos por el usuario.
        /// </summary>
        public void RegistrarPronostico(
            int usuarioId,
            int partidoId,
            int golesLocal,
            int golesVisitante,
            IEnumerable<string>? goleadoresLocal = null,
            IEnumerable<string>? goleadoresVisitante = null)
        {
            Usuario usuarioActual = SesionUsuarioService.UsuarioActual;
            ValidarSolicitud(usuarioActual, usuarioId, partidoId, golesLocal, golesVisitante);
            ValidarUsuarioExistente(usuarioId);

            Partido partido = _partidoController.ObtenerPartidos()
                .FirstOrDefault(actual => actual.Id == partidoId)
                ?? throw new InvalidOperationException(
                    "No se encontró el partido seleccionado.");
            if (_fechaService.FechaActual >= partido.FechaHora)
            {
                throw new InvalidOperationException(
                    "El partido ya inició. No se permiten pronósticos.");
            }

            List<Pronostico> pronosticos = _pronosticoRepository.ObtenerTodos();
            if (pronosticos.Any(pronostico =>
                pronostico.UsuarioId == usuarioId && pronostico.PartidoId == partidoId))
            {
                throw new InvalidOperationException(
                    "El usuario ya registró un pronóstico para este partido.");
            }

            pronosticos.Add(new Pronostico
            {
                Id = pronosticos.Count == 0
                    ? 1
                    : pronosticos.Max(pronostico => pronostico.Id) + 1,
                UsuarioId = usuarioId,
                PartidoId = partidoId,
                GolesLocalPronosticados = golesLocal,
                GolesVisitantePronosticados = golesVisitante,
                FechaRegistro = _fechaService.FechaActual,
                PuntosObtenidos = null,
                GoleadoresLocalPronosticados =
                    GoleadoresPronosticoService.Normalizar(goleadoresLocal),
                GoleadoresVisitantePronosticados =
                    GoleadoresPronosticoService.Normalizar(goleadoresVisitante),
                GoleadoresConfirmados = true
            });
            _pronosticoRepository.GuardarTodos(pronosticos);
        }

        private static void ValidarSolicitud(
            Usuario usuarioActual,
            int usuarioId,
            int partidoId,
            int golesLocal,
            int golesVisitante)
        {
            if (SesionUsuarioService.EsAdministrador)
            {
                throw new UnauthorizedAccessException(
                    "El administrador no participa en los pronósticos.");
            }

            if (usuarioId != usuarioActual.Id)
            {
                throw new UnauthorizedAccessException(
                    "No puede registrar un pronóstico a nombre de otra persona.");
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException("Debe seleccionar un usuario.");
            }

            if (partidoId <= 0)
            {
                throw new ArgumentException("Debe seleccionar un partido.");
            }

            if (golesLocal < 0 || golesVisitante < 0)
            {
                throw new ArgumentException(
                    "Los goles pronosticados no pueden ser negativos.");
            }
        }

        private void ValidarUsuarioExistente(int usuarioId)
        {
            if (!_usuarioController.ObtenerUsuarios().Any(usuario => usuario.Id == usuarioId))
            {
                throw new InvalidOperationException(
                    "No se encontró el usuario seleccionado.");
            }
        }
    }
}

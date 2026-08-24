using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Localiza partidos próximos que un participante todavía no ha pronosticado.
    /// </summary>
    public class NotificacionPronosticoController
    {
        private readonly JsonRepository<Partido> _partidoRepository;
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly FechaSimuladaService _fechaService;

        /// <summary>Inicializa el controlador con los datos del proyecto.</summary>
        public NotificacionPronosticoController()
            : this(
                new JsonRepository<Partido>(
                    RutaDatosService.ObtenerRuta("partidos.json")),
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta("pronosticos.json")),
                new JsonRepository<Seleccion>(
                    RutaDatosService.ObtenerRuta("selecciones.json")),
                FechaSimuladaService.Instancia)
        {
        }

        /// <summary>
        /// Inicializa el controlador con fuentes de datos específicas.
        /// </summary>
        public NotificacionPronosticoController(
            JsonRepository<Partido> partidoRepository,
            JsonRepository<Pronostico> pronosticoRepository,
            JsonRepository<Seleccion> seleccionRepository,
            FechaSimuladaService fechaService)
        {
            _partidoRepository = partidoRepository ??
                throw new ArgumentNullException(nameof(partidoRepository));
            _pronosticoRepository = pronosticoRepository ??
                throw new ArgumentNullException(nameof(pronosticoRepository));
            _seleccionRepository = seleccionRepository ??
                throw new ArgumentNullException(nameof(seleccionRepository));
            _fechaService = fechaService ??
                throw new ArgumentNullException(nameof(fechaService));
        }

        /// <summary>
        /// Obtiene los partidos que comienzan en las próximas 24 horas y para
        /// los cuales el participante todavía no registró un pronóstico.
        /// </summary>
        /// <param name="usuario">Usuario que acaba de iniciar sesión.</param>
        /// <returns>Partidos pendientes ordenados por fecha de inicio.</returns>
        public List<NotificacionPronosticoItem> ObtenerPendientes(
            Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (usuario.Rol == RolUsuario.Administrador)
            {
                return new List<NotificacionPronosticoItem>();
            }

            DateTime desde = _fechaService.FechaActual;
            DateTime hasta = desde.AddHours(24);
            HashSet<int> partidosPronosticados = _pronosticoRepository
                .ObtenerTodos()
                .Where(pronostico => pronostico.UsuarioId == usuario.Id)
                .Select(pronostico => pronostico.PartidoId)
                .ToHashSet();
            Dictionary<int, string> selecciones = _seleccionRepository
                .ObtenerTodos()
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);

            return _partidoRepository.ObtenerTodos()
                .Where(partido =>
                    partido.FechaHora > desde &&
                    partido.FechaHora <= hasta &&
                    !partidosPronosticados.Contains(partido.Id))
                .OrderBy(partido => partido.FechaHora)
                .ThenBy(partido => partido.Id)
                .Select(partido => new NotificacionPronosticoItem
                {
                    PartidoId = partido.Id,
                    FechaHora = partido.FechaHora,
                    Partido = $"{ObtenerNombre(selecciones, partido.SeleccionLocalId)} vs " +
                        ObtenerNombre(selecciones, partido.SeleccionVisitanteId)
                })
                .ToList();
        }

        private static string ObtenerNombre(
            IReadOnlyDictionary<int, string> selecciones,
            int seleccionId)
        {
            return selecciones.TryGetValue(seleccionId, out string? nombre)
                ? nombre
                : $"Selección {seleccionId}";
        }
    }
}

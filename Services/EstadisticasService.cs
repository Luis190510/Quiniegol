using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>
    /// Carga los datos del rango y solicita el reporte permitido para la sesión.
    /// </summary>
    public class EstadisticasService
    {
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Usuario> _usuarioRepository;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly PartidoController _partidoController;
        private readonly PuntajeController _puntajeController;

        public EstadisticasService()
        {
            _pronosticoRepository = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json"));
            _usuarioRepository = new JsonRepository<Usuario>(
                RutaDatosService.ObtenerRuta("usuarios.json"));
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _partidoController = new PartidoController();
            _puntajeController = new PuntajeController();
        }

        /// <summary>
        /// Obtiene los indicadores administrativos o personales según la sesión.
        /// </summary>
        public List<EstadisticaItem> ObtenerReportePorRol(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            DateTime inicio = fechaDesde.Date;
            DateTime final = fechaHasta.Date.AddDays(1).AddTicks(-1);
            if (inicio > final)
            {
                throw new ArgumentException(
                    "La fecha inicial no puede ser mayor que la fecha final.");
            }

            Usuario solicitante = SesionUsuarioService.UsuarioActual;
            _puntajeController.CalcularTodosLosPuntajes();

            List<Partido> partidosRango = _partidoController.ObtenerPartidos()
                .Where(partido => partido.FechaHora >= inicio && partido.FechaHora <= final)
                .ToList();
            List<Pronostico> pronosticosRango = FiltrarPronosticosDePartidos(
                _pronosticoRepository.ObtenerTodos(),
                partidosRango);

            return ReportePorRolService.CrearReporte(
                solicitante,
                pronosticosRango,
                partidosRango,
                _usuarioRepository.ObtenerTodos(),
                _seleccionRepository.ObtenerTodos());
        }

        /// <summary>
        /// Alias conservado para las pantallas de la versión anterior.
        /// </summary>
        public List<EstadisticaItem> ObtenerEstadisticas(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            return ObtenerReportePorRol(fechaDesde, fechaHasta);
        }

        /// <summary>
        /// Conserva los pronósticos de los partidos incluidos en el rango. La fecha
        /// relevante es la del encuentro, aunque el pronóstico se registrara antes.
        /// </summary>
        public static List<Pronostico> FiltrarPronosticosDePartidos(
            IEnumerable<Pronostico> pronosticos,
            IEnumerable<Partido> partidosRango)
        {
            HashSet<int> partidosIds = partidosRango
                .Select(partido => partido.Id)
                .ToHashSet();
            return pronosticos
                .Where(pronostico => partidosIds.Contains(pronostico.PartidoId))
                .ToList();
        }
    }
}

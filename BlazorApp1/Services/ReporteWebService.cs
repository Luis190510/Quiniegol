using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>Genera el reporte permitido para la sesión web actual.</summary>
    public sealed class ReporteWebService
    {
        private readonly SesionUsuarioService _sesion;
        private readonly FechaSimuladaService _fechaSimulada;
        private readonly JsonRepository<Pronostico> _pronosticos;
        private readonly JsonRepository<Partido> _partidos;
        private readonly JsonRepository<Usuario> _usuarios;
        private readonly JsonRepository<Seleccion> _selecciones;

        public ReporteWebService(
            SesionUsuarioService sesion,
            FechaSimuladaService fechaSimulada)
        {
            _sesion = sesion;
            _fechaSimulada = fechaSimulada;
            _pronosticos = CrearRepositorio<Pronostico>("pronosticos.json");
            _partidos = CrearRepositorio<Partido>("partidos.json");
            _usuarios = CrearRepositorio<Usuario>("usuarios.json");
            _selecciones = CrearRepositorio<Seleccion>("selecciones.json");
        }

        public List<EstadisticaItem> Generar(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
            {
                throw new ArgumentException(
                    "La fecha inicial no puede ser posterior a la fecha final.");
            }

            List<Partido> partidos = _partidos.ObtenerTodos()
                .Where(partido =>
                    partido.FechaHora.Date >= desde.Date &&
                    partido.FechaHora.Date <= hasta.Date)
                .Select(OcultarResultadoFuturo)
                .ToList();
            HashSet<int> partidosIds = partidos
                .Select(partido => partido.Id)
                .ToHashSet();
            List<Pronostico> pronosticos = _pronosticos.ObtenerTodos()
                .Where(pronostico => partidosIds.Contains(pronostico.PartidoId))
                .ToList();

            return ReportePorRolService.CrearReporte(
                _sesion.UsuarioActual,
                pronosticos,
                partidos,
                _usuarios.ObtenerTodos(),
                _selecciones.ObtenerTodos());
        }

        private Partido OcultarResultadoFuturo(Partido partido)
        {
            if (partido.FechaHora <= _fechaSimulada.FechaActual)
            {
                return partido;
            }

            return new Partido
            {
                Id = partido.Id,
                SeleccionLocalId = partido.SeleccionLocalId,
                SeleccionVisitanteId = partido.SeleccionVisitanteId,
                FechaHora = partido.FechaHora,
                Estado = "Pendiente"
            };
        }

        private static JsonRepository<T> CrearRepositorio<T>(string archivo)
        {
            return new JsonRepository<T>(RutaDatosService.ObtenerRuta(archivo));
        }
    }
}

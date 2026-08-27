using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>Consulta partidos anteriores y próximos con nombres legibles.</summary>
    public class InformacionPartidosController
    {
        private readonly PartidoController _partidoController;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly FechaSimuladaService _fechaService;

        public InformacionPartidosController()
        {
            _partidoController = new PartidoController();
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _fechaService = FechaSimuladaService.Instancia;
        }

        /// <summary>Obtiene los cinco encuentros finalizados más recientes.</summary>
        public List<PartidoInformacionItem> ObtenerUltimosCinco()
        {
            Dictionary<int, string> nombres = ObtenerNombresSelecciones();

            return _partidoController.ObtenerPartidos()
                .Where(partido =>
                    partido.Estado == "Finalizado" &&
                    partido.FechaHora <= _fechaService.FechaActual)
                .OrderByDescending(partido => partido.FechaHora)
                .Take(5)
                .Select(partido => CrearItem(partido, nombres))
                .ToList();
        }

        /// <summary>Obtiene los encuentros programados durante las próximas 24 horas.</summary>
        public List<PartidoInformacionItem> ObtenerProximos24Horas()
        {
            DateTime desde = _fechaService.FechaActual;
            DateTime hasta = desde.AddHours(24);
            Dictionary<int, string> nombres = ObtenerNombresSelecciones();

            return _partidoController.ObtenerPartidos()
                .Where(partido => partido.FechaHora > desde && partido.FechaHora <= hasta)
                .OrderBy(partido => partido.FechaHora)
                .Select(partido => CrearItem(partido, nombres))
                .ToList();
        }

        public DateTime ObtenerFechaSimulada() => _fechaService.FechaActual;

        private Dictionary<int, string> ObtenerNombresSelecciones()
        {
            return _seleccionRepository.ObtenerTodos()
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);
        }

        private static PartidoInformacionItem CrearItem(
            Partido partido,
            IReadOnlyDictionary<int, string> nombres)
        {
            string local = ObtenerNombre(nombres, partido.SeleccionLocalId);
            string visitante = ObtenerNombre(nombres, partido.SeleccionVisitanteId);
            string marcador = partido.GolesLocal.HasValue && partido.GolesVisitante.HasValue
                ? $"{partido.GolesLocal} - {partido.GolesVisitante}"
                : "Pendiente";

            return new PartidoInformacionItem
            {
                PartidoId = partido.Id,
                FechaHora = partido.FechaHora,
                Partido = $"{local} vs {visitante}",
                Estado = partido.Estado,
                Marcador = marcador
            };
        }

        private static string ObtenerNombre(
            IReadOnlyDictionary<int, string> nombres,
            int seleccionId)
        {
            return nombres.TryGetValue(seleccionId, out string? nombre)
                ? nombre
                : $"Selección {seleccionId}";
        }
    }
}

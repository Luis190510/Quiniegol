using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Prepara el historial de pronósticos que se muestra a cada usuario.
    /// </summary>
    public class HistorialPronosticoController
    {
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly PartidoController _partidoController;

        public HistorialPronosticoController()
        {
            _pronosticoRepository = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json"));
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _partidoController = new PartidoController();
        }

        /// <summary>
        /// Obtiene el historial propio o, si la sesión es administrativa, el de otro usuario.
        /// </summary>
        public List<HistorialPronosticoItem> ObtenerPorUsuario(int usuarioId)
        {
            ValidarAcceso(usuarioId);

            Dictionary<int, Partido> partidosPorId = _partidoController.ObtenerPartidos()
                .ToDictionary(partido => partido.Id);
            Dictionary<int, string> seleccionesPorId = _seleccionRepository.ObtenerTodos()
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);

            return _pronosticoRepository.ObtenerTodos()
                .Where(pronostico => pronostico.UsuarioId == usuarioId)
                .Where(pronostico => partidosPorId.ContainsKey(pronostico.PartidoId))
                .Select(pronostico => CrearFila(
                    pronostico,
                    partidosPorId[pronostico.PartidoId],
                    seleccionesPorId))
                .OrderByDescending(fila => fila.FechaRegistro)
                .ToList();
        }

        private static void ValidarAcceso(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("Debe seleccionar un usuario.");
            }

            if (!SesionUsuarioService.EsAdministrador &&
                usuarioId != SesionUsuarioService.UsuarioActual.Id)
            {
                throw new UnauthorizedAccessException(
                    "Solo puede consultar su propio historial.");
            }
        }

        private static HistorialPronosticoItem CrearFila(
            Pronostico pronostico,
            Partido partido,
            IReadOnlyDictionary<int, string> selecciones)
        {
            string local = ObtenerNombreSeleccion(selecciones, partido.SeleccionLocalId);
            string visitante = ObtenerNombreSeleccion(selecciones, partido.SeleccionVisitanteId);
            string resultadoReal = PartidoTieneResultado(partido)
                ? $"{partido.GolesLocal} - {partido.GolesVisitante}"
                : "Pendiente";
            string puntos = pronostico.PuntosObtenidos?.ToString() ?? "Pendiente";

            return new HistorialPronosticoItem
            {
                PronosticoId = pronostico.Id,
                FechaRegistro = pronostico.FechaRegistro,
                Partido = $"{local} vs {visitante}",
                MarcadorPronosticado =
                    $"{pronostico.GolesLocalPronosticados} - {pronostico.GolesVisitantePronosticados}",
                GoleadoresPronosticados = GoleadoresPronosticoService.Formatear(
                    pronostico,
                    local,
                    visitante),
                ResultadoReal = resultadoReal,
                Estado = partido.Estado,
                Puntos = puntos
            };
        }

        private static bool PartidoTieneResultado(Partido partido)
        {
            return partido.Estado == "Finalizado" &&
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue;
        }

        private static string ObtenerNombreSeleccion(
            IReadOnlyDictionary<int, string> selecciones,
            int seleccionId)
        {
            return selecciones.GetValueOrDefault(seleccionId, $"Selección {seleccionId}");
        }
    }
}

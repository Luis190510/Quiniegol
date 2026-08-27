using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>Construye la actividad de una quiniela sin revelar partidos pendientes.</summary>
    public class TimelineService
    {
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly PartidoController _partidoController;
        private readonly QuinielaController _quinielaController;

        public TimelineService()
        {
            _pronosticoRepository = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json")
            );
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json")
            );
            _partidoController = new PartidoController();
            _quinielaController = new QuinielaController();
        }

        /// <summary>
        /// Obtiene logros de pronósticos finalizados para miembros autorizados.
        /// </summary>
        public List<Notificacion> ObtenerPorQuiniela(int quinielaId)
        {
            new PuntajeController().CalcularTodosLosPuntajes();

            Dictionary<int, Usuario> integrantes = _quinielaController
                .ObtenerIntegrantes(quinielaId)
                .ToDictionary(usuario => usuario.Id);
            Dictionary<int, Partido> partidosFinalizados = _partidoController
                .ObtenerPartidos()
                .Where(partido => partido.Estado == "Finalizado")
                .ToDictionary(partido => partido.Id);
            Dictionary<int, string> selecciones = _seleccionRepository.ObtenerTodos()
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);

            return _pronosticoRepository.ObtenerTodos()
                .Where(pronostico =>
                    integrantes.ContainsKey(pronostico.UsuarioId) &&
                    pronostico.PuntosObtenidos.HasValue)
                .Select(pronostico => CrearNotificacion(
                    pronostico,
                    integrantes,
                    partidosFinalizados,
                    selecciones))
                .Where(notificacion => notificacion != null)
                .Cast<Notificacion>()
                .OrderByDescending(notificacion => notificacion.Fecha)
                .ThenByDescending(notificacion => notificacion.Id)
                .ToList();
        }

        private static Notificacion? CrearNotificacion(
            Pronostico pronostico,
            IReadOnlyDictionary<int, Usuario> usuarios,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            if (!usuarios.TryGetValue(pronostico.UsuarioId, out Usuario? usuario) ||
                !partidos.TryGetValue(pronostico.PartidoId, out Partido? partido))
            {
                return null;
            }

            string local = selecciones.GetValueOrDefault(
                partido.SeleccionLocalId,
                $"Selección {partido.SeleccionLocalId}");
            string visitante = selecciones.GetValueOrDefault(
                partido.SeleccionVisitanteId,
                $"Selección {partido.SeleccionVisitanteId}");

            return new Notificacion
            {
                Id = pronostico.Id,
                Fecha = partido.FechaHora.AddMinutes(120),
                Mensaje =
                    $"{usuario.Nombre} obtuvo {pronostico.PuntosObtenidos} " +
                    $"puntos en {local} vs {visitante}. " +
                    $"Goleadores pronosticados: " +
                    GoleadoresPronosticoService.Formatear(
                        pronostico,
                        local,
                        visitante) + "."
            };
        }
    }
}

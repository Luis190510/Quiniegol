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

            List<Usuario> integrantes =
                _quinielaController.ObtenerIntegrantes(quinielaId);
            HashSet<int> integrantesIds =
                integrantes.Select(usuario => usuario.Id).ToHashSet();
            List<Partido> partidos =
                _partidoController.ObtenerPartidos();
            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            List<Notificacion> actividad =
                _pronosticoRepository
                    .ObtenerTodos()
                    .Where(pronostico =>
                        integrantesIds.Contains(pronostico.UsuarioId) &&
                        pronostico.PuntosObtenidos.HasValue)
                    .Select(pronostico => CrearNotificacion(
                        pronostico,
                        integrantes,
                        partidos,
                        selecciones))
                    .Where(notificacion => notificacion != null)
                    .Cast<Notificacion>()
                    .OrderByDescending(notificacion => notificacion.Fecha)
                    .ThenByDescending(notificacion => notificacion.Id)
                    .ToList();

            return actividad;
        }

        private static Notificacion? CrearNotificacion(
            Pronostico pronostico,
            List<Usuario> usuarios,
            List<Partido> partidos,
            List<Seleccion> selecciones)
        {
            Usuario? usuario = usuarios.FirstOrDefault(elemento =>
                elemento.Id == pronostico.UsuarioId);
            Partido? partido = partidos.FirstOrDefault(elemento =>
                elemento.Id == pronostico.PartidoId &&
                elemento.Estado == "Finalizado");

            if (usuario == null || partido == null)
            {
                return null;
            }

            string local = selecciones.FirstOrDefault(seleccion =>
                seleccion.Id == partido.SeleccionLocalId)?.Nombre
                ?? $"Selección {partido.SeleccionLocalId}";
            string visitante = selecciones.FirstOrDefault(seleccion =>
                seleccion.Id == partido.SeleccionVisitanteId)?.Nombre
                ?? $"Selección {partido.SeleccionVisitanteId}";

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

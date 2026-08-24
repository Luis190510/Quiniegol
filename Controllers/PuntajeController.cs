using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;
using Quiniegol.Strategies;

namespace Quiniegol.Controllers
{
    /// <summary>Calcula los puntos y construye el ranking global.</summary>
    public class PuntajeController
    {
        private readonly JsonRepository<Usuario> _usuarioRepository;
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly PartidoController _partidoController;
        private readonly CalculadoraPuntajeService _calculadoraPuntaje;

        public PuntajeController()
        {
            _usuarioRepository = new JsonRepository<Usuario>(
                RutaDatosService.ObtenerRuta("usuarios.json"));
            _pronosticoRepository = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json"));
            _partidoController = new PartidoController();
            _calculadoraPuntaje = new CalculadoraPuntajeService();
        }

        /// <summary>Recalcula pronósticos y totales según los partidos finalizados.</summary>
        public void CalcularTodosLosPuntajes()
        {
            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();
            List<Pronostico> pronosticos = _pronosticoRepository.ObtenerTodos();
            Dictionary<int, Partido> partidos = _partidoController.ObtenerPartidos()
                .ToDictionary(partido => partido.Id);

            foreach (Pronostico pronostico in pronosticos)
            {
                pronostico.PuntosObtenidos =
                    partidos.TryGetValue(pronostico.PartidoId, out Partido? partido) &&
                    PartidoTieneResultado(partido)
                        ? _calculadoraPuntaje.Calcular(pronostico, partido)
                        : null;
            }

            Dictionary<int, int> puntosPorUsuario = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos.HasValue)
                .GroupBy(pronostico => pronostico.UsuarioId)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Sum(pronostico => pronostico.PuntosObtenidos!.Value));

            foreach (Usuario usuario in usuarios)
            {
                usuario.Puntos = puntosPorUsuario.GetValueOrDefault(usuario.Id);
            }

            _pronosticoRepository.GuardarTodos(pronosticos);
            _usuarioRepository.GuardarTodos(usuarios);
        }

        public List<RankingItem> ObtenerRanking()
        {
            CalcularTodosLosPuntajes();

            return RankingService.Crear(
                _usuarioRepository.ObtenerTodos()
                    .Where(usuario => usuario.Rol == RolUsuario.Usuario),
                usuario => VisibilidadInsigniasService.ObtenerGlobales(usuario.Insignias));
        }

        private static bool PartidoTieneResultado(Partido partido)
        {
            return partido.Estado == "Finalizado" &&
                   partido.GolesLocal.HasValue &&
                   partido.GolesVisitante.HasValue;
        }
    }
}

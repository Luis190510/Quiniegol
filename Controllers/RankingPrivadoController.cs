using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Construye el ranking de una quiniela visible para la sesión actual.
    /// </summary>
    public class RankingPrivadoController
    {
        private readonly JsonRepository<Quiniela> _quinielaRepository;
        private readonly JsonRepository<Usuario> _usuarioRepository;
        private readonly PuntajeController _puntajeController;

        public RankingPrivadoController()
        {
            _quinielaRepository = new JsonRepository<Quiniela>(
                RutaDatosService.ObtenerRuta("quinielas.json"));
            _usuarioRepository = new JsonRepository<Usuario>(
                RutaDatosService.ObtenerRuta("usuarios.json"));
            _puntajeController = new PuntajeController();
        }

        /// <summary>
        /// Obtiene el ranking de integrantes y las insignias visibles en la quiniela.
        /// </summary>
        public List<RankingItem> ObtenerRanking(int quinielaId)
        {
            _puntajeController.CalcularTodosLosPuntajes();

            Quiniela quiniela = _quinielaRepository.ObtenerTodos()
                .FirstOrDefault(actual => actual.Id == quinielaId)
                ?? throw new InvalidOperationException("No se encontró la quiniela.");
            AccesoQuinielaService.ExigirConsulta(
                quiniela,
                SesionUsuarioService.UsuarioActual);

            HashSet<int> integrantesIds = quiniela.IntegrantesIds.ToHashSet();
            IEnumerable<Usuario> integrantes = _usuarioRepository.ObtenerTodos()
                .Where(usuario =>
                    usuario.Rol == RolUsuario.Usuario &&
                    integrantesIds.Contains(usuario.Id));

            return RankingService.Crear(
                integrantes,
                usuario => VisibilidadInsigniasService.ObtenerDeQuiniela(
                    usuario.Insignias,
                    quiniela.Nombre));
        }
    }
}

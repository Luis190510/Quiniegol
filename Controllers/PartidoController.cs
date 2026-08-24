using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>
    /// Administra el calendario y sincroniza sus estados con la fecha simulada.
    /// </summary>
    public class PartidoController
    {
        private const int DuracionPartidoMinutos = 120;

        private readonly JsonRepository<Partido> _partidoRepository;
        private readonly JsonRepository<ResultadoPartido> _resultadoRepository;
        private readonly FechaSimuladaService _fechaService;

        public PartidoController()
        {
            _partidoRepository = new JsonRepository<Partido>(
                RutaDatosService.ObtenerRuta("partidos.json"));
            _resultadoRepository = new JsonRepository<ResultadoPartido>(
                RutaDatosService.ObtenerRuta("resultados2026.json"));
            _fechaService = FechaSimuladaService.Instancia;
        }

        /// <summary>
        /// Obtiene el calendario actualizado y ordenado por fecha.
        /// </summary>
        public List<Partido> ObtenerPartidos()
        {
            List<Partido> partidos = _partidoRepository.ObtenerTodos();
            Dictionary<int, ResultadoPartido> resultados = _resultadoRepository.ObtenerTodos()
                .ToDictionary(resultado => resultado.PartidoId);

            ActualizarEstadosYResultados(partidos, resultados);
            return partidos.OrderBy(partido => partido.FechaHora).ToList();
        }

        /// <summary>
        /// Obtiene los encuentros que todavía no han comenzado.
        /// </summary>
        public List<Partido> ObtenerPartidosPendientes()
        {
            return ObtenerPartidos()
                .Where(partido => _fechaService.FechaActual < partido.FechaHora)
                .ToList();
        }

        /// <summary>
        /// Registra un encuentro nuevo en el calendario.
        /// </summary>
        public void RegistrarPartido(
            int seleccionLocalId,
            int seleccionVisitanteId,
            DateTime fechaHora)
        {
            SesionUsuarioService.ExigirAdministrador();
            ValidarSelecciones(seleccionLocalId, seleccionVisitanteId);

            List<Partido> partidos = _partidoRepository.ObtenerTodos();
            bool partidoRepetido = partidos.Any(partido =>
                partido.FechaHora == fechaHora &&
                partido.SeleccionLocalId == seleccionLocalId &&
                partido.SeleccionVisitanteId == seleccionVisitanteId);
            if (partidoRepetido)
            {
                throw new InvalidOperationException("Ese partido ya se encuentra registrado.");
            }

            partidos.Add(new Partido
            {
                Id = ObtenerSiguienteId(partidos),
                SeleccionLocalId = seleccionLocalId,
                SeleccionVisitanteId = seleccionVisitanteId,
                FechaHora = fechaHora,
                GolesLocal = null,
                GolesVisitante = null,
                Estado = "Pendiente"
            });
            _partidoRepository.GuardarTodos(partidos);
        }

        /// <summary>
        /// Registra o modifica el marcador oficial de un encuentro.
        /// </summary>
        public void GuardarResultado(int partidoId, int golesLocal, int golesVisitante)
        {
            SesionUsuarioService.ExigirAdministrador();
            if (golesLocal < 0 || golesVisitante < 0)
            {
                throw new ArgumentException("Los goles no pueden ser negativos.");
            }

            List<ResultadoPartido> resultados = _resultadoRepository.ObtenerTodos();
            ResultadoPartido? resultado = resultados.FirstOrDefault(
                existente => existente.PartidoId == partidoId);
            if (resultado == null)
            {
                resultados.Add(new ResultadoPartido
                {
                    PartidoId = partidoId,
                    GolesLocal = golesLocal,
                    GolesVisitante = golesVisitante
                });
            }
            else
            {
                resultado.GolesLocal = golesLocal;
                resultado.GolesVisitante = golesVisitante;
            }

            _resultadoRepository.GuardarTodos(resultados);
        }

        /// <summary>
        /// Elimina un encuentro y cualquier resultado asociado.
        /// </summary>
        public void EliminarPartido(int partidoId)
        {
            SesionUsuarioService.ExigirAdministrador();

            List<Partido> partidos = _partidoRepository.ObtenerTodos();
            Partido partido = partidos.FirstOrDefault(actual => actual.Id == partidoId)
                ?? throw new InvalidOperationException(
                    "No se encontró el partido seleccionado.");
            partidos.Remove(partido);
            _partidoRepository.GuardarTodos(partidos);

            List<ResultadoPartido> resultados = _resultadoRepository.ObtenerTodos();
            if (resultados.RemoveAll(resultado => resultado.PartidoId == partidoId) > 0)
            {
                _resultadoRepository.GuardarTodos(resultados);
            }
        }

        private void ActualizarEstadosYResultados(
            List<Partido> partidos,
            IReadOnlyDictionary<int, ResultadoPartido> resultados)
        {
            bool huboCambios = false;
            foreach (Partido partido in partidos)
            {
                (string estado, int? golesLocal, int? golesVisitante) =
                    CalcularEstado(partido, resultados);
                if (partido.Estado == estado &&
                    partido.GolesLocal == golesLocal &&
                    partido.GolesVisitante == golesVisitante)
                {
                    continue;
                }

                partido.Estado = estado;
                partido.GolesLocal = golesLocal;
                partido.GolesVisitante = golesVisitante;
                huboCambios = true;
            }

            if (huboCambios)
            {
                _partidoRepository.GuardarTodos(partidos);
            }
        }

        private (string Estado, int? GolesLocal, int? GolesVisitante) CalcularEstado(
            Partido partido,
            IReadOnlyDictionary<int, ResultadoPartido> resultados)
        {
            if (_fechaService.FechaActual < partido.FechaHora)
            {
                return ("Pendiente", null, null);
            }

            if (_fechaService.FechaActual <
                partido.FechaHora.AddMinutes(DuracionPartidoMinutos))
            {
                return ("En curso", null, null);
            }

            return resultados.TryGetValue(partido.Id, out ResultadoPartido? resultado)
                ? ("Finalizado", resultado.GolesLocal, resultado.GolesVisitante)
                : ("Pendiente de resultado", null, null);
        }

        private static void ValidarSelecciones(int seleccionLocalId, int seleccionVisitanteId)
        {
            if (seleccionLocalId <= 0)
            {
                throw new ArgumentException("Debe seleccionar el equipo local.");
            }

            if (seleccionVisitanteId <= 0)
            {
                throw new ArgumentException("Debe seleccionar el equipo visitante.");
            }

            if (seleccionLocalId == seleccionVisitanteId)
            {
                throw new InvalidOperationException(
                    "Una selección no puede jugar contra sí misma.");
            }
        }

        private static int ObtenerSiguienteId(List<Partido> partidos)
        {
            return partidos.Count == 0 ? 1 : partidos.Max(partido => partido.Id) + 1;
        }
    }
}

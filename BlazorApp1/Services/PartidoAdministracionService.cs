using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>Permite al administrador actualizar el calendario y los resultados.</summary>
    public sealed class PartidoAdministracionService
    {
        private readonly JsonRepository<Partido> _partidos;
        private readonly JsonRepository<ResultadoPartido> _resultados;
        private readonly SesionUsuarioService _sesion;
        private readonly FechaSimuladaService _fechaSimulada;

        public PartidoAdministracionService(
            SesionUsuarioService sesion,
            FechaSimuladaService fechaSimulada)
        {
            _sesion = sesion;
            _fechaSimulada = fechaSimulada;
            _partidos = new JsonRepository<Partido>(
                RutaDatosService.ObtenerRuta("partidos.json"));
            _resultados = new JsonRepository<ResultadoPartido>(
                RutaDatosService.ObtenerRuta("resultados2026.json"));
        }

        public List<Partido> ObtenerPartidos()
        {
            _sesion.ExigirAdministrador();
            return _partidos.ObtenerTodos()
                .OrderBy(partido => partido.FechaHora)
                .ToList();
        }

        public void ActualizarPartido(
            int partidoId,
            int seleccionLocalId,
            int seleccionVisitanteId,
            DateTime fechaHora,
            int? golesLocal,
            int? golesVisitante)
        {
            _sesion.ExigirAdministrador();

            if (seleccionLocalId <= 0 || seleccionVisitanteId <= 0)
            {
                throw new ArgumentException("Debe seleccionar ambos equipos.");
            }

            if (seleccionLocalId == seleccionVisitanteId)
            {
                throw new ArgumentException(
                    "Una selección no puede jugar contra sí misma.");
            }

            if (golesLocal.HasValue != golesVisitante.HasValue)
            {
                throw new ArgumentException(
                    "Debe indicar ambos marcadores o dejar ambos vacíos.");
            }

            if (golesLocal < 0 || golesVisitante < 0)
            {
                throw new ArgumentException("Los goles no pueden ser negativos.");
            }

            List<Partido> partidos = _partidos.ObtenerTodos();
            Partido partido = partidos.FirstOrDefault(elemento => elemento.Id == partidoId)
                ?? throw new InvalidOperationException(
                    "No se encontró el partido seleccionado.");

            partido.SeleccionLocalId = seleccionLocalId;
            partido.SeleccionVisitanteId = seleccionVisitanteId;
            partido.FechaHora = fechaHora;
            partido.GolesLocal = golesLocal;
            partido.GolesVisitante = golesVisitante;
            partido.Estado = golesLocal.HasValue && fechaHora <= _fechaSimulada.FechaActual
                ? "Finalizado"
                : "Pendiente";
            _partidos.GuardarTodos(partidos);

            List<ResultadoPartido> resultados = _resultados.ObtenerTodos();
            ResultadoPartido? resultado = resultados.FirstOrDefault(
                elemento => elemento.PartidoId == partidoId);

            if (!golesLocal.HasValue)
            {
                if (resultado is not null)
                {
                    resultados.Remove(resultado);
                    _resultados.GuardarTodos(resultados);
                }
                return;
            }

            if (resultado is null)
            {
                resultados.Add(new ResultadoPartido { PartidoId = partidoId });
                resultado = resultados[^1];
            }

            resultado.GolesLocal = golesLocal.Value;
            resultado.GolesVisitante = golesVisitante!.Value;
            _resultados.GuardarTodos(resultados);
        }
    }
}

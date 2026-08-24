using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>Calcula las posiciones de la fase de grupos.</summary>
    public class TablaPosicionesService
    {
        private const int UltimoPartidoFaseGrupos = 72;

        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly PartidoController _partidoController;

        public TablaPosicionesService()
        {
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _partidoController = new PartidoController();
        }

        public List<string> ObtenerGrupos()
        {
            return _seleccionRepository.ObtenerTodos()
                .Select(seleccion => seleccion.Grupo)
                .Where(grupo => !string.IsNullOrWhiteSpace(grupo))
                .Distinct()
                .OrderBy(grupo => grupo)
                .ToList();
        }

        public List<PosicionGrupoItem> CalcularTabla(string grupo)
        {
            if (string.IsNullOrWhiteSpace(grupo))
            {
                throw new ArgumentException("Debe seleccionar un grupo.");
            }

            List<Seleccion> selecciones = _seleccionRepository.ObtenerTodos()
                .Where(seleccion => string.Equals(
                    seleccion.Grupo,
                    grupo,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (selecciones.Count == 0)
            {
                throw new InvalidOperationException(
                    "No se encontraron selecciones para ese grupo.");
            }

            Dictionary<int, PosicionGrupoItem> tabla = selecciones.ToDictionary(
                seleccion => seleccion.Id,
                seleccion => new PosicionGrupoItem
                {
                    SeleccionId = seleccion.Id,
                    Seleccion = seleccion.Nombre,
                    Grupo = seleccion.Grupo
                });

            foreach (Partido partido in ObtenerPartidosDelGrupo(tabla.Keys.ToHashSet()))
            {
                ActualizarPosiciones(
                    tabla[partido.SeleccionLocalId],
                    tabla[partido.SeleccionVisitanteId],
                    partido.GolesLocal!.Value,
                    partido.GolesVisitante!.Value);
            }

            List<PosicionGrupoItem> posiciones = tabla.Values
                .OrderByDescending(fila => fila.Puntos)
                .ThenByDescending(fila => fila.DiferenciaGoles)
                .ThenByDescending(fila => fila.GolesFavor)
                .ThenBy(fila => fila.Seleccion)
                .ToList();

            for (int indice = 0; indice < posiciones.Count; indice++)
            {
                posiciones[indice].Posicion = indice + 1;
            }

            return posiciones;
        }

        private IEnumerable<Partido> ObtenerPartidosDelGrupo(HashSet<int> seleccionesIds)
        {
            return _partidoController.ObtenerPartidos().Where(partido =>
                partido.Id <= UltimoPartidoFaseGrupos &&
                partido.Estado == "Finalizado" &&
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue &&
                seleccionesIds.Contains(partido.SeleccionLocalId) &&
                seleccionesIds.Contains(partido.SeleccionVisitanteId));
        }

        private static void ActualizarPosiciones(
            PosicionGrupoItem local,
            PosicionGrupoItem visitante,
            int golesLocal,
            int golesVisitante)
        {
            local.PartidosJugados++;
            visitante.PartidosJugados++;
            local.GolesFavor += golesLocal;
            local.GolesContra += golesVisitante;
            visitante.GolesFavor += golesVisitante;
            visitante.GolesContra += golesLocal;

            if (golesLocal > golesVisitante)
            {
                local.PartidosGanados++;
                visitante.PartidosPerdidos++;
                local.Puntos += 3;
            }
            else if (golesVisitante > golesLocal)
            {
                visitante.PartidosGanados++;
                local.PartidosPerdidos++;
                visitante.Puntos += 3;
            }
            else
            {
                local.PartidosEmpatados++;
                visitante.PartidosEmpatados++;
                local.Puntos++;
                visitante.Puntos++;
            }
        }
    }
}

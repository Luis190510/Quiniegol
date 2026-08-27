using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>
    /// Lee las selecciones y los partidos del Mundial 2026 desde los archivos JSON.
    /// </summary>
    public sealed class DatosMundialService
    {
        private readonly JsonRepository<Seleccion> _selecciones;
        private readonly JsonRepository<Partido> _partidos;
        private readonly JsonRepository<Pronostico> _pronosticos;
        private readonly JsonRepository<Usuario> _usuarios;
        private readonly JsonRepository<Quiniela> _quinielas;

        public DatosMundialService()
        {
            _selecciones = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _partidos = new JsonRepository<Partido>(
                RutaDatosService.ObtenerRuta("partidos.json"));
            _pronosticos = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json"));
            _usuarios = new JsonRepository<Usuario>(
                RutaDatosService.ObtenerRuta("usuarios.json"));
            _quinielas = new JsonRepository<Quiniela>(
                RutaDatosService.ObtenerRuta("quinielas.json"));
        }

        public List<Seleccion> ObtenerSelecciones()
        {
            return _selecciones.ObtenerTodos()
                .OrderBy(seleccion => seleccion.Id)
                .ToList();
        }

        public List<PartidoMundialItem> ObtenerPartidos(DateTime fechaSimulada)
        {
            Dictionary<int, Seleccion> selecciones = ObtenerSelecciones()
                .ToDictionary(seleccion => seleccion.Id);

            return _partidos.ObtenerTodos()
                .OrderBy(partido => partido.FechaHora)
                .Select(partido => Convertir(partido, selecciones, fechaSimulada))
                .ToList();
        }

        public List<EstadisticaSeleccionMundialItem> ObtenerEstadisticas(
            DateTime fechaSimulada,
            bool soloFaseGrupos = false)
        {
            List<Seleccion> selecciones = ObtenerSelecciones();
            Dictionary<int, EstadisticaSeleccionMundialItem> tabla = selecciones
                .ToDictionary(
                    seleccion => seleccion.Id,
                    seleccion => new EstadisticaSeleccionMundialItem
                    {
                        SeleccionId = seleccion.Id,
                        Seleccion = seleccion.Nombre,
                        Grupo = seleccion.Grupo
                    });

            IEnumerable<PartidoMundialItem> partidos = ObtenerPartidos(fechaSimulada)
                .Where(partido => partido.Finalizado);

            if (soloFaseGrupos)
            {
                partidos = partidos.Where(partido => partido.Id <= 72);
            }

            foreach (PartidoMundialItem partido in partidos)
            {
                if (!partido.GolesLocal.HasValue || !partido.GolesVisitante.HasValue)
                {
                    continue;
                }

                EstadisticaSeleccionMundialItem local = tabla[partido.SeleccionLocalId];
                EstadisticaSeleccionMundialItem visitante = tabla[partido.SeleccionVisitanteId];
                local.PartidosJugados++;
                visitante.PartidosJugados++;
                local.GolesFavor += partido.GolesLocal.Value;
                local.GolesContra += partido.GolesVisitante.Value;
                visitante.GolesFavor += partido.GolesVisitante.Value;
                visitante.GolesContra += partido.GolesLocal.Value;

                if (partido.GolesLocal > partido.GolesVisitante)
                {
                    local.Victorias++;
                    visitante.Derrotas++;
                }
                else if (partido.GolesVisitante > partido.GolesLocal)
                {
                    visitante.Victorias++;
                    local.Derrotas++;
                }
                else
                {
                    local.Empates++;
                    visitante.Empates++;
                }
            }

            return tabla.Values.ToList();
        }

        public List<PartidoMundialItem> ObtenerProximosSinPronostico(
            int usuarioId,
            DateTime fechaSimulada)
        {
            HashSet<int> pronosticados = _pronosticos.ObtenerTodos()
                .Where(pronostico => pronostico.UsuarioId == usuarioId)
                .Select(pronostico => pronostico.PartidoId)
                .ToHashSet();
            DateTime limite = fechaSimulada.AddHours(24);

            return ObtenerPartidos(fechaSimulada)
                .Where(partido =>
                    partido.Fecha >= fechaSimulada &&
                    partido.Fecha <= limite &&
                    !pronosticados.Contains(partido.Id))
                .ToList();
        }

        public List<Pronostico> ObtenerPronosticos(int usuarioId)
        {
            return _pronosticos.ObtenerTodos()
                .Where(pronostico => pronostico.UsuarioId == usuarioId)
                .OrderByDescending(pronostico => pronostico.FechaRegistro)
                .ToList();
        }

        public int ObtenerPosicionUsuario(int usuarioId)
        {
            RankingItem? fila = RankingService
                .Crear(_usuarios.ObtenerTodos().Where(usuario => usuario.Activo),
                    usuario => usuario.Insignias)
                .FirstOrDefault(elemento => elemento.UsuarioId == usuarioId);

            return fila?.Posicion ?? 0;
        }

        public int ObtenerCantidadQuinielas(int usuarioId)
        {
            return _quinielas.ObtenerTodos()
                .Count(quiniela => quiniela.IntegrantesIds.Contains(usuarioId));
        }

        private static PartidoMundialItem Convertir(
            Partido partido,
            IReadOnlyDictionary<int, Seleccion> selecciones,
            DateTime fechaSimulada)
        {
            bool finalizado = partido.FechaHora <= fechaSimulada &&
                partido.GolesLocal.HasValue && partido.GolesVisitante.HasValue;
            string fase = ObtenerFase(partido.Id);
            string grupo = fase == "Fase de grupos" &&
                selecciones.TryGetValue(partido.SeleccionLocalId, out Seleccion? local)
                    ? $"Grupo {local.Grupo}"
                    : fase;

            return new PartidoMundialItem
            {
                Id = partido.Id,
                SeleccionLocalId = partido.SeleccionLocalId,
                SeleccionVisitanteId = partido.SeleccionVisitanteId,
                EquipoLocal = ObtenerNombre(selecciones, partido.SeleccionLocalId),
                EquipoVisitante = ObtenerNombre(selecciones, partido.SeleccionVisitanteId),
                Fecha = partido.FechaHora,
                Fase = fase,
                Grupo = grupo,
                Estado = finalizado ? "Finalizado" : "Próximo",
                Finalizado = finalizado,
                GolesLocal = finalizado ? partido.GolesLocal : null,
                GolesVisitante = finalizado ? partido.GolesVisitante : null
            };
        }

        private static string ObtenerNombre(
            IReadOnlyDictionary<int, Seleccion> selecciones,
            int seleccionId)
        {
            return selecciones.TryGetValue(seleccionId, out Seleccion? seleccion)
                ? seleccion.Nombre
                : $"Selección {seleccionId}";
        }

        private static string ObtenerFase(int partidoId)
        {
            return partidoId switch
            {
                <= 72 => "Fase de grupos",
                <= 88 => "Ronda de 32",
                <= 96 => "Octavos de final",
                <= 100 => "Cuartos de final",
                <= 102 => "Semifinal",
                103 => "Tercer lugar",
                _ => "Final"
            };
        }
    }
}

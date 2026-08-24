using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>
    /// Calcula las estadísticas de pronósticos y resultados para un rango de fechas.
    /// </summary>
    public class EstadisticasService
    {
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Usuario> _usuarioRepository;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly PartidoController _partidoController;
        private readonly PuntajeController _puntajeController;

        public EstadisticasService()
        {
            _pronosticoRepository = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json"));
            _usuarioRepository = new JsonRepository<Usuario>(
                RutaDatosService.ObtenerRuta("usuarios.json"));
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _partidoController = new PartidoController();
            _puntajeController = new PuntajeController();
        }

        /// <summary>
        /// Obtiene las estadísticas de los partidos programados dentro del rango indicado.
        /// </summary>
        public List<EstadisticaItem> ObtenerEstadisticas(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            DateTime inicio = fechaDesde.Date;
            DateTime final = fechaHasta.Date.AddDays(1).AddTicks(-1);
            if (inicio > final)
            {
                throw new ArgumentException(
                    "La fecha inicial no puede ser mayor que la fecha final.");
            }

            _puntajeController.CalcularTodosLosPuntajes();

            List<Partido> partidos = _partidoController.ObtenerPartidos();
            List<Partido> partidosRango = partidos
                .Where(partido => partido.FechaHora >= inicio && partido.FechaHora <= final)
                .ToList();
            List<Pronostico> pronosticos = FiltrarPronosticosDePartidos(
                _pronosticoRepository.ObtenerTodos(),
                partidosRango);

            List<Seleccion> selecciones = _seleccionRepository.ObtenerTodos();
            Dictionary<int, Partido> partidosPorId = partidos.ToDictionary(partido => partido.Id);
            Dictionary<int, string> seleccionesPorId = selecciones
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);
            Dictionary<int, string> usuariosPorId = _usuarioRepository.ObtenerTodos()
                .ToDictionary(usuario => usuario.Id, usuario => usuario.Nombre);

            return new List<EstadisticaItem>
            {
                CrearItem("Equipo más apostado como ganador",
                    ObtenerEquipoMasApostado(pronosticos, partidosPorId, seleccionesPorId)),
                CrearItem("Marcador más repetido",
                    ObtenerMarcadorMasRepetido(pronosticos)),
                CrearItem("Partido con más aciertos",
                    ObtenerPartidoConMasAciertos(
                        pronosticos, partidosPorId, seleccionesPorId)),
                CrearItem("Usuario con más aciertos",
                    ObtenerUsuarioConMasAciertos(pronosticos, usuariosPorId)),
                CrearItem("Partido con más pronósticos",
                    ObtenerPartidoConMasPronosticos(
                        pronosticos, partidosPorId, seleccionesPorId)),
                CrearItem("Equipo sorpresa",
                    ObtenerEquipoSorpresa(pronosticos, partidosRango, seleccionesPorId)),
                CrearItem("Equipo(s) con más goles",
                    EstadisticasGolesService.ObtenerConMasGoles(
                        partidosRango, selecciones)),
                CrearItem("Equipo(s) con menos goles",
                    EstadisticasGolesService.ObtenerConMenosGoles(
                        partidosRango, selecciones)),
                CrearItem("Promedio de goles por partido", ObtenerPromedioGoles(partidosRango))
            };
        }

        /// <summary>
        /// Conserva los pronósticos de los partidos incluidos en el rango. La fecha
        /// relevante es la del encuentro, aunque el pronóstico se registrara antes.
        /// </summary>
        public static List<Pronostico> FiltrarPronosticosDePartidos(
            IEnumerable<Pronostico> pronosticos,
            IEnumerable<Partido> partidosRango)
        {
            HashSet<int> partidosIds = partidosRango
                .Select(partido => partido.Id)
                .ToHashSet();
            return pronosticos
                .Where(pronostico => partidosIds.Contains(pronostico.PartidoId))
                .ToList();
        }

        private static EstadisticaItem CrearItem(string nombre, string resultado)
        {
            return new EstadisticaItem { Estadistica = nombre, Resultado = resultado };
        }

        private static string ObtenerEquipoMasApostado(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            var grupo = pronosticos
                .Select(pronostico => partidos.TryGetValue(pronostico.PartidoId, out Partido? partido)
                    ? ObtenerGanadorPronosticado(pronostico, partido)
                    : null)
                .Where(seleccionId => seleccionId.HasValue)
                .GroupBy(seleccionId => seleccionId!.Value)
                .OrderByDescending(actual => actual.Count())
                .FirstOrDefault();

            return grupo == null
                ? "Sin datos"
                : $"{ObtenerNombreSeleccion(selecciones, grupo.Key)} ({grupo.Count()} apuestas)";
        }

        private static string ObtenerMarcadorMasRepetido(IEnumerable<Pronostico> pronosticos)
        {
            var grupo = pronosticos
                .GroupBy(pronostico => (
                    pronostico.GolesLocalPronosticados,
                    pronostico.GolesVisitantePronosticados))
                .OrderByDescending(actual => actual.Count())
                .FirstOrDefault();

            return grupo == null
                ? "Sin datos"
                : $"{grupo.Key.GolesLocalPronosticados} - " +
                  $"{grupo.Key.GolesVisitantePronosticados} ({grupo.Count()} veces)";
        }

        private static string ObtenerPartidoConMasAciertos(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            var grupo = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos > 0)
                .GroupBy(pronostico => pronostico.PartidoId)
                .OrderByDescending(actual => actual.Count())
                .FirstOrDefault();

            return grupo == null
                ? "Sin datos"
                : $"{ObtenerNombrePartido(partidos, selecciones, grupo.Key)} " +
                  $"({grupo.Count()} aciertos)";
        }

        private static string ObtenerUsuarioConMasAciertos(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, string> usuarios)
        {
            var grupo = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos > 0)
                .GroupBy(pronostico => pronostico.UsuarioId)
                .OrderByDescending(actual => actual.Count())
                .FirstOrDefault();

            if (grupo == null)
            {
                return "Sin datos";
            }

            string usuario = usuarios.GetValueOrDefault(grupo.Key, "Usuario desconocido");
            return $"{usuario} ({grupo.Count()} aciertos)";
        }

        private static string ObtenerPartidoConMasPronosticos(
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            var grupo = pronosticos
                .GroupBy(pronostico => pronostico.PartidoId)
                .OrderByDescending(actual => actual.Count())
                .FirstOrDefault();

            return grupo == null
                ? "Sin datos"
                : $"{ObtenerNombrePartido(partidos, selecciones, grupo.Key)} " +
                  $"({grupo.Count()} pronósticos)";
        }

        private static string ObtenerPromedioGoles(IEnumerable<Partido> partidos)
        {
            List<Partido> finalizados = partidos.Where(PartidoTieneResultado).ToList();
            if (finalizados.Count == 0)
            {
                return "Sin partidos finalizados";
            }

            double totalGoles = finalizados.Sum(
                partido => partido.GolesLocal!.Value + partido.GolesVisitante!.Value);
            return (totalGoles / finalizados.Count).ToString("0.00");
        }

        private static string ObtenerEquipoSorpresa(
            IEnumerable<Pronostico> pronosticos,
            IEnumerable<Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones)
        {
            Dictionary<int, List<Pronostico>> pronosticosPorPartido = pronosticos
                .GroupBy(pronostico => pronostico.PartidoId)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList());

            int? mejorEquipoId = null;
            double mayorPorcentaje = 0;
            foreach (Partido partido in partidos.Where(PartidoTieneResultado))
            {
                (int? ganadorRealId, int? rivalId) = ObtenerGanadorYRival(partido);
                if (!ganadorRealId.HasValue ||
                    !rivalId.HasValue ||
                    !pronosticosPorPartido.TryGetValue(partido.Id, out List<Pronostico>? apuestas))
                {
                    continue;
                }

                int apuestasPorRival = apuestas.Count(
                    pronostico => ObtenerGanadorPronosticado(pronostico, partido) == rivalId);
                double porcentaje = (double)apuestasPorRival / apuestas.Count;
                if (porcentaje >= 0.60 && porcentaje > mayorPorcentaje)
                {
                    mayorPorcentaje = porcentaje;
                    mejorEquipoId = ganadorRealId;
                }
            }

            return mejorEquipoId.HasValue
                ? $"{ObtenerNombreSeleccion(selecciones, mejorEquipoId.Value)} " +
                  $"({mayorPorcentaje:P0} apostó en su contra)"
                : "No se encontró un equipo sorpresa";
        }

        private static bool PartidoTieneResultado(Partido partido)
        {
            return partido.Estado == "Finalizado" &&
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue;
        }

        private static (int? GanadorId, int? RivalId) ObtenerGanadorYRival(Partido partido)
        {
            if (partido.GolesLocal > partido.GolesVisitante)
            {
                return (partido.SeleccionLocalId, partido.SeleccionVisitanteId);
            }

            if (partido.GolesVisitante > partido.GolesLocal)
            {
                return (partido.SeleccionVisitanteId, partido.SeleccionLocalId);
            }

            return (null, null);
        }

        private static int? ObtenerGanadorPronosticado(Pronostico pronostico, Partido partido)
        {
            if (pronostico.GolesLocalPronosticados > pronostico.GolesVisitantePronosticados)
            {
                return partido.SeleccionLocalId;
            }

            if (pronostico.GolesVisitantePronosticados > pronostico.GolesLocalPronosticados)
            {
                return partido.SeleccionVisitanteId;
            }

            return null;
        }

        private static string ObtenerNombrePartido(
            IReadOnlyDictionary<int, Partido> partidos,
            IReadOnlyDictionary<int, string> selecciones,
            int partidoId)
        {
            if (!partidos.TryGetValue(partidoId, out Partido? partido))
            {
                return "Partido desconocido";
            }

            string local = ObtenerNombreSeleccion(selecciones, partido.SeleccionLocalId);
            string visitante = ObtenerNombreSeleccion(selecciones, partido.SeleccionVisitanteId);
            return $"{local} vs {visitante}";
        }

        private static string ObtenerNombreSeleccion(
            IReadOnlyDictionary<int, string> selecciones,
            int seleccionId)
        {
            return selecciones.GetValueOrDefault(seleccionId, $"Selección {seleccionId}");
        }
    }
}

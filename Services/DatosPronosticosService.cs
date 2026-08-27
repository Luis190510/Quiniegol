using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>
    /// Completa una sola vez los pronósticos de demostración creados en la Parte 1.
    /// </summary>
    public class DatosPronosticosService
    {
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Partido> _partidoRepository;
        private readonly JsonRepository<GoleadorReal> _goleadorRepository;

        public DatosPronosticosService()
            : this(
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta("pronosticos.json")),
                new JsonRepository<Partido>(
                    RutaDatosService.ObtenerRuta("partidos.json")),
                new JsonRepository<GoleadorReal>(
                    RutaDatosService.ObtenerRuta("goleadores2026.json")))
        {
        }

        public DatosPronosticosService(
            JsonRepository<Pronostico> pronosticoRepository,
            JsonRepository<Partido> partidoRepository,
            JsonRepository<GoleadorReal> goleadorRepository)
        {
            _pronosticoRepository = pronosticoRepository;
            _partidoRepository = partidoRepository;
            _goleadorRepository = goleadorRepository;
        }

        /// <summary>
        /// Completa los partidos que faltan en los pronósticos de demostración.
        /// Solo utiliza los usuarios que ya tenían los doce encuentros de la
        /// muestra original, por lo que no crea información en nombre de
        /// cuentas registradas después.
        /// </summary>
        /// <returns>Cantidad de pronósticos de demostración agregados.</returns>
        public int CompletarCoberturaDelTorneo()
        {
            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            List<Partido> partidos =
                _partidoRepository.ObtenerTodos()
                    .OrderBy(partido => partido.FechaHora)
                    .ToList();

            HashSet<int> partidosMuestraIds =
                partidos
                    .Take(12)
                    .Select(partido => partido.Id)
                    .ToHashSet();

            List<int> usuariosDemostracion =
                pronosticos
                    .Where(pronostico => pronostico.UsuarioId > 0)
                    .GroupBy(pronostico => pronostico.UsuarioId)
                    .Where(grupo =>
                        partidosMuestraIds.Count == 12 &&
                        partidosMuestraIds.All(partidoId =>
                            grupo.Any(pronostico =>
                                pronostico.PartidoId == partidoId)))
                    .Select(grupo => grupo.Key)
                    .OrderBy(usuarioId => usuarioId)
                    .ToList();

            HashSet<(int UsuarioId, int PartidoId)> existentes =
                pronosticos
                    .Select(pronostico =>
                        (pronostico.UsuarioId, pronostico.PartidoId))
                    .ToHashSet();

            int siguienteId = pronosticos.Count == 0
                ? 1
                : pronosticos.Max(pronostico => pronostico.Id) + 1;

            int agregados = 0;

            foreach (int usuarioId in usuariosDemostracion)
            {
                foreach (Partido partido in partidos)
                {
                    if (existentes.Contains((usuarioId, partido.Id)))
                    {
                        continue;
                    }

                    Pronostico nuevoPronostico = new()
                    {
                        Id = siguienteId++,
                        UsuarioId = usuarioId,
                        PartidoId = partido.Id,
                        GolesLocalPronosticados =
                            (usuarioId * 7 + partido.Id * 3) % 4,
                        GolesVisitantePronosticados =
                            (usuarioId * 5 + partido.Id * 2 + 1) % 4,
                        FechaRegistro = partido.FechaHora
                            .AddDays(-2)
                            .AddMinutes(
                                (usuarioId * 37 + partido.Id * 11) % 600),
                        PuntosObtenidos = null,
                        GoleadoresConfirmados = false
                    };

                    pronosticos.Add(nuevoPronostico);
                    existentes.Add((usuarioId, partido.Id));
                    agregados++;
                }
            }

            if (agregados > 0)
            {
                _pronosticoRepository.GuardarTodos(pronosticos);
            }

            return agregados;
        }

        /// <summary>
        /// Agrega candidatos reales y deterministas a los registros históricos.
        /// Los pronósticos nuevos nunca son modificados por esta migración.
        /// </summary>
        /// <returns>Cantidad de pronósticos históricos actualizados.</returns>
        public int CompletarGoleadoresHistoricos()
        {
            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();
            Dictionary<int, Partido> partidos =
                _partidoRepository.ObtenerTodos()
                    .ToDictionary(partido => partido.Id);
            Dictionary<int, List<string>> candidatos =
                _goleadorRepository.ObtenerTodos()
                    .GroupBy(goleador => goleador.SeleccionId)
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo
                            .Select(goleador => LimpiarTipoDeGol(goleador.Jugador))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .OrderBy(nombre => nombre)
                            .ToList());

            int actualizados = 0;

            foreach (Pronostico pronostico in pronosticos
                         .Where(elemento => !elemento.GoleadoresConfirmados))
            {
                pronostico.GoleadoresLocalPronosticados ??= new List<string>();
                pronostico.GoleadoresVisitantePronosticados ??= new List<string>();

                if (partidos.TryGetValue(pronostico.PartidoId, out Partido? partido))
                {
                    AgregarCandidatoSiCorresponde(
                        pronostico.GolesLocalPronosticados,
                        partido.SeleccionLocalId,
                        pronostico.Id,
                        candidatos,
                        pronostico.GoleadoresLocalPronosticados);

                    AgregarCandidatoSiCorresponde(
                        pronostico.GolesVisitantePronosticados,
                        partido.SeleccionVisitanteId,
                        pronostico.Id + 1,
                        candidatos,
                        pronostico.GoleadoresVisitantePronosticados);
                }

                pronostico.GoleadoresConfirmados = true;
                actualizados++;
            }

            if (actualizados > 0)
            {
                _pronosticoRepository.GuardarTodos(pronosticos);
            }

            return actualizados;
        }

        private static void AgregarCandidatoSiCorresponde(
            int golesPronosticados,
            int seleccionId,
            int semilla,
            Dictionary<int, List<string>> candidatos,
            List<string> destino)
        {
            if (golesPronosticados <= 0 || destino.Count > 0 ||
                !candidatos.TryGetValue(seleccionId, out List<string>? nombres) ||
                nombres.Count == 0)
            {
                return;
            }

            destino.Add(nombres[Math.Abs(semilla) % nombres.Count]);
        }

        private static string LimpiarTipoDeGol(string jugador)
        {
            return (jugador ?? string.Empty)
                .Replace(" (autogol)", string.Empty)
                .Replace(" (penal)", string.Empty)
                .Trim();
        }
    }
}

using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    /// <summary>
    /// Administra las insignias automáticas obtenidas por los participantes.
    /// </summary>
    public class InsigniaService
    {
        private static readonly HashSet<string> InsigniasGlobales = new()
        {
            "Líder global",
            "Peor del ranking global",
            "Rey de los empates",
            "Racha de 10 aciertos",
            "Precisión goleadora",
            "Cazagoleadores"
        };

        private readonly JsonRepository<Usuario> _usuarioRepository;
        private readonly JsonRepository<Pronostico> _pronosticoRepository;
        private readonly JsonRepository<Quiniela> _quinielaRepository;
        private readonly JsonRepository<GoleadorReal> _goleadorRepository;
        private readonly PartidoController _partidoController;
        private readonly PuntajeController _puntajeController;

        public InsigniaService()
        {
            _usuarioRepository = new JsonRepository<Usuario>(
                RutaDatosService.ObtenerRuta("usuarios.json"));
            _pronosticoRepository = new JsonRepository<Pronostico>(
                RutaDatosService.ObtenerRuta("pronosticos.json"));
            _quinielaRepository = new JsonRepository<Quiniela>(
                RutaDatosService.ObtenerRuta("quinielas.json"));
            _goleadorRepository = new JsonRepository<GoleadorReal>(
                RutaDatosService.ObtenerRuta("goleadores2026.json"));
            _partidoController = new PartidoController();
            _puntajeController = new PuntajeController();
        }

        /// <summary>
        /// Devuelve el catálogo de insignias globales disponibles.
        /// </summary>
        public static List<Insignia> ObtenerCatalogo()
        {
            return new List<Insignia>
            {
                CrearInsignia("Líder global", "Usuario con mayor puntaje global.", "Positiva"),
                CrearInsignia(
                    "Rey de los empates", "Usuario con más empates acertados.", "Positiva"),
                CrearInsignia(
                    "Racha de 10 aciertos",
                    "Usuario con al menos 10 aciertos consecutivos.",
                    "Positiva"),
                CrearInsignia(
                    "Precisión goleadora",
                    "Usuario con más cantidades de goles exactas acertadas por equipo.",
                    "Positiva"),
                CrearInsignia(
                    "Cazagoleadores",
                    "Usuario con más jugadores goleadores acertados.",
                    "Positiva"),
                CrearInsignia(
                    "Peor del ranking global", "Usuario con menor puntaje global.", "Vergüenza")
            };
        }

        /// <summary>
        /// Recalcula todas las insignias automáticas a partir de los datos actuales.
        /// </summary>
        public void RecalcularInsignias()
        {
            _puntajeController.CalcularTodosLosPuntajes();

            List<Usuario> usuarios = _usuarioRepository.ObtenerTodos();
            List<Usuario> participantes = usuarios
                .Where(usuario => usuario.Rol == RolUsuario.Usuario)
                .ToList();
            List<Pronostico> pronosticos = _pronosticoRepository.ObtenerTodos();
            List<Quiniela> quinielas = _quinielaRepository.ObtenerTodos();
            List<GoleadorReal> goleadores = _goleadorRepository.ObtenerTodos();
            Dictionary<int, Partido> partidosPorId = _partidoController.ObtenerPartidos()
                .ToDictionary(partido => partido.Id);
            Dictionary<int, Usuario> participantesPorId = participantes
                .ToDictionary(usuario => usuario.Id);
            HashSet<int> participantesConResultados = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos.HasValue)
                .Select(pronostico => pronostico.UsuarioId)
                .ToHashSet();
            List<Usuario> participantesEvaluados = participantes
                .Where(usuario => participantesConResultados.Contains(usuario.Id))
                .ToList();

            foreach (Usuario usuario in usuarios)
            {
                usuario.Insignias ??= new List<string>();
                usuario.Insignias.RemoveAll(EsInsigniaAutomatica);
            }

            AsignarExtremoGlobal(participantesEvaluados, esMayor: true);
            AsignarExtremoGlobal(participantesEvaluados, esMayor: false);
            AsignarReyDeLosEmpates(participantesPorId, pronosticos, partidosPorId);
            AsignarRachas(participantes, pronosticos, partidosPorId);
            Dictionary<int, int> golesExactos =
                MetricasInsigniasService.ContarGolesExactos(
                    pronosticos,
                    partidosPorId);
            Dictionary<int, int> goleadoresAcertados =
                MetricasInsigniasService.ContarGoleadoresAcertados(
                    pronosticos,
                    partidosPorId,
                    goleadores);
            AsignarGanadoresDeMetrica(
                participantesPorId,
                golesExactos,
                "Precisión goleadora");
            AsignarGanadoresDeMetrica(
                participantesPorId,
                goleadoresAcertados,
                "Cazagoleadores");
            AsignarInsigniasPorQuiniela(
                participantesPorId,
                quinielas,
                participantesConResultados,
                golesExactos,
                goleadoresAcertados);

            _usuarioRepository.GuardarTodos(usuarios);
        }

        /// <summary>Obtiene las insignias actuales de un participante.</summary>
        public List<string> ObtenerInsigniasDeUsuario(int usuarioId)
        {
            return _usuarioRepository.ObtenerTodos()
                .FirstOrDefault(usuario => usuario.Id == usuarioId)?
                .Insignias?
                .ToList() ?? new List<string>();
        }

        private static Insignia CrearInsignia(string nombre, string descripcion, string tipo)
        {
            return new Insignia { Nombre = nombre, Descripcion = descripcion, Tipo = tipo };
        }

        private static bool EsInsigniaAutomatica(string insignia)
        {
            return InsigniasGlobales.Contains(insignia) ||
                insignia.StartsWith("Líder de quiniela:") ||
                insignia.StartsWith("Peor de quiniela:") ||
                insignia.StartsWith("Precisión goleadora de quiniela:") ||
                insignia.StartsWith("Cazagoleadores de quiniela:");
        }

        private static void AsignarExtremoGlobal(List<Usuario> usuarios, bool esMayor)
        {
            if (usuarios.Count == 0 || (!esMayor && usuarios.Count <= 1))
            {
                return;
            }

            int puntajeObjetivo = esMayor
                ? usuarios.Max(usuario => usuario.Puntos)
                : usuarios.Min(usuario => usuario.Puntos);
            string insignia = esMayor ? "Líder global" : "Peor del ranking global";

            foreach (Usuario usuario in usuarios.Where(
                participante => participante.Puntos == puntajeObjetivo))
            {
                AgregarInsignia(usuario, insignia);
            }
        }

        private static void AsignarReyDeLosEmpates(
            IReadOnlyDictionary<int, Usuario> usuarios,
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos)
        {
            var empatesAcertados = new Dictionary<int, int>();
            foreach (Pronostico pronostico in pronosticos)
            {
                if (!usuarios.ContainsKey(pronostico.UsuarioId) ||
                    !partidos.TryGetValue(pronostico.PartidoId, out Partido? partido) ||
                    !PartidoTieneResultado(partido) ||
                    partido.GolesLocal != partido.GolesVisitante ||
                    pronostico.GolesLocalPronosticados !=
                        pronostico.GolesVisitantePronosticados)
                {
                    continue;
                }

                empatesAcertados[pronostico.UsuarioId] =
                    empatesAcertados.GetValueOrDefault(pronostico.UsuarioId) + 1;
            }

            if (empatesAcertados.Count == 0)
            {
                return;
            }

            int mayorCantidad = empatesAcertados.Max(resultado => resultado.Value);
            foreach (var resultado in empatesAcertados.Where(
                resultado => resultado.Value == mayorCantidad))
            {
                AgregarInsignia(usuarios[resultado.Key], "Rey de los empates");
            }
        }

        private static void AsignarRachas(
            IEnumerable<Usuario> usuarios,
            IEnumerable<Pronostico> pronosticos,
            IReadOnlyDictionary<int, Partido> partidos)
        {
            Dictionary<int, List<Pronostico>> pronosticosPorUsuario = pronosticos
                .Where(pronostico => pronostico.PuntosObtenidos.HasValue)
                .GroupBy(pronostico => pronostico.UsuarioId)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList());

            foreach (Usuario usuario in usuarios)
            {
                if (!pronosticosPorUsuario.TryGetValue(usuario.Id, out List<Pronostico>? historial))
                {
                    continue;
                }

                IEnumerable<Pronostico> historialOrdenado = historial.OrderBy(pronostico =>
                    partidos.GetValueOrDefault(pronostico.PartidoId)?.FechaHora ?? DateTime.MaxValue);
                if (ObtenerMayorRacha(historialOrdenado) >= 10)
                {
                    AgregarInsignia(usuario, "Racha de 10 aciertos");
                }
            }
        }

        private static int ObtenerMayorRacha(IEnumerable<Pronostico> pronosticos)
        {
            int rachaActual = 0;
            int mayorRacha = 0;
            foreach (Pronostico pronostico in pronosticos)
            {
                rachaActual = pronostico.PuntosObtenidos > 0 ? rachaActual + 1 : 0;
                mayorRacha = Math.Max(mayorRacha, rachaActual);
            }

            return mayorRacha;
        }

        private static void AsignarInsigniasPorQuiniela(
            IReadOnlyDictionary<int, Usuario> usuarios,
            IEnumerable<Quiniela> quinielas,
            IReadOnlySet<int> participantesConResultados,
            IReadOnlyDictionary<int, int> golesExactos,
            IReadOnlyDictionary<int, int> goleadoresAcertados)
        {
            foreach (Quiniela quiniela in quinielas)
            {
                List<Usuario> integrantes = quiniela.IntegrantesIds
                    .Where(id =>
                        usuarios.ContainsKey(id) &&
                        participantesConResultados.Contains(id))
                    .Select(id => usuarios[id])
                    .ToList();
                if (integrantes.Count == 0)
                {
                    continue;
                }

                AsignarExtremoQuiniela(integrantes, quiniela.Nombre, esMayor: true);
                if (integrantes.Count > 1)
                {
                    AsignarExtremoQuiniela(integrantes, quiniela.Nombre, esMayor: false);
                }

                AsignarGanadoresDeMetricaQuiniela(
                    integrantes,
                    golesExactos,
                    "Precisión goleadora de quiniela",
                    quiniela.Nombre);
                AsignarGanadoresDeMetricaQuiniela(
                    integrantes,
                    goleadoresAcertados,
                    "Cazagoleadores de quiniela",
                    quiniela.Nombre);
            }
        }

        private static void AsignarGanadoresDeMetrica(
            IReadOnlyDictionary<int, Usuario> usuarios,
            IReadOnlyDictionary<int, int> resultados,
            string insignia)
        {
            List<KeyValuePair<int, int>> participantes = resultados
                .Where(resultado =>
                    resultado.Value > 0 &&
                    usuarios.ContainsKey(resultado.Key))
                .ToList();
            if (participantes.Count == 0)
            {
                return;
            }

            int mayorCantidad = participantes.Max(resultado => resultado.Value);
            foreach (KeyValuePair<int, int> resultado in participantes.Where(
                resultado => resultado.Value == mayorCantidad))
            {
                AgregarInsignia(usuarios[resultado.Key], insignia);
            }
        }

        private static void AsignarGanadoresDeMetricaQuiniela(
            IEnumerable<Usuario> integrantes,
            IReadOnlyDictionary<int, int> resultados,
            string nombreInsignia,
            string nombreQuiniela)
        {
            Dictionary<int, Usuario> integrantesPorId = integrantes
                .ToDictionary(usuario => usuario.Id);
            List<KeyValuePair<int, int>> resultadosDeQuiniela = resultados
                .Where(resultado =>
                    resultado.Value > 0 &&
                    integrantesPorId.ContainsKey(resultado.Key))
                .ToList();
            if (resultadosDeQuiniela.Count == 0)
            {
                return;
            }

            int mayorCantidad = resultadosDeQuiniela.Max(
                resultado => resultado.Value);
            foreach (KeyValuePair<int, int> resultado in resultadosDeQuiniela.Where(
                resultado => resultado.Value == mayorCantidad))
            {
                AgregarInsignia(
                    integrantesPorId[resultado.Key],
                    $"{nombreInsignia}: {nombreQuiniela}");
            }
        }

        private static void AsignarExtremoQuiniela(
            IEnumerable<Usuario> integrantes,
            string nombreQuiniela,
            bool esMayor)
        {
            List<Usuario> lista = integrantes.ToList();
            int puntaje = esMayor
                ? lista.Max(usuario => usuario.Puntos)
                : lista.Min(usuario => usuario.Puntos);
            string prefijo = esMayor ? "Líder" : "Peor";

            foreach (Usuario usuario in lista.Where(usuario => usuario.Puntos == puntaje))
            {
                AgregarInsignia(usuario, $"{prefijo} de quiniela: {nombreQuiniela}");
            }
        }

        private static bool PartidoTieneResultado(Partido partido)
        {
            return partido.Estado == "Finalizado" &&
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue;
        }

        private static void AgregarInsignia(Usuario usuario, string nombreInsignia)
        {
            if (!usuario.Insignias.Contains(nombreInsignia))
            {
                usuario.Insignias.Add(nombreInsignia);
            }
        }
    }
}

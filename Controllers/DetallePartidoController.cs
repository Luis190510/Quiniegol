using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    /// <summary>Prepara la información de un partido para su consulta.</summary>
    public class DetallePartidoController
    {
        private readonly PartidoController _partidoController;
        private readonly JsonRepository<Seleccion> _seleccionRepository;
        private readonly JsonRepository<GoleadorReal> _goleadorRepository;

        public DetallePartidoController()
        {
            _partidoController = new PartidoController();
            _seleccionRepository = new JsonRepository<Seleccion>(
                RutaDatosService.ObtenerRuta("selecciones.json"));
            _goleadorRepository = new JsonRepository<GoleadorReal>(
                RutaDatosService.ObtenerRuta("goleadores2026.json"));
        }

        /// <summary>Obtiene las opciones ordenadas que se muestran en el selector.</summary>
        public List<PartidoOpcionItem> ObtenerOpcionesPartidos()
        {
            Dictionary<int, string> nombres = ObtenerNombresSelecciones();

            return _partidoController.ObtenerPartidos()
                .OrderBy(partido => partido.FechaHora)
                .Select(partido => new PartidoOpcionItem
                {
                    PartidoId = partido.Id,
                    Descripcion =
                        $"{partido.FechaHora:dd/MM/yyyy HH:mm} - " +
                        $"{ObtenerNombre(nombres, partido.SeleccionLocalId)} vs " +
                        ObtenerNombre(nombres, partido.SeleccionVisitanteId)
                })
                .ToList();
        }

        /// <summary>Obtiene el marcador, equipos y goleadores visibles de un partido.</summary>
        public PartidoDetalleItem ObtenerDetalle(int partidoId)
        {
            Partido partido = _partidoController.ObtenerPartidos()
                .FirstOrDefault(elemento => elemento.Id == partidoId)
                ?? throw new InvalidOperationException("No se encontró el partido.");

            Dictionary<int, Seleccion> selecciones = _seleccionRepository
                .ObtenerTodos()
                .ToDictionary(seleccion => seleccion.Id);

            selecciones.TryGetValue(partido.SeleccionLocalId, out Seleccion? local);
            selecciones.TryGetValue(partido.SeleccionVisitanteId, out Seleccion? visitante);

            Dictionary<int, string> nombres = selecciones.ToDictionary(
                elemento => elemento.Key,
                elemento => elemento.Value.Nombre);

            return new PartidoDetalleItem
            {
                PartidoId = partido.Id,
                SeleccionLocalId = partido.SeleccionLocalId,
                SeleccionVisitanteId = partido.SeleccionVisitanteId,
                Local = local?.Nombre ?? "Local",
                Visitante = visitante?.Nombre ?? "Visitante",
                RutaBanderaLocal = local?.RutaBandera ?? string.Empty,
                RutaBanderaVisitante = visitante?.RutaBandera ?? string.Empty,
                FechaHora = partido.FechaHora,
                Estado = partido.Estado,
                Marcador = ObtenerMarcador(partido),
                Anotadores = ObtenerAnotadores(partido, nombres)
            };
        }

        private List<AnotadorVistaItem> ObtenerAnotadores(
            Partido partido,
            IReadOnlyDictionary<int, string> nombres)
        {
            return GoleadoresPartidoService
                .ObtenerVisibles(partido, _goleadorRepository.ObtenerTodos())
                .OrderBy(goleador => ExtraerMinuto(goleador.Minuto))
                .Select(goleador => new AnotadorVistaItem
                {
                    SeleccionId = goleador.SeleccionId,
                    Jugador = goleador.Jugador,
                    Seleccion = ObtenerNombre(nombres, goleador.SeleccionId),
                    Minuto = goleador.Minuto
                })
                .ToList();
        }

        private Dictionary<int, string> ObtenerNombresSelecciones()
        {
            return _seleccionRepository.ObtenerTodos()
                .ToDictionary(seleccion => seleccion.Id, seleccion => seleccion.Nombre);
        }

        private static string ObtenerMarcador(Partido partido)
        {
            return partido.GolesLocal.HasValue && partido.GolesVisitante.HasValue
                ? $"{partido.GolesLocal} - {partido.GolesVisitante}"
                : "Pendiente";
        }

        private static string ObtenerNombre(
            IReadOnlyDictionary<int, string> nombres,
            int seleccionId)
        {
            return nombres.TryGetValue(seleccionId, out string? nombre)
                ? nombre
                : $"Selección {seleccionId}";
        }

        private static int ExtraerMinuto(string minuto)
        {
            string parteNumerica = new((minuto ?? string.Empty)
                .TakeWhile(char.IsDigit)
                .ToArray());

            return int.TryParse(parteNumerica, out int valor)
                ? valor
                : int.MaxValue;
        }
    }
}

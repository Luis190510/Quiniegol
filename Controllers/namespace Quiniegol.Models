using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class HistorialPronosticoController
    {
        private readonly JsonRepository<Pronostico>
            _pronosticoRepository;

        private readonly JsonRepository<Seleccion>
            _seleccionRepository;

        private readonly PartidoController
            _partidoController;

        public HistorialPronosticoController()
        {
            string rutaPronosticos =
                RutaDatosService.ObtenerRuta(
                    "pronosticos.json"
                );

            string rutaSelecciones =
                RutaDatosService.ObtenerRuta(
                    "selecciones.json"
                );

            _pronosticoRepository =
                new JsonRepository<Pronostico>(
                    rutaPronosticos
                );

            _seleccionRepository =
                new JsonRepository<Seleccion>(
                    rutaSelecciones
                );

            _partidoController =
                new PartidoController();
        }

        public List<HistorialPronosticoItem>
            ObtenerPorUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar un usuario."
                );
            }

            List<Pronostico> pronosticos =
                _pronosticoRepository
                    .ObtenerTodos()
                    .Where(pronostico =>
                        pronostico.UsuarioId ==
                        usuarioId
                    )
                    .ToList();

            List<Partido> partidos =
                _partidoController.ObtenerPartidos();

            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            List<HistorialPronosticoItem> historial =
                new List<HistorialPronosticoItem>();

            foreach (Pronostico pronostico in pronosticos)
            {
                Partido? partido =
                    partidos.FirstOrDefault(
                        partidoActual =>
                            partidoActual.Id ==
                            pronostico.PartidoId
                    );

                if (partido == null)
                {
                    continue;
                }

                string nombreLocal =
                    ObtenerNombreSeleccion(
                        selecciones,
                        partido.SeleccionLocalId
                    );

                string nombreVisitante =
                    ObtenerNombreSeleccion(
                        selecciones,
                        partido.SeleccionVisitanteId
                    );

                string resultadoReal = "Pendiente";

                if (partido.Estado == "Finalizado" &&
                    partido.GolesLocal.HasValue &&
                    partido.GolesVisitante.HasValue)
                {
                    resultadoReal =
                        $"{partido.GolesLocal} - " +
                        $"{partido.GolesVisitante}";
                }

                string puntos =
                    pronostico.PuntosObtenidos.HasValue
                        ? pronostico
                            .PuntosObtenidos
                            .Value
                            .ToString()
                        : "Pendiente";

                HistorialPronosticoItem fila =
                    new HistorialPronosticoItem
                    {
                        PronosticoId =
                            pronostico.Id,

                        FechaRegistro =
                            pronostico.FechaRegistro,

                        Partido =
                            $"{nombreLocal} vs " +
                            $"{nombreVisitante}",

                        MarcadorPronosticado =
                            $"{pronostico.GolesLocalPronosticados}" +
                            " - " +
                            $"{pronostico.GolesVisitantePronosticados}",

                        ResultadoReal =
                            resultadoReal,

                        Estado =
                            partido.Estado,

                        Puntos =
                            puntos
                    };

                historial.Add(fila);
            }

            return historial
                .OrderByDescending(fila =>
                    fila.FechaRegistro
                )
                .ToList();
        }

        private string ObtenerNombreSeleccion(
            List<Seleccion> selecciones,
            int seleccionId)
        {
            Seleccion? seleccion =
                selecciones.FirstOrDefault(
                    seleccionActual =>
                        seleccionActual.Id ==
                        seleccionId
                );

            if (seleccion == null)
            {
                return $"Selección {seleccionId}";
            }

            return seleccion.Nombre;
        }
    }
}
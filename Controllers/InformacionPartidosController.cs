using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class InformacionPartidosController
    {
        private readonly PartidoController
            _partidoController;

        private readonly JsonRepository<Seleccion>
            _seleccionRepository;

        private readonly FechaSimuladaService
            _fechaService;

        public InformacionPartidosController()
        {
            _partidoController =
                new PartidoController();

            _seleccionRepository =
                new JsonRepository<Seleccion>(
                    RutaDatosService.ObtenerRuta(
                        "selecciones.json"
                    )
                );

            _fechaService =
                FechaSimuladaService.Instancia;
        }

        public List<PartidoInformacionItem>
            ObtenerUltimosCinco()
        {
            List<Partido> partidos =
                _partidoController.ObtenerPartidos();

            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            return partidos
                .Where(partido =>
                    partido.Estado ==
                    "Finalizado" &&
                    partido.FechaHora <=
                    _fechaService.FechaActual
                )
                .OrderByDescending(partido =>
                    partido.FechaHora
                )
                .Take(5)
                .Select(partido =>
                    CrearItem(
                        partido,
                        selecciones
                    )
                )
                .ToList();
        }

        public List<PartidoInformacionItem>
            ObtenerProximos24Horas()
        {
            DateTime fechaInicial =
                _fechaService.FechaActual;

            DateTime fechaFinal =
                fechaInicial.AddHours(24);

            List<Partido> partidos =
                _partidoController.ObtenerPartidos();

            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            return partidos
                .Where(partido =>
                    partido.FechaHora >
                    fechaInicial &&
                    partido.FechaHora <=
                    fechaFinal
                )
                .OrderBy(partido =>
                    partido.FechaHora
                )
                .Select(partido =>
                    CrearItem(
                        partido,
                        selecciones
                    )
                )
                .ToList();
        }

        public DateTime ObtenerFechaSimulada()
        {
            return _fechaService.FechaActual;
        }

        private PartidoInformacionItem CrearItem(
            Partido partido,
            List<Seleccion> selecciones)
        {
            string local =
                ObtenerNombreSeleccion(
                    selecciones,
                    partido.SeleccionLocalId
                );

            string visitante =
                ObtenerNombreSeleccion(
                    selecciones,
                    partido.SeleccionVisitanteId
                );

            string marcador = "Pendiente";

            if (partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue)
            {
                marcador =
                    $"{partido.GolesLocal} - " +
                    $"{partido.GolesVisitante}";
            }

            return new PartidoInformacionItem
            {
                PartidoId = partido.Id,
                FechaHora = partido.FechaHora,
                Partido =
                    $"{local} vs {visitante}",
                Estado = partido.Estado,
                Marcador = marcador
            };
        }

        private string ObtenerNombreSeleccion(
            List<Seleccion> selecciones,
            int seleccionId)
        {
            Seleccion? seleccion =
                selecciones.FirstOrDefault(
                    elemento =>
                        elemento.Id ==
                        seleccionId
                );

            return seleccion?.Nombre
                ?? $"Selección {seleccionId}";
        }
    }
}

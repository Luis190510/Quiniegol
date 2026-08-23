using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class DetallePartidoController
    {
        private readonly PartidoController
            _partidoController;

        private readonly JsonRepository<Seleccion>
            _seleccionRepository;

        private readonly JsonRepository<GoleadorReal>
            _goleadorRepository;

        public DetallePartidoController()
        {
            _partidoController =
                new PartidoController();

            _seleccionRepository =
                new JsonRepository<Seleccion>(
                    RutaDatosService.ObtenerRuta(
                        "selecciones.json"
                    )
                );

            _goleadorRepository =
                new JsonRepository<GoleadorReal>(
                    RutaDatosService.ObtenerRuta(
                        "goleadores2026.json"
                    )
                );
        }

        public List<PartidoOpcionItem>
            ObtenerOpcionesPartidos()
        {
            List<Partido> partidos =
                _partidoController.ObtenerPartidos();

            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            return partidos
                .OrderBy(partido =>
                    partido.FechaHora
                )
                .Select(partido =>
                    new PartidoOpcionItem
                    {
                        PartidoId = partido.Id,

                        Descripcion =
                            $"{partido.FechaHora:dd/MM/yyyy HH:mm} - " +
                            $"{ObtenerNombre(selecciones, partido.SeleccionLocalId)} " +
                            $"vs " +
                            $"{ObtenerNombre(selecciones, partido.SeleccionVisitanteId)}"
                    }
                )
                .ToList();
        }

        public PartidoDetalleItem ObtenerDetalle(
            int partidoId)
        {
            Partido? partido =
                _partidoController
                    .ObtenerPartidos()
                    .FirstOrDefault(
                        elemento =>
                            elemento.Id ==
                            partidoId
                    );

            if (partido == null)
            {
                throw new InvalidOperationException(
                    "No se encontró el partido."
                );
            }

            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            Seleccion? local =
                selecciones.FirstOrDefault(
                    seleccion =>
                        seleccion.Id ==
                        partido.SeleccionLocalId
                );

            Seleccion? visitante =
                selecciones.FirstOrDefault(
                    seleccion =>
                        seleccion.Id ==
                        partido.SeleccionVisitanteId
                );

            string marcador =
                partido.GolesLocal.HasValue &&
                partido.GolesVisitante.HasValue
                    ? $"{partido.GolesLocal} - " +
                      $"{partido.GolesVisitante}"
                    : "Pendiente";

            List<AnotadorVistaItem> anotadores =
                GoleadoresPartidoService
                    .ObtenerVisibles(
                        partido,
                        _goleadorRepository.ObtenerTodos())
                    .OrderBy(goleador =>
                        ExtraerMinuto(goleador.Minuto))
                    .Select(goleador =>
                    new AnotadorVistaItem
                    {
                        SeleccionId =
                            goleador.SeleccionId,

                        Jugador =
                            goleador.Jugador,

                        Seleccion =
                            ObtenerNombre(
                                selecciones,
                                goleador.SeleccionId
                            ),

                        Minuto =
                            goleador.Minuto
                    }
                )
                .ToList();

            return new PartidoDetalleItem
            {
                PartidoId = partido.Id,
                SeleccionLocalId =
                    partido.SeleccionLocalId,
                SeleccionVisitanteId =
                    partido.SeleccionVisitanteId,
                Local =
                    local?.Nombre ?? "Local",
                Visitante =
                    visitante?.Nombre ?? "Visitante",
                RutaBanderaLocal =
                    local?.RutaBandera ?? "",
                RutaBanderaVisitante =
                    visitante?.RutaBandera ?? "",
                FechaHora =
                    partido.FechaHora,
                Estado =
                    partido.Estado,
                Marcador =
                    marcador,
                Anotadores =
                    anotadores
            };
        }

        private static int ExtraerMinuto(string minuto)
        {
            string parteNumerica = new(
                (minuto ?? string.Empty)
                    .TakeWhile(char.IsDigit)
                    .ToArray()
            );

            return int.TryParse(parteNumerica, out int valor)
                ? valor
                : int.MaxValue;
        }

        private string ObtenerNombre(
            List<Seleccion> selecciones,
            int seleccionId)
        {
            return selecciones
                .FirstOrDefault(seleccion =>
                    seleccion.Id ==
                    seleccionId
                )
                ?.Nombre
                ?? $"Selección {seleccionId}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    public class TablaPosicionesService
    {
        private readonly JsonRepository<Seleccion>
            _seleccionRepository;

        private readonly PartidoController
            _partidoController;

        public TablaPosicionesService()
        {
            _seleccionRepository =
                new JsonRepository<Seleccion>(
                    RutaDatosService.ObtenerRuta(
                        "selecciones.json"
                    )
                );

            _partidoController =
                new PartidoController();
        }

        public List<string> ObtenerGrupos()
        {
            return _seleccionRepository
                .ObtenerTodos()
                .Where(seleccion =>
                    !string.IsNullOrWhiteSpace(
                        seleccion.Grupo
                    )
                )
                .Select(seleccion =>
                    seleccion.Grupo
                )
                .Distinct()
                .OrderBy(grupo =>
                    grupo
                )
                .ToList();
        }

        public List<PosicionGrupoItem>
            CalcularTabla(string grupo)
        {
            if (string.IsNullOrWhiteSpace(grupo))
            {
                throw new ArgumentException(
                    "Debe seleccionar un grupo."
                );
            }

            List<Seleccion> selecciones =
                _seleccionRepository
                    .ObtenerTodos()
                    .Where(seleccion =>
                        string.Equals(
                            seleccion.Grupo,
                            grupo,
                            StringComparison
                                .OrdinalIgnoreCase
                        )
                    )
                    .ToList();

            if (selecciones.Count == 0)
            {
                throw new InvalidOperationException(
                    "No se encontraron selecciones " +
                    "para ese grupo."
                );
            }

            List<PosicionGrupoItem> tabla =
                selecciones
                    .Select(seleccion =>
                        new PosicionGrupoItem
                        {
                            SeleccionId =
                                seleccion.Id,
                            Seleccion =
                                seleccion.Nombre,
                            Grupo =
                                seleccion.Grupo
                        }
                    )
                    .ToList();

            List<int> idsGrupo =
                selecciones
                    .Select(seleccion =>
                        seleccion.Id
                    )
                    .ToList();

            List<Partido> partidosGrupo =
                _partidoController
                    .ObtenerPartidos()
                    .Where(partido =>
                        partido.Id <= 72 &&
                        partido.Estado ==
                        "Finalizado" &&
                        partido.GolesLocal.HasValue &&
                        partido.GolesVisitante.HasValue &&
                        idsGrupo.Contains(
                            partido.SeleccionLocalId
                        ) &&
                        idsGrupo.Contains(
                            partido.SeleccionVisitanteId
                        )
                    )
                    .ToList();

            foreach (Partido partido
                     in partidosGrupo)
            {
                PosicionGrupoItem? local =
                    tabla.FirstOrDefault(
                        fila =>
                            fila.SeleccionId ==
                            partido.SeleccionLocalId
                    );

                PosicionGrupoItem? visitante =
                    tabla.FirstOrDefault(
                        fila =>
                            fila.SeleccionId ==
                            partido.SeleccionVisitanteId
                    );

                if (local == null ||
                    visitante == null)
                {
                    continue;
                }

                int golesLocal =
                    partido.GolesLocal ?? 0;

                int golesVisitante =
                    partido.GolesVisitante ?? 0;

                local.PartidosJugados++;
                visitante.PartidosJugados++;

                local.GolesFavor +=
                    golesLocal;

                local.GolesContra +=
                    golesVisitante;

                visitante.GolesFavor +=
                    golesVisitante;

                visitante.GolesContra +=
                    golesLocal;

                if (golesLocal > golesVisitante)
                {
                    local.PartidosGanados++;
                    visitante.PartidosPerdidos++;

                    local.Puntos += 3;
                }
                else if (
                    golesVisitante > golesLocal)
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

            List<PosicionGrupoItem> ordenada =
                tabla
                    .OrderByDescending(fila =>
                        fila.Puntos
                    )
                    .ThenByDescending(fila =>
                        fila.DiferenciaGoles
                    )
                    .ThenByDescending(fila =>
                        fila.GolesFavor
                    )
                    .ThenBy(fila =>
                        fila.Seleccion
                    )
                    .ToList();

            for (int indice = 0;
                 indice < ordenada.Count;
                 indice++)
            {
                ordenada[indice].Posicion =
                    indice + 1;
            }

            return ordenada;
        }
    }
}

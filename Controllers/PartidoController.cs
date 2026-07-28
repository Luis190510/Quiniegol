using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class PartidoController
    {
        private readonly JsonRepository<Partido>
            _partidoRepository;

        private readonly JsonRepository<ResultadoPartido>
            _resultadoRepository;

        private readonly FechaSimuladaService
            _fechaService;

        private const int DuracionPartidoMinutos = 120;

        public PartidoController()
        {
            string rutaPartidos = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "partidos.json"
            );

            string rutaResultados = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "resultados2026.json"
            );

            _partidoRepository =
                new JsonRepository<Partido>(
                    rutaPartidos
                );

            _resultadoRepository =
                new JsonRepository<ResultadoPartido>(
                    rutaResultados
                );

            _fechaService =
                FechaSimuladaService.Instancia;
        }

        public List<Partido> ObtenerPartidos()
        {
            List<Partido> partidos =
                _partidoRepository.ObtenerTodos();

            List<ResultadoPartido> resultados =
                _resultadoRepository.ObtenerTodos();

            ActualizarEstadosYResultados(
                partidos,
                resultados
            );

            return partidos
                .OrderBy(partido => partido.FechaHora)
                .ToList();
        }

        public List<Partido> ObtenerPartidosPendientes()
        {
            return ObtenerPartidos()
                .Where(partido =>
                    _fechaService.FechaActual <
                    partido.FechaHora
                )
                .ToList();
        }

        private void ActualizarEstadosYResultados(
            List<Partido> partidos,
            List<ResultadoPartido> resultados)
        {
            bool huboCambios = false;

            foreach (Partido partido in partidos)
            {
                string nuevoEstado;
                int? nuevosGolesLocal = null;
                int? nuevosGolesVisitante = null;

                DateTime fechaFinalAproximada =
                    partido.FechaHora.AddMinutes(
                        DuracionPartidoMinutos
                    );

                if (_fechaService.FechaActual <
                    partido.FechaHora)
                {
                    nuevoEstado = "Pendiente";
                }
                else if (_fechaService.FechaActual <
                         fechaFinalAproximada)
                {
                    nuevoEstado = "En curso";
                }
                else
                {
                    ResultadoPartido? resultado =
                        resultados.FirstOrDefault(
                            resultadoActual =>
                                resultadoActual.PartidoId ==
                                partido.Id
                        );

                    if (resultado != null)
                    {
                        nuevoEstado = "Finalizado";
                        nuevosGolesLocal =
                            resultado.GolesLocal;
                        nuevosGolesVisitante =
                            resultado.GolesVisitante;
                    }
                    else
                    {
                        nuevoEstado =
                            "Pendiente de resultado";
                    }
                }

                if (partido.Estado != nuevoEstado ||
                    partido.GolesLocal !=
                    nuevosGolesLocal ||
                    partido.GolesVisitante !=
                    nuevosGolesVisitante)
                {
                    partido.Estado = nuevoEstado;
                    partido.GolesLocal =
                        nuevosGolesLocal;
                    partido.GolesVisitante =
                        nuevosGolesVisitante;

                    huboCambios = true;
                }
            }

            if (huboCambios)
            {
                _partidoRepository.GuardarTodos(
                    partidos
                );
            }
        }

        public void RegistrarPartido(
            int seleccionLocalId,
            int seleccionVisitanteId,
            DateTime fechaHora)
        {
            if (seleccionLocalId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar el equipo local."
                );
            }

            if (seleccionVisitanteId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar el equipo visitante."
                );
            }

            if (seleccionLocalId ==
                seleccionVisitanteId)
            {
                throw new InvalidOperationException(
                    "Una selección no puede jugar contra sí misma."
                );
            }

            List<Partido> partidos =
                _partidoRepository.ObtenerTodos();

            bool partidoRepetido = partidos.Any(
                partido =>
                    partido.FechaHora == fechaHora &&
                    partido.SeleccionLocalId ==
                    seleccionLocalId &&
                    partido.SeleccionVisitanteId ==
                    seleccionVisitanteId
            );

            if (partidoRepetido)
            {
                throw new InvalidOperationException(
                    "Ese partido ya se encuentra registrado."
                );
            }

            int nuevoId = partidos.Count == 0
                ? 1
                : partidos.Max(
                    partido => partido.Id
                ) + 1;

            Partido nuevoPartido = new()
            {
                Id = nuevoId,
                SeleccionLocalId =
                    seleccionLocalId,
                SeleccionVisitanteId =
                    seleccionVisitanteId,
                FechaHora = fechaHora,
                GolesLocal = null,
                GolesVisitante = null,
                Estado = "Pendiente",
                Anotadores = new List<Anotador>()
            };

            partidos.Add(nuevoPartido);

            _partidoRepository.GuardarTodos(
                partidos
            );
        }

        public void GuardarResultado(
            int partidoId,
            int golesLocal,
            int golesVisitante)
        {
            if (golesLocal < 0 ||
                golesVisitante < 0)
            {
                throw new ArgumentException(
                    "Los goles no pueden ser negativos."
                );
            }

            List<ResultadoPartido> resultados =
                _resultadoRepository.ObtenerTodos();

            ResultadoPartido? resultado =
                resultados.FirstOrDefault(
                    resultadoActual =>
                        resultadoActual.PartidoId ==
                        partidoId
                );

            if (resultado == null)
            {
                resultado = new ResultadoPartido
                {
                    PartidoId = partidoId,
                    GolesLocal = golesLocal,
                    GolesVisitante =
                        golesVisitante
                };

                resultados.Add(resultado);
            }
            else
            {
                resultado.GolesLocal =
                    golesLocal;

                resultado.GolesVisitante =
                    golesVisitante;
            }

            _resultadoRepository.GuardarTodos(
                resultados
            );
        }

        public void EliminarPartido(
            int partidoId)
        {
            List<Partido> partidos =
                _partidoRepository.ObtenerTodos();

            Partido? partido =
                partidos.FirstOrDefault(
                    partidoActual =>
                        partidoActual.Id ==
                        partidoId
                );

            if (partido == null)
            {
                throw new InvalidOperationException(
                    "No se encontró el partido seleccionado."
                );
            }

            partidos.Remove(partido);

            _partidoRepository.GuardarTodos(
                partidos
            );

            List<ResultadoPartido> resultados =
                _resultadoRepository.ObtenerTodos();

            ResultadoPartido? resultado =
                resultados.FirstOrDefault(
                    resultadoActual =>
                        resultadoActual.PartidoId ==
                        partidoId
                );

            if (resultado != null)
            {
                resultados.Remove(resultado);

                _resultadoRepository.GuardarTodos(
                    resultados
                );
            }
        }
    }
}
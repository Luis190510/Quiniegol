using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    public class EstadisticasService
    {
        private readonly JsonRepository<Pronostico>
            _pronosticoRepository;

        private readonly JsonRepository<Usuario>
            _usuarioRepository;

        private readonly JsonRepository<Seleccion>
            _seleccionRepository;

        private readonly PartidoController
            _partidoController;

        private readonly PuntajeController
            _puntajeController;

        public EstadisticasService()
        {
            _pronosticoRepository =
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta(
                        "pronosticos.json"
                    )
                );

            _usuarioRepository =
                new JsonRepository<Usuario>(
                    RutaDatosService.ObtenerRuta(
                        "usuarios.json"
                    )
                );

            _seleccionRepository =
                new JsonRepository<Seleccion>(
                    RutaDatosService.ObtenerRuta(
                        "selecciones.json"
                    )
                );

            _partidoController =
                new PartidoController();

            _puntajeController =
                new PuntajeController();
        }

        public List<EstadisticaItem>
            ObtenerEstadisticas(
                DateTime fechaDesde,
                DateTime fechaHasta)
        {
            DateTime inicio =
                fechaDesde.Date;

            DateTime final =
                fechaHasta
                    .Date
                    .AddDays(1)
                    .AddTicks(-1);

            if (inicio > final)
            {
                throw new ArgumentException(
                    "La fecha inicial no puede ser mayor que la fecha final."
                );
            }

            _puntajeController
                .CalcularTodosLosPuntajes();

            List<Pronostico> pronosticos =
                _pronosticoRepository
                    .ObtenerTodos()
                    .Where(pronostico =>
                        pronostico.FechaRegistro >=
                        inicio &&
                        pronostico.FechaRegistro <=
                        final
                    )
                    .ToList();

            List<Partido> partidos =
                _partidoController
                    .ObtenerPartidos();

            List<Partido> partidosRango =
                partidos
                    .Where(partido =>
                        partido.FechaHora >= inicio &&
                        partido.FechaHora <= final
                    )
                    .ToList();

            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            List<Seleccion> selecciones =
                _seleccionRepository.ObtenerTodos();

            return new List<EstadisticaItem>
            {
                new EstadisticaItem
                {
                    Estadistica =
                        "Equipo más apostado como ganador",

                    Resultado =
                        ObtenerEquipoMasApostado(
                            pronosticos,
                            partidos,
                            selecciones
                        )
                },

                new EstadisticaItem
                {
                    Estadistica =
                        "Marcador más repetido",

                    Resultado =
                        ObtenerMarcadorMasRepetido(
                            pronosticos
                        )
                },

                new EstadisticaItem
                {
                    Estadistica =
                        "Partido con más aciertos",

                    Resultado =
                        ObtenerPartidoConMasAciertos(
                            pronosticos,
                            partidos,
                            selecciones
                        )
                },

                new EstadisticaItem
                {
                    Estadistica =
                        "Usuario con más aciertos",

                    Resultado =
                        ObtenerUsuarioConMasAciertos(
                            pronosticos,
                            usuarios
                        )
                },

                new EstadisticaItem
                {
                    Estadistica =
                        "Partido con más pronósticos",

                    Resultado =
                        ObtenerPartidoConMasPronosticos(
                            pronosticos,
                            partidos,
                            selecciones
                        )
                },

                new EstadisticaItem
                {
                    Estadistica =
                        "Equipo sorpresa",

                    Resultado =
                        ObtenerEquipoSorpresa(
                            pronosticos,
                            partidosRango,
                            selecciones
                        )
                },

                new EstadisticaItem
                {
                    Estadistica =
                        "Promedio de goles por partido",

                    Resultado =
                        ObtenerPromedioGoles(
                            partidosRango
                        )
                }
            };
        }

        private string ObtenerEquipoMasApostado(
            List<Pronostico> pronosticos,
            List<Partido> partidos,
            List<Seleccion> selecciones)
        {
            List<int> equiposElegidos =
                new List<int>();

            foreach (Pronostico pronostico
                     in pronosticos)
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

                if (pronostico.GolesLocalPronosticados >
                    pronostico.GolesVisitantePronosticados)
                {
                    equiposElegidos.Add(
                        partido.SeleccionLocalId
                    );
                }
                else if (
                    pronostico.GolesVisitantePronosticados >
                    pronostico.GolesLocalPronosticados)
                {
                    equiposElegidos.Add(
                        partido.SeleccionVisitanteId
                    );
                }
            }

            var grupo =
                equiposElegidos
                    .GroupBy(id => id)
                    .OrderByDescending(
                        grupoActual =>
                            grupoActual.Count()
                    )
                    .FirstOrDefault();

            if (grupo == null)
            {
                return "Sin datos";
            }

            return
                $"{ObtenerNombreSeleccion(selecciones, grupo.Key)} " +
                $"({grupo.Count()} apuestas)";
        }

        private string ObtenerMarcadorMasRepetido(
            List<Pronostico> pronosticos)
        {
            var grupo =
                pronosticos
                    .GroupBy(pronostico =>
                        new
                        {
                            pronostico
                                .GolesLocalPronosticados,

                            pronostico
                                .GolesVisitantePronosticados
                        }
                    )
                    .OrderByDescending(
                        grupoActual =>
                            grupoActual.Count()
                    )
                    .FirstOrDefault();

            if (grupo == null)
            {
                return "Sin datos";
            }

            return
                $"{grupo.Key.GolesLocalPronosticados} - " +
                $"{grupo.Key.GolesVisitantePronosticados} " +
                $"({grupo.Count()} veces)";
        }

        private string ObtenerPartidoConMasAciertos(
            List<Pronostico> pronosticos,
            List<Partido> partidos,
            List<Seleccion> selecciones)
        {
            var grupo =
                pronosticos
                    .Where(pronostico =>
                        pronostico
                            .PuntosObtenidos
                            .HasValue &&
                        pronostico
                            .PuntosObtenidos
                            .Value > 0
                    )
                    .GroupBy(pronostico =>
                        pronostico.PartidoId
                    )
                    .OrderByDescending(
                        grupoActual =>
                            grupoActual.Count()
                    )
                    .FirstOrDefault();

            if (grupo == null)
            {
                return "Sin datos";
            }

            return
                $"{ObtenerNombrePartido(partidos, selecciones, grupo.Key)} " +
                $"({grupo.Count()} aciertos)";
        }

        private string ObtenerUsuarioConMasAciertos(
            List<Pronostico> pronosticos,
            List<Usuario> usuarios)
        {
            var grupo =
                pronosticos
                    .Where(pronostico =>
                        pronostico
                            .PuntosObtenidos
                            .HasValue &&
                        pronostico
                            .PuntosObtenidos
                            .Value > 0
                    )
                    .GroupBy(pronostico =>
                        pronostico.UsuarioId
                    )
                    .OrderByDescending(
                        grupoActual =>
                            grupoActual.Count()
                    )
                    .FirstOrDefault();

            if (grupo == null)
            {
                return "Sin datos";
            }

            Usuario? usuario =
                usuarios.FirstOrDefault(
                    usuarioActual =>
                        usuarioActual.Id ==
                        grupo.Key
                );

            return
                $"{usuario?.Nombre ?? "Usuario desconocido"} " +
                $"({grupo.Count()} aciertos)";
        }

        private string ObtenerPartidoConMasPronosticos(
            List<Pronostico> pronosticos,
            List<Partido> partidos,
            List<Seleccion> selecciones)
        {
            var grupo =
                pronosticos
                    .GroupBy(pronostico =>
                        pronostico.PartidoId
                    )
                    .OrderByDescending(
                        grupoActual =>
                            grupoActual.Count()
                    )
                    .FirstOrDefault();

            if (grupo == null)
            {
                return "Sin datos";
            }

            return
                $"{ObtenerNombrePartido(partidos, selecciones, grupo.Key)} " +
                $"({grupo.Count()} pronósticos)";
        }

        private string ObtenerPromedioGoles(
            List<Partido> partidos)
        {
            List<Partido> finalizados =
                partidos
                    .Where(partido =>
                        partido.Estado ==
                        "Finalizado" &&
                        partido.GolesLocal.HasValue &&
                        partido.GolesVisitante.HasValue
                    )
                    .ToList();

            if (finalizados.Count == 0)
            {
                return "Sin partidos finalizados";
            }

            double totalGoles =
                finalizados.Sum(partido =>
                    (partido.GolesLocal ?? 0) +
                    (partido.GolesVisitante ?? 0)
                );

            double promedio =
                totalGoles /
                finalizados.Count;

            return promedio.ToString("0.00");
        }

        private string ObtenerEquipoSorpresa(
            List<Pronostico> pronosticos,
            List<Partido> partidos,
            List<Seleccion> selecciones)
        {
            int? mejorEquipoId = null;
            double mayorPorcentaje = 0;

            foreach (Partido partido in partidos)
            {
                if (partido.Estado != "Finalizado" ||
                    !partido.GolesLocal.HasValue ||
                    !partido.GolesVisitante.HasValue)
                {
                    continue;
                }

                int? ganadorRealId = null;
                int? rivalId = null;

                if (partido.GolesLocal >
                    partido.GolesVisitante)
                {
                    ganadorRealId =
                        partido.SeleccionLocalId;

                    rivalId =
                        partido.SeleccionVisitanteId;
                }
                else if (
                    partido.GolesVisitante >
                    partido.GolesLocal)
                {
                    ganadorRealId =
                        partido.SeleccionVisitanteId;

                    rivalId =
                        partido.SeleccionLocalId;
                }

                if (!ganadorRealId.HasValue ||
                    !rivalId.HasValue)
                {
                    continue;
                }

                List<Pronostico> pronosticosPartido =
                    pronosticos
                        .Where(pronostico =>
                            pronostico.PartidoId ==
                            partido.Id
                        )
                        .ToList();

                if (pronosticosPartido.Count == 0)
                {
                    continue;
                }

                int apuestasPorElRival =
                    pronosticosPartido.Count(
                        pronostico =>
                            ObtenerGanadorPronosticado(
                                pronostico,
                                partido
                            ) == rivalId
                    );

                double porcentaje =
                    (double)apuestasPorElRival /
                    pronosticosPartido.Count;

                if (porcentaje >= 0.60 &&
                    porcentaje > mayorPorcentaje)
                {
                    mayorPorcentaje =
                        porcentaje;

                    mejorEquipoId =
                        ganadorRealId;
                }
            }

            if (!mejorEquipoId.HasValue)
            {
                return "No se encontró un equipo sorpresa";
            }

            return
                $"{ObtenerNombreSeleccion(selecciones, mejorEquipoId.Value)} " +
                $"({mayorPorcentaje:P0} apostó en su contra)";
        }

        private int? ObtenerGanadorPronosticado(
            Pronostico pronostico,
            Partido partido)
        {
            if (pronostico.GolesLocalPronosticados >
                pronostico.GolesVisitantePronosticados)
            {
                return partido.SeleccionLocalId;
            }

            if (pronostico.GolesVisitantePronosticados >
                pronostico.GolesLocalPronosticados)
            {
                return partido.SeleccionVisitanteId;
            }

            return null;
        }

        private string ObtenerNombrePartido(
            List<Partido> partidos,
            List<Seleccion> selecciones,
            int partidoId)
        {
            Partido? partido =
                partidos.FirstOrDefault(
                    partidoActual =>
                        partidoActual.Id ==
                        partidoId
                );

            if (partido == null)
            {
                return "Partido desconocido";
            }

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

            return $"{local} vs {visitante}";
        }

        private string ObtenerNombreSeleccion(
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

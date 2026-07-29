
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class PuntajeController
    {
        private readonly JsonRepository<Usuario>
            _usuarioRepository;

        private readonly JsonRepository<Pronostico>
            _pronosticoRepository;

        private readonly PartidoController
            _partidoController;

        public PuntajeController()
        {
            string rutaUsuarios =
                RutaDatosService.ObtenerRuta(
                "usuarios.json"
            );

            string rutaPronosticos =
                RutaDatosService.ObtenerRuta(
                    "pronosticos.json"
                );

            _usuarioRepository =
                new JsonRepository<Usuario>(
                    rutaUsuarios
                );

            _pronosticoRepository =
                new JsonRepository<Pronostico>(
                    rutaPronosticos
                );

            _partidoController =
                new PartidoController();
        }

        public void CalcularTodosLosPuntajes()
        {
            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            List<Partido> partidos =
                _partidoController.ObtenerPartidos();

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
                    pronostico.PuntosObtenidos = null;
                    continue;
                }

                if (partido.Estado != "Finalizado")
                {
                    pronostico.PuntosObtenidos = null;
                    continue;
                }

                if (!partido.GolesLocal.HasValue ||
                    !partido.GolesVisitante.HasValue)
                {
                    pronostico.PuntosObtenidos = null;
                    continue;
                }

                pronostico.PuntosObtenidos =
                    CalcularPuntosPronostico(
                        pronostico,
                        partido
                    );
            }

            foreach (Usuario usuario in usuarios)
            {
                usuario.Puntos = pronosticos
                    .Where(pronostico =>
                        pronostico.UsuarioId ==
                        usuario.Id &&
                        pronostico.PuntosObtenidos.HasValue
                    )
                    .Sum(pronostico =>
                        pronostico.PuntosObtenidos ?? 0
                    );
            }

            _pronosticoRepository.GuardarTodos(
                pronosticos
            );

            _usuarioRepository.GuardarTodos(
                usuarios
            );
        }

        private int CalcularPuntosPronostico(
            Pronostico pronostico,
            Partido partido)
        {
            int golesLocalReal =
                partido.GolesLocal ?? 0;

            int golesVisitanteReal =
                partido.GolesVisitante ?? 0;

            bool marcadorExacto =
                pronostico.GolesLocalPronosticados ==
                golesLocalReal &&
                pronostico.GolesVisitantePronosticados ==
                golesVisitanteReal;

            if (marcadorExacto)
            {
                return 5;
            }

            int resultadoPronosticado =
                ObtenerTipoResultado(
                    pronostico
                        .GolesLocalPronosticados,
                    pronostico
                        .GolesVisitantePronosticados
                );

            int resultadoReal =
                ObtenerTipoResultado(
                    golesLocalReal,
                    golesVisitanteReal
                );

            if (resultadoPronosticado ==
                resultadoReal)
            {
                return 2;
            }

            return 0;
        }

        private int ObtenerTipoResultado(
            int golesLocal,
            int golesVisitante)
        {
            if (golesLocal > golesVisitante)
            {
                return 1;
            }

            if (golesLocal < golesVisitante)
            {
                return 2;
            }

            return 0;
        }

        public List<RankingItem> ObtenerRanking()
        {
            CalcularTodosLosPuntajes();

            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            List<Usuario> usuariosOrdenados =
                usuarios
                    .OrderByDescending(
                        usuario => usuario.Puntos
                    )
                    .ThenBy(
                        usuario => usuario.Nombre
                    )
                    .ToList();

            List<RankingItem> ranking =
                new List<RankingItem>();

            for (int indice = 0;
                 indice < usuariosOrdenados.Count;
                 indice++)
            {
                Usuario usuario =
                    usuariosOrdenados[indice];

                RankingItem filaRanking =
                    new RankingItem
                    {
                        Posicion = indice + 1,
                        UsuarioId = usuario.Id,
                        Nombre = usuario.Nombre,
                        PaisPreferido =
                            usuario.PaisPreferido,
                        Puntos = usuario.Puntos,
                        Insignias =
                            string.Join(
                                ", ",
                                usuario.Insignias ??
                                new List<string>()
                            )
                    };

                ranking.Add(filaRanking);
            }

            return ranking;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class RankingPrivadoController
    {
        private readonly JsonRepository<Quiniela>
            _quinielaRepository;

        private readonly JsonRepository<Usuario>
            _usuarioRepository;

        private readonly PuntajeController
            _puntajeController;

        public RankingPrivadoController()
        {
            string rutaQuinielas =
                RutaDatosService.ObtenerRuta(
                    "quinielas.json"
                );

            string rutaUsuarios =
                RutaDatosService.ObtenerRuta(
                    "usuarios.json"
                );

            _quinielaRepository =
                new JsonRepository<Quiniela>(
                    rutaQuinielas
                );

            _usuarioRepository =
                new JsonRepository<Usuario>(
                    rutaUsuarios
                );

            _puntajeController =
                new PuntajeController();
        }

        public List<RankingItem> ObtenerRanking(
            int quinielaId)
        {
            _puntajeController
                .CalcularTodosLosPuntajes();

            Quiniela? quiniela =
                _quinielaRepository
                    .ObtenerTodos()
                    .FirstOrDefault(
                        quinielaActual =>
                            quinielaActual.Id ==
                            quinielaId
                    );

            if (quiniela == null)
            {
                throw new InvalidOperationException(
                    "No se encontró la quiniela."
                );
            }

            List<Usuario> usuariosOrdenados =
                _usuarioRepository
                    .ObtenerTodos()
                    .Where(usuario =>
                        quiniela.IntegrantesIds.Contains(
                            usuario.Id
                        )
                    )
                    .OrderByDescending(usuario =>
                        usuario.Puntos
                    )
                    .ThenBy(usuario =>
                        usuario.Nombre
                    )
                    .ToList();

            List<RankingItem> ranking =
                new List<RankingItem>();

            int posicionActual = 0;
            int? puntosAnteriores = null;

            for (int indice = 0;
                 indice < usuariosOrdenados.Count;
                 indice++)
            {
                Usuario usuario =
                    usuariosOrdenados[indice];

                if (!puntosAnteriores.HasValue ||
                    usuario.Puntos !=
                    puntosAnteriores.Value)
                {
                    posicionActual = indice + 1;
                }

                RankingItem fila =
                    new RankingItem
                    {
                        Posicion =
                            posicionActual,

                        UsuarioId =
                            usuario.Id,

                        Nombre =
                            usuario.Nombre,

                        PaisPreferido =
                            usuario.PaisPreferido,

                        Puntos =
                            usuario.Puntos
                    };

                ranking.Add(fila);

                puntosAnteriores =
                    usuario.Puntos;
            }

            return ranking;
        }
    }
}

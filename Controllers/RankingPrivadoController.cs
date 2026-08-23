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

            AccesoQuinielaService.ExigirConsulta(
                quiniela,
                SesionUsuarioService.UsuarioActual
            );

            IEnumerable<Usuario> integrantes =
                _usuarioRepository
                    .ObtenerTodos()
                    .Where(usuario =>
                        usuario.Rol == RolUsuario.Usuario &&
                        quiniela.IntegrantesIds.Contains(usuario.Id));

            return RankingService.Crear(
                integrantes,
                usuario => VisibilidadInsigniasService.ObtenerDeQuiniela(
                    usuario.Insignias,
                    quiniela.Nombre)
            );
        }
    }
}


using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;
using Quiniegol.Strategies;

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

        private readonly CalculadoraPuntajeService
            _calculadoraPuntaje;

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

            _calculadoraPuntaje =
                new CalculadoraPuntajeService();
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
                    _calculadoraPuntaje.Calcular(
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

        public List<RankingItem> ObtenerRanking()
        {
            CalcularTodosLosPuntajes();

            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            return RankingService.Crear(
                usuarios.Where(usuario =>
                    usuario.Rol == RolUsuario.Usuario),
                usuario => VisibilidadInsigniasService
                    .ObtenerGlobales(usuario.Insignias)
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    public class InsigniaService
    {
        private readonly JsonRepository<Usuario>
            _usuarioRepository;

        private readonly JsonRepository<Pronostico>
            _pronosticoRepository;

        private readonly JsonRepository<Quiniela>
            _quinielaRepository;

        private readonly PartidoController
            _partidoController;

        private readonly PuntajeController
            _puntajeController;

        public InsigniaService()
        {
            _usuarioRepository =
                new JsonRepository<Usuario>(
                    RutaDatosService.ObtenerRuta(
                        "usuarios.json"
                    )
                );

            _pronosticoRepository =
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta(
                        "pronosticos.json"
                    )
                );

            _quinielaRepository =
                new JsonRepository<Quiniela>(
                    RutaDatosService.ObtenerRuta(
                        "quinielas.json"
                    )
                );

            _partidoController =
                new PartidoController();

            _puntajeController =
                new PuntajeController();
        }

        public List<Insignia> ObtenerCatalogo()
        {
            return new List<Insignia>
            {
                new Insignia
                {
                    Nombre = "Líder global",
                    Descripcion =
                        "Usuario con mayor puntaje global.",
                    Tipo = "Positiva"
                },
                new Insignia
                {
                    Nombre = "Rey de los empates",
                    Descripcion =
                        "Usuario con más empates acertados.",
                    Tipo = "Positiva"
                },
                new Insignia
                {
                    Nombre = "Racha de 10 aciertos",
                    Descripcion =
                        "Usuario con al menos 10 aciertos consecutivos.",
                    Tipo = "Positiva"
                },
                new Insignia
                {
                    Nombre = "Peor del ranking global",
                    Descripcion =
                        "Usuario con menor puntaje global.",
                    Tipo = "Vergüenza"
                }
            };
        }

        public void RecalcularInsignias()
        {
            _puntajeController
                .CalcularTodosLosPuntajes();

            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            List<Partido> partidos =
                _partidoController.ObtenerPartidos();

            List<Usuario> participantes = usuarios
                .Where(usuario =>
                    usuario.Rol == RolUsuario.Usuario)
                .ToList();

            foreach (Usuario usuario in usuarios)
            {
                usuario.Insignias ??=
                    new List<string>();

                usuario.Insignias.RemoveAll(
                    EsInsigniaAutomatica
                );
            }

            AsignarLiderGlobal(participantes);

            AsignarPeorGlobal(participantes);

            AsignarReyDeLosEmpates(
                participantes,
                pronosticos,
                partidos
            );

            AsignarRachas(
                participantes,
                pronosticos,
                partidos
            );

            AsignarInsigniasPorQuiniela(
                participantes,
                quinielas
            );

            _usuarioRepository.GuardarTodos(
                usuarios
            );
        }

        private bool EsInsigniaAutomatica(
            string insignia)
        {
            return
                insignia == "Líder global" ||
                insignia == "Peor del ranking global" ||
                insignia == "Rey de los empates" ||
                insignia == "Racha de 10 aciertos" ||
                insignia.StartsWith(
                    "Líder de quiniela:"
                ) ||
                insignia.StartsWith(
                    "Peor de quiniela:"
                );
        }

        private void AsignarLiderGlobal(
            List<Usuario> usuarios)
        {
            if (usuarios.Count == 0)
            {
                return;
            }

            int mayorPuntaje =
                usuarios.Max(usuario =>
                    usuario.Puntos
                );

            foreach (Usuario usuario in usuarios
                         .Where(usuario =>
                             usuario.Puntos ==
                             mayorPuntaje
                         ))
            {
                AgregarInsignia(
                    usuario,
                    "Líder global"
                );
            }
        }

        private void AsignarPeorGlobal(
            List<Usuario> usuarios)
        {
            if (usuarios.Count <= 1)
            {
                return;
            }

            int menorPuntaje =
                usuarios.Min(usuario =>
                    usuario.Puntos
                );

            foreach (Usuario usuario in usuarios
                         .Where(usuario =>
                             usuario.Puntos ==
                             menorPuntaje
                         ))
            {
                AgregarInsignia(
                    usuario,
                    "Peor del ranking global"
                );
            }
        }

        private void AsignarReyDeLosEmpates(
            List<Usuario> usuarios,
            List<Pronostico> pronosticos,
            List<Partido> partidos)
        {
            Dictionary<int, int> empatesAcertados =
                new Dictionary<int, int>();

            foreach (Pronostico pronostico
                     in pronosticos)
            {
                Partido? partido =
                    partidos.FirstOrDefault(
                        partidoActual =>
                            partidoActual.Id ==
                            pronostico.PartidoId
                    );

                if (partido == null ||
                    partido.Estado != "Finalizado" ||
                    !partido.GolesLocal.HasValue ||
                    !partido.GolesVisitante.HasValue)
                {
                    continue;
                }

                bool resultadoRealFueEmpate =
                    partido.GolesLocal ==
                    partido.GolesVisitante;

                bool pronosticoFueEmpate =
                    pronostico
                        .GolesLocalPronosticados ==
                    pronostico
                        .GolesVisitantePronosticados;

                if (!resultadoRealFueEmpate ||
                    !pronosticoFueEmpate)
                {
                    continue;
                }

                if (!empatesAcertados.ContainsKey(
                    pronostico.UsuarioId
                ))
                {
                    empatesAcertados[
                        pronostico.UsuarioId
                    ] = 0;
                }

                empatesAcertados[
                    pronostico.UsuarioId
                ]++;
            }

            if (empatesAcertados.Count == 0)
            {
                return;
            }

            int mayorCantidad =
                empatesAcertados
                    .Max(elemento =>
                        elemento.Value
                    );

            foreach (var elemento in empatesAcertados
                         .Where(elemento =>
                             elemento.Value ==
                             mayorCantidad
                         ))
            {
                Usuario? usuario =
                    usuarios.FirstOrDefault(
                        usuarioActual =>
                            usuarioActual.Id ==
                            elemento.Key
                    );

                if (usuario != null)
                {
                    AgregarInsignia(
                        usuario,
                        "Rey de los empates"
                    );
                }
            }
        }

        private void AsignarRachas(
            List<Usuario> usuarios,
            List<Pronostico> pronosticos,
            List<Partido> partidos)
        {
            foreach (Usuario usuario in usuarios)
            {
                List<Pronostico> pronosticosUsuario =
                    pronosticos
                        .Where(pronostico =>
                            pronostico.UsuarioId ==
                            usuario.Id &&
                            pronostico
                                .PuntosObtenidos
                                .HasValue
                        )
                        .OrderBy(pronostico =>
                            partidos.FirstOrDefault(
                                partido =>
                                    partido.Id ==
                                    pronostico.PartidoId
                            )
                            ?.FechaHora
                            ?? DateTime.MaxValue
                        )
                        .ToList();

                int rachaActual = 0;
                int mayorRacha = 0;

                foreach (Pronostico pronostico
                         in pronosticosUsuario)
                {
                    if (pronostico.PuntosObtenidos >
                        0)
                    {
                        rachaActual++;

                        if (rachaActual > mayorRacha)
                        {
                            mayorRacha =
                                rachaActual;
                        }
                    }
                    else
                    {
                        rachaActual = 0;
                    }
                }

                if (mayorRacha >= 10)
                {
                    AgregarInsignia(
                        usuario,
                        "Racha de 10 aciertos"
                    );
                }
            }
        }

        private void AsignarInsigniasPorQuiniela(
            List<Usuario> usuarios,
            List<Quiniela> quinielas)
        {
            foreach (Quiniela quiniela in quinielas)
            {
                List<Usuario> integrantes =
                    usuarios
                        .Where(usuario =>
                            quiniela
                                .IntegrantesIds
                                .Contains(
                                    usuario.Id
                                )
                        )
                        .ToList();

                if (integrantes.Count == 0)
                {
                    continue;
                }

                int mayorPuntaje =
                    integrantes.Max(usuario =>
                        usuario.Puntos
                    );

                foreach (Usuario lider
                         in integrantes.Where(
                             usuario =>
                                 usuario.Puntos ==
                                 mayorPuntaje
                         ))
                {
                    AgregarInsignia(
                        lider,
                        $"Líder de quiniela: " +
                        $"{quiniela.Nombre}"
                    );
                }

                if (integrantes.Count <= 1)
                {
                    continue;
                }

                int menorPuntaje =
                    integrantes.Min(usuario =>
                        usuario.Puntos
                    );

                foreach (Usuario ultimo
                         in integrantes.Where(
                             usuario =>
                                 usuario.Puntos ==
                                 menorPuntaje
                         ))
                {
                    AgregarInsignia(
                        ultimo,
                        $"Peor de quiniela: " +
                        $"{quiniela.Nombre}"
                    );
                }
            }
        }

        private void AgregarInsignia(
            Usuario usuario,
            string nombreInsignia)
        {
            if (!usuario.Insignias.Contains(
                nombreInsignia
            ))
            {
                usuario.Insignias.Add(
                    nombreInsignia
                );
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Services
{
    public class DatosPruebaService
    {
        private readonly JsonRepository<Usuario>
            _usuarioRepository;

        private readonly JsonRepository<Quiniela>
            _quinielaRepository;

        private readonly JsonRepository<Pronostico>
            _pronosticoRepository;

        private readonly JsonRepository<Partido>
            _partidoRepository;

        public DatosPruebaService()
        {
            _usuarioRepository =
                new JsonRepository<Usuario>(
                    RutaDatosService.ObtenerRuta(
                        "usuarios.json"
                    )
                );

            _quinielaRepository =
                new JsonRepository<Quiniela>(
                    RutaDatosService.ObtenerRuta(
                        "quinielas.json"
                    )
                );

            _pronosticoRepository =
                new JsonRepository<Pronostico>(
                    RutaDatosService.ObtenerRuta(
                        "pronosticos.json"
                    )
                );

            _partidoRepository =
                new JsonRepository<Partido>(
                    RutaDatosService.ObtenerRuta(
                        "partidos.json"
                    )
                );
        }

        public string GenerarDatosMinimos()
        {
            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            List<Partido> partidos =
                _partidoRepository
                    .ObtenerTodos()
                    .OrderBy(partido =>
                        partido.FechaHora
                    )
                    .ToList();

            GenerarUsuarios(usuarios);

            GenerarQuinielas(
                quinielas,
                usuarios
            );

            GenerarPronosticos(
                pronosticos,
                usuarios,
                partidos
            );

            _usuarioRepository.GuardarTodos(
                usuarios
            );

            _quinielaRepository.GuardarTodos(
                quinielas
            );

            _pronosticoRepository.GuardarTodos(
                pronosticos
            );

            return
                $"Usuarios registrados: {usuarios.Count}\n" +
                $"Quinielas privadas registradas: " +
                $"{ContarQuinielasPrivadas(quinielas)}\n" +
                $"Pronósticos registrados: {pronosticos.Count}";
        }

        private void GenerarUsuarios(
            List<Usuario> usuarios)
        {
            string[] paisesPreferidos =
            {
                "Argentina",
                "Australia",
                "Arabia Saudita",
                "Austria",
                "Bélgica",
                "Bosnia y Herzegovina",
                "Brasil",
                "Cabo Verde",
                "Canadá",
                "Colombia",
                "Congo DR",
                "Corea del Sur",
                "Costa de Marfil",
                "Croacia",
                "Curazao",
                "Ecuador",
                "Egipto",
                "Escocia",
                "Eslovaquia",
                "España",
                "Estados Unidos",
                "Francia",
                "Gales",
                "Ghana",
                "Haití",
                "Inglaterra",
                "Irán",
                "Irak",
                "Japón",
                "Jordania",
                "Marruecos",
                "México",
                "Noruega",
                "Nueva Zelanda",
                "Países Bajos",
                "Panamá",
                "Paraguay",
                "Portugal",
                "Qatar",
                "República Checa",
                "Senegal",
                "Sudáfrica",
                "Suecia",
                "Suiza",
                "Túnez",
                "Turquía",
                "Uruguay",
                "Uzbekistán"
            };

            int consecutivo = 1;

            while (usuarios.Count < 40)
            {
                string nombre =
                    $"Participante {consecutivo:00}";

                bool nombreExiste =
                    usuarios.Any(usuario =>
                        usuario.Nombre.Equals(
                            nombre,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );

                if (nombreExiste)
                {
                    consecutivo++;
                    continue;
                }

                int nuevoId = usuarios.Count == 0
                    ? 1
                    : usuarios.Max(usuario =>
                        usuario.Id
                    ) + 1;

                string pais =
                    paisesPreferidos[
                        usuarios.Count %
                        paisesPreferidos.Length
                    ];

                Usuario nuevoUsuario =
                    new Usuario
                    {
                        Id = nuevoId,
                        Nombre = nombre,
                        PaisPreferido = pais,
                        Puntos = 0
                    };

                usuarios.Add(nuevoUsuario);

                consecutivo++;
            }
        }

        private void GenerarQuinielas(
            List<Quiniela> quinielas,
            List<Usuario> usuarios)
        {
            string[] nombresQuinielas =
            {
                "Amigos del Mundial",
                "Expertos del Fútbol",
                "Quiniela Tica",
                "Fanáticos 2026",
                "Reto de Campeones"
            };

            for (int indice = 0;
                 indice < nombresQuinielas.Length;
                 indice++)
            {
                if (ContarQuinielasPrivadas(
                    quinielas
                ) >= 5)
                {
                    break;
                }

                string nombre =
                    nombresQuinielas[indice];

                bool yaExiste =
                    quinielas.Any(quiniela =>
                        quiniela.Nombre.Equals(
                            nombre,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );

                if (yaExiste)
                {
                    continue;
                }

                List<int> integrantes =
                    new List<int>();

                int inicio =
                    (indice * 7) %
                    usuarios.Count;

                for (int posicion = 0;
                     posicion < 12;
                     posicion++)
                {
                    int indiceUsuario =
                        (inicio + posicion) %
                        usuarios.Count;

                    integrantes.Add(
                        usuarios[indiceUsuario].Id
                    );
                }

                int nuevoId = quinielas.Count == 0
                    ? 1
                    : quinielas.Max(quiniela =>
                        quiniela.Id
                    ) + 1;

                Quiniela nuevaQuiniela =
                    new Quiniela
                    {
                        Id = nuevoId,
                        Nombre = nombre,
                        Descripcion =
                            "Quiniela privada generada para pruebas.",
                        Tipo = "Privada",
                        IntegrantesIds =
                            integrantes
                                .Distinct()
                                .ToList()
                    };

                quinielas.Add(nuevaQuiniela);
            }
        }

        private int ContarQuinielasPrivadas(
            List<Quiniela> quinielas)
        {
            return quinielas.Count(quiniela =>
                quiniela.Tipo.Equals(
                    "Privada",
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        private void GenerarPronosticos(
            List<Pronostico> pronosticos,
            List<Usuario> usuarios,
            List<Partido> partidos)
        {
            Random random =
                new Random(2026);

            int usuarioSinPronosticosId =
                usuarios
                    .Max(usuario =>
                        usuario.Id
                    );

            List<Usuario> usuariosConPronosticos =
                usuarios
                    .Where(usuario =>
                        usuario.Id !=
                        usuarioSinPronosticosId
                    )
                    .ToList();

            List<Partido> partidosParaPronosticar =
                partidos
                    .Take(12)
                    .ToList();

            foreach (Usuario usuario
                     in usuariosConPronosticos)
            {
                foreach (Partido partido
                         in partidosParaPronosticar)
                {
                    bool yaExiste =
                        pronosticos.Any(pronostico =>
                            pronostico.UsuarioId ==
                            usuario.Id &&
                            pronostico.PartidoId ==
                            partido.Id
                        );

                    if (yaExiste)
                    {
                        continue;
                    }

                    int nuevoId =
                        pronosticos.Count == 0
                            ? 1
                            : pronosticos.Max(
                                pronostico =>
                                    pronostico.Id
                            ) + 1;

                    Pronostico nuevoPronostico =
                        new Pronostico
                        {
                            Id = nuevoId,
                            UsuarioId =
                                usuario.Id,

                            PartidoId =
                                partido.Id,

                            GolesLocalPronosticados =
                                random.Next(0, 4),

                            GolesVisitantePronosticados =
                                random.Next(0, 4),

                            FechaRegistro =
                                partido.FechaHora
                                    .AddDays(-2)
                                    .AddHours(
                                        random.Next(0, 10)
                                    ),

                            PuntosObtenidos =
                                null
                        };

                    pronosticos.Add(
                        nuevoPronostico
                    );
                }
            }
        }
    }
}

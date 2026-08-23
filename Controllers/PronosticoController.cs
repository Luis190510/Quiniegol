using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class PronosticoController
    {
        private readonly JsonRepository<Pronostico>
            _pronosticoRepository;

        private readonly UsuarioController
            _usuarioController;

        private readonly PartidoController
            _partidoController;

        private readonly FechaSimuladaService
            _fechaService;

        public PronosticoController()
        {
            string rutaArchivo =
                RutaDatosService.ObtenerRuta(
                   "pronosticos.json"
            );

            _pronosticoRepository =
                new JsonRepository<Pronostico>(
                    rutaArchivo
                );

            _usuarioController =
                new UsuarioController();

            _partidoController =
                new PartidoController();

            _fechaService =
                FechaSimuladaService.Instancia;
        }

        public List<Pronostico> ObtenerPronosticos()
        {
            Usuario usuarioActual = SesionUsuarioService.UsuarioActual;
            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            return SesionUsuarioService.EsAdministrador
                ? pronosticos
                : pronosticos
                    .Where(pronostico =>
                        pronostico.UsuarioId == usuarioActual.Id)
                    .ToList();
        }

        public void RegistrarPronostico(
            int usuarioId,
            int partidoId,
            int golesLocal,
            int golesVisitante,
            IEnumerable<string>? goleadoresLocal = null,
            IEnumerable<string>? goleadoresVisitante = null)
        {
            Usuario usuarioActual = SesionUsuarioService.UsuarioActual;

            if (SesionUsuarioService.EsAdministrador)
            {
                throw new UnauthorizedAccessException(
                    "El administrador no participa en los pronósticos."
                );
            }

            if (usuarioId != usuarioActual.Id)
            {
                throw new UnauthorizedAccessException(
                    "No puede registrar un pronóstico a nombre de otra persona."
                );
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar un usuario."
                );
            }

            if (partidoId <= 0)
            {
                throw new ArgumentException(
                    "Debe seleccionar un partido."
                );
            }

            if (golesLocal < 0 ||
                golesVisitante < 0)
            {
                throw new ArgumentException(
                    "Los goles pronosticados no pueden ser negativos."
                );
            }

            bool usuarioExiste =
                _usuarioController
                    .ObtenerUsuarios()
                    .Any(usuario =>
                        usuario.Id == usuarioId);

            if (!usuarioExiste)
            {
                throw new InvalidOperationException(
                    "No se encontró el usuario seleccionado."
                );
            }

            Partido? partido =
                _partidoController
                    .ObtenerPartidos()
                    .FirstOrDefault(
                        partidoActual =>
                            partidoActual.Id == partidoId
                    );

            if (partido == null)
            {
                throw new InvalidOperationException(
                    "No se encontró el partido seleccionado."
                );
            }

            if (_fechaService.FechaActual >=
                partido.FechaHora)
            {
                throw new InvalidOperationException(
                    "El partido ya inició. No se permiten pronósticos."
                );
            }

            List<Pronostico> pronosticos =
                _pronosticoRepository.ObtenerTodos();

            bool pronosticoRepetido =
                pronosticos.Any(
                    pronostico =>
                        pronostico.UsuarioId == usuarioId &&
                        pronostico.PartidoId == partidoId
                );

            if (pronosticoRepetido)
            {
                throw new InvalidOperationException(
                    "El usuario ya registró un pronóstico para este partido."
                );
            }

            int nuevoId = pronosticos.Count == 0
                ? 1
                : pronosticos.Max(
                    pronostico => pronostico.Id
                ) + 1;

            Pronostico nuevoPronostico = new()
            {
                Id = nuevoId,
                UsuarioId = usuarioId,
                PartidoId = partidoId,
                GolesLocalPronosticados =
                    golesLocal,
                GolesVisitantePronosticados =
                    golesVisitante,
                FechaRegistro = _fechaService.FechaActual,
                PuntosObtenidos = null,
                GoleadoresLocalPronosticados =
                    GoleadoresPronosticoService.Normalizar(goleadoresLocal),
                GoleadoresVisitantePronosticados =
                    GoleadoresPronosticoService.Normalizar(goleadoresVisitante),
                GoleadoresConfirmados = true
            };

            pronosticos.Add(nuevoPronostico);

            _pronosticoRepository.GuardarTodos(
                pronosticos
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class QuinielaController
    {
        private readonly JsonRepository<Quiniela>
            _quinielaRepository;

        private readonly UsuarioController
            _usuarioController;

        public QuinielaController()
        {
            string rutaArchivo =
                RutaDatosService.ObtenerRuta(
                    "quinielas.json"
                );

            _quinielaRepository =
                new JsonRepository<Quiniela>(
                    rutaArchivo
                );

            _usuarioController =
                new UsuarioController();
        }

        public List<Quiniela> ObtenerQuinielas()
        {
            return _quinielaRepository
                .ObtenerTodos()
                .OrderBy(quiniela =>
                    quiniela.Nombre
                )
                .ToList();
        }

        public void CrearQuiniela(
            string nombre,
            string descripcion,
            List<int> integrantesIds)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException(
                    "Debe escribir el nombre de la quiniela."
                );
            }

            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            bool nombreRepetido =
                quinielas.Any(quiniela =>
                    quiniela.Nombre.Equals(
                        nombre.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            if (nombreRepetido)
            {
                throw new InvalidOperationException(
                    "Ya existe una quiniela con ese nombre."
                );
            }

            List<Usuario> usuarios =
                _usuarioController.ObtenerUsuarios();

            List<int> integrantesSinRepetir =
                integrantesIds
                    .Distinct()
                    .ToList();

            bool existeUsuarioInvalido =
                integrantesSinRepetir.Any(
                    usuarioId =>
                        !usuarios.Any(usuario =>
                            usuario.Id == usuarioId
                        )
                );

            if (existeUsuarioInvalido)
            {
                throw new InvalidOperationException(
                    "Uno de los usuarios seleccionados no existe."
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
                    Nombre = nombre.Trim(),
                    Descripcion =
                        descripcion.Trim(),
                    Tipo = "Privada",
                    IntegrantesIds =
                        integrantesSinRepetir
                };

            quinielas.Add(nuevaQuiniela);

            _quinielaRepository.GuardarTodos(
                quinielas
            );
        }

        public void AgregarIntegrante(
            int quinielaId,
            int usuarioId)
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            Quiniela? quiniela =
                quinielas.FirstOrDefault(
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

            bool usuarioExiste =
                _usuarioController
                    .ObtenerUsuarios()
                    .Any(usuario =>
                        usuario.Id == usuarioId
                    );

            if (!usuarioExiste)
            {
                throw new InvalidOperationException(
                    "No se encontró el usuario."
                );
            }

            if (quiniela.IntegrantesIds.Contains(
                usuarioId
            ))
            {
                throw new InvalidOperationException(
                    "El usuario ya pertenece a esta quiniela."
                );
            }

            quiniela.IntegrantesIds.Add(
                usuarioId
            );

            _quinielaRepository.GuardarTodos(
                quinielas
            );
        }

        public void EliminarIntegrante(
            int quinielaId,
            int usuarioId)
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            Quiniela? quiniela =
                quinielas.FirstOrDefault(
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

            if (!quiniela.IntegrantesIds.Contains(
                usuarioId
            ))
            {
                throw new InvalidOperationException(
                    "El usuario no pertenece a esta quiniela."
                );
            }

            quiniela.IntegrantesIds.Remove(
                usuarioId
            );

            _quinielaRepository.GuardarTodos(
                quinielas
            );
        }

        public List<Usuario> ObtenerIntegrantes(
            int quinielaId)
        {
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

            return _usuarioController
                .ObtenerUsuarios()
                .Where(usuario =>
                    quiniela.IntegrantesIds.Contains(
                        usuario.Id
                    )
                )
                .OrderBy(usuario =>
                    usuario.Nombre
                )
                .ToList();
        }

        public void EliminarQuiniela(
            int quinielaId)
        {
            List<Quiniela> quinielas =
                _quinielaRepository.ObtenerTodos();

            Quiniela? quiniela =
                quinielas.FirstOrDefault(
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

            quinielas.Remove(quiniela);

            _quinielaRepository.GuardarTodos(
                quinielas
            );
        }
    }
}
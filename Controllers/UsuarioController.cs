using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Controllers
{
    public class UsuarioController
    {
        private readonly JsonRepository<Usuario> _usuarioRepository;

        public UsuarioController()
        {
            string rutaArchivo = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "usuarios.json"
            );

            _usuarioRepository =
                new JsonRepository<Usuario>(rutaArchivo);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _usuarioRepository.ObtenerTodos();
        }

        public void RegistrarUsuario(
            string nombre,
            string paisPreferido)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException(
                    "Debe ingresar el nombre del usuario."
                );
            }

            if (string.IsNullOrWhiteSpace(paisPreferido))
            {
                throw new ArgumentException(
                    "Debe seleccionar un país."
                );
            }

            List<Usuario> usuarios =
                _usuarioRepository.ObtenerTodos();

            bool usuarioRepetido = usuarios.Any(
                usuario => usuario.Nombre.Equals(
                    nombre.Trim(),
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (usuarioRepetido)
            {
                throw new InvalidOperationException(
                    "Ya existe un usuario con ese nombre."
                );
            }

            int nuevoId = usuarios.Count == 0
                ? 1
                : usuarios.Max(usuario => usuario.Id) + 1;

            Usuario nuevoUsuario = new()
            {
                Id = nuevoId,
                Nombre = nombre.Trim(),
                PaisPreferido = paisPreferido.Trim(),
                Puntos = 0,
                Insignias = new List<string>()
            };

            usuarios.Add(nuevoUsuario);

            _usuarioRepository.GuardarTodos(usuarios);
        }
    }
}
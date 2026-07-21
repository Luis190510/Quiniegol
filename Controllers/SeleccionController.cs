using System;
using System.Collections.Generic;
using System.Text;
using Quiniegol.Models;
using Quiniegol.Repositories;

namespace Quiniegol.Controllers
{
    public class SeleccionController
    {
        private readonly JsonRepository<Seleccion> _seleccionRepository;

        public SeleccionController()
        {
            string rutaArchivo = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "selecciones.json"
            );

            _seleccionRepository =
                new JsonRepository<Seleccion>(rutaArchivo);
        }

        public List<Seleccion> ObtenerSelecciones()
        {
            return _seleccionRepository.ObtenerTodos();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Controllers
{
    public class SeleccionController
    {
        private readonly JsonRepository<Seleccion> _seleccionRepository;

        public SeleccionController()
        {
            string rutaArchivo =
                RutaDatosService.ObtenerRuta(
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
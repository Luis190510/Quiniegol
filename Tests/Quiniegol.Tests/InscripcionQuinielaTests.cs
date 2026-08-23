using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class InscripcionQuinielaTests
    {
        [TestCleanup]
        public void LimpiarSesion()
        {
            SesionUsuarioService.CerrarSesion();
        }

        [TestMethod]
        public void CualquierParticipantePuedeElegirQuinielaDisponibleYUnirse()
        {
            string carpeta = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-inscripcion-{Guid.NewGuid():N}");
            Directory.CreateDirectory(carpeta);

            try
            {
                JsonRepository<Usuario> usuarios = new(
                    Path.Combine(carpeta, "usuarios.json"));
                JsonRepository<Quiniela> quinielas = new(
                    Path.Combine(carpeta, "quinielas.json"));
                JsonRepository<Pronostico> pronosticos = new(
                    Path.Combine(carpeta, "pronosticos.json"));

                Usuario participante = new()
                {
                    Id = 42,
                    Nombre = "Participante Nuevo",
                    NombreUsuario = "participante.nuevo",
                    Correo = "participante@example.com",
                    ContrasenaHash = ContrasenaService.CrearHash("Clave123!"),
                    Rol = RolUsuario.Usuario,
                    Activo = true
                };
                usuarios.GuardarTodos(new List<Usuario>
                {
                    new()
                    {
                        Id = 1,
                        Nombre = "Creador",
                        NombreUsuario = "creador",
                        Correo = "creador@example.com",
                        ContrasenaHash = ContrasenaService.CrearHash("Clave123!"),
                        Rol = RolUsuario.Usuario,
                        Activo = true
                    },
                    participante
                });
                quinielas.GuardarTodos(new List<Quiniela>
                {
                    new()
                    {
                        Id = 7,
                        Nombre = "Amigos",
                        Descripcion = "Dato privado",
                        CreadorUsuarioId = 1,
                        IntegrantesIds = new List<int> { 1 }
                    }
                });
                pronosticos.GuardarTodos(new List<Pronostico>());

                UsuarioController usuarioController = new(usuarios);
                SesionUsuarioService.IniciarSesion(participante);
                QuinielaController controller = new(
                    quinielas,
                    pronosticos,
                    usuarioController);

                Assert.AreEqual(0, controller.ObtenerQuinielas().Count);
                QuinielaDisponibleItem disponible =
                    controller.ObtenerQuinielasDisponibles().Single();
                Assert.AreEqual("Amigos", disponible.Nombre);

                controller.UnirseAQuiniela(disponible.QuinielaId);

                Assert.AreEqual(1, controller.ObtenerQuinielas().Count);
                Assert.IsTrue(
                    quinielas.ObtenerTodos().Single().IntegrantesIds
                        .Contains(participante.Id));
            }
            finally
            {
                SesionUsuarioService.CerrarSesion();
                Directory.Delete(carpeta, true);
            }
        }
    }
}

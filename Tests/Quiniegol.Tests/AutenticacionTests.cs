using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class AutenticacionTests
    {
        [TestCleanup]
        public void LimpiarSesion()
        {
            SesionUsuarioService.CerrarSesion();
        }

        [TestMethod]
        public void HashAceptaContrasenaCorrectaYRechazaLaIncorrecta()
        {
            string hash = ContrasenaService.CrearHash("ClaveSegura123!");

            Assert.IsTrue(
                ContrasenaService.Verificar("ClaveSegura123!", hash)
            );
            Assert.IsFalse(
                ContrasenaService.Verificar("OtraClave", hash)
            );
            Assert.AreNotEqual("ClaveSegura123!", hash);
        }

        [TestMethod]
        public void LoginPermiteUsuarioOCorreoConCredencialesValidas()
        {
            string rutaTemporal = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-usuarios-{Guid.NewGuid():N}.json"
            );

            try
            {
                JsonRepository<Usuario> repositorio = new(rutaTemporal);
                repositorio.GuardarTodos(new List<Usuario>
                {
                    new()
                    {
                        Id = 1,
                        Nombre = "Persona de prueba",
                        NombreUsuario = "persona",
                        Correo = "persona@example.com",
                        ContrasenaHash =
                            ContrasenaService.CrearHash("ClaveSegura123!"),
                        Rol = RolUsuario.Usuario,
                        Activo = true,
                        PaisPreferido = "Costa Rica"
                    }
                });

                LoginController login = new(
                    new UsuarioController(repositorio)
                );

                Assert.IsNotNull(
                    login.Autenticar("persona", "ClaveSegura123!")
                );
                Assert.IsNotNull(
                    login.Autenticar(
                        "PERSONA@EXAMPLE.COM",
                        "ClaveSegura123!"
                    )
                );
                Assert.IsNull(
                    login.Autenticar("persona", "incorrecta")
                );
            }
            finally
            {
                File.Delete(rutaTemporal);
            }
        }

        [TestMethod]
        public void OperacionAdministrativaRechazaUsuarioRegular()
        {
            SesionUsuarioService.IniciarSesion(new Usuario
            {
                Id = 7,
                Nombre = "Participante",
                Rol = RolUsuario.Usuario,
                Activo = true
            });

            Assert.ThrowsException<UnauthorizedAccessException>(
                SesionUsuarioService.ExigirAdministrador
            );
        }

        [TestMethod]
        public void UsuarioRegularNoPuedeAjustarFechaSimulada()
        {
            SesionUsuarioService.IniciarSesion(new Usuario
            {
                Id = 7,
                Nombre = "Participante",
                Rol = RolUsuario.Usuario,
                Activo = true
            });
            DateTime fechaAnterior =
                FechaSimuladaService.Instancia.FechaActual;
            Assert.ThrowsException<UnauthorizedAccessException>(() =>
                FechaSimuladaService.Instancia.CambiarFecha(
                    new DateTime(2026, 6, 10, 12, 0, 0)));
            Assert.AreEqual(
                fechaAnterior,
                FechaSimuladaService.Instancia.FechaActual);
        }

        [TestMethod]
        public void AdministradorPuedeAjustarFechaSimulada()
        {
            SesionUsuarioService.IniciarSesion(new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Rol = RolUsuario.Administrador,
                Activo = true
            });
            DateTime fechaAnterior =
                FechaSimuladaService.Instancia.FechaActual;
            DateTime fechaPrueba = new(2026, 6, 10, 12, 0, 0);

            try
            {
                FechaSimuladaService.Instancia.CambiarFecha(fechaPrueba);

                Assert.AreEqual(
                    fechaPrueba,
                    FechaSimuladaService.Instancia.FechaActual);
            }
            finally
            {
                FechaSimuladaService.Instancia.CambiarFecha(fechaAnterior);
            }
        }

        [TestMethod]
        public void MigracionConservaParticipantesYCreaAdministradorSeparado()
        {
            string rutaTemporal = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-migracion-{Guid.NewGuid():N}.json"
            );

            try
            {
                JsonRepository<Usuario> repositorio = new(rutaTemporal);
                repositorio.GuardarTodos(new List<Usuario>
                {
                    new()
                    {
                        Id = 1,
                        Nombre = "Luis Alonso",
                        PaisPreferido = "España"
                    }
                });

                UsuarioController usuarios = new(repositorio);
                List<Usuario> cuentas = usuarios.ObtenerUsuarios();
                Usuario participante = cuentas.Single(cuenta =>
                    cuenta.Id == 1);
                Usuario administrador = cuentas.Single(cuenta =>
                    cuenta.Rol == RolUsuario.Administrador);

                Assert.AreEqual(RolUsuario.Usuario, participante.Rol);
                Assert.AreEqual("luis.alonso", participante.NombreUsuario);
                Assert.AreNotEqual(participante.Id, administrador.Id);
                Assert.AreEqual("admin", administrador.NombreUsuario);
            }
            finally
            {
                File.Delete(rutaTemporal);
            }
        }

        [TestMethod]
        public void RegistroPublicoUsaLaContrasenaElegidaYCreaParticipante()
        {
            string rutaTemporal = Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-registro-{Guid.NewGuid():N}.json"
            );

            try
            {
                LoginController login = new(
                    new UsuarioController(
                        new JsonRepository<Usuario>(rutaTemporal))
                );

                Usuario creado = login.RegistrarCuenta(
                    "María Rojas",
                    "Costa Rica",
                    "maria.rojas",
                    "maria@example.com",
                    "MiClaveElegida!"
                );

                Assert.AreEqual(RolUsuario.Usuario, creado.Rol);
                Assert.IsFalse(creado.DebeCambiarContrasena);
                Assert.IsNotNull(
                    login.Autenticar("maria.rojas", "MiClaveElegida!")
                );
                Assert.IsNull(
                    login.Autenticar(
                        "maria.rojas",
                        UsuarioController.ContrasenaTemporalUsuario)
                );
            }
            finally
            {
                File.Delete(rutaTemporal);
            }
        }
    }
}

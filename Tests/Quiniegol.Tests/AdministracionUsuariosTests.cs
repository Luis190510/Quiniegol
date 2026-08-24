using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class AdministracionUsuariosTests
    {
        [TestCleanup]
        public void LimpiarSesion()
        {
            SesionUsuarioService.CerrarSesion();
        }

        [TestMethod]
        public void AdministradorPuedeDesactivarYReactivarUnaCuenta()
        {
            string ruta = CrearRutaTemporal();
            try
            {
                UsuarioController usuarios = CrearControlador(ruta);
                LoginController login = new(usuarios);
                Usuario administrador = usuarios.ObtenerUsuarios().Single(
                    usuario => usuario.Rol == RolUsuario.Administrador);
                Usuario participante = usuarios.ObtenerUsuarios().Single(
                    usuario => usuario.Rol == RolUsuario.Usuario);
                SesionUsuarioService.IniciarSesion(administrador);

                usuarios.CambiarEstadoCuenta(participante.Id, activar: false);
                Assert.IsNull(login.Autenticar("participante", "ClaveAnterior!"));

                usuarios.CambiarEstadoCuenta(participante.Id, activar: true);
                Assert.IsNotNull(login.Autenticar("participante", "ClaveAnterior!"));
            }
            finally
            {
                File.Delete(ruta);
            }
        }

        [TestMethod]
        public void ParticipanteNoPuedeAdministrarCuentas()
        {
            string ruta = CrearRutaTemporal();
            try
            {
                UsuarioController usuarios = CrearControlador(ruta);
                Usuario participante = usuarios.ObtenerUsuarios().Single(
                    usuario => usuario.Rol == RolUsuario.Usuario);
                SesionUsuarioService.IniciarSesion(participante);

                Assert.ThrowsException<UnauthorizedAccessException>(
                    usuarios.ObtenerUsuariosParaAdministracion);
                Assert.ThrowsException<UnauthorizedAccessException>(
                    () => usuarios.RestablecerContrasena(participante.Id));
                Assert.ThrowsException<UnauthorizedAccessException>(
                    () => usuarios.CambiarEstadoCuenta(participante.Id, activar: false));
            }
            finally
            {
                File.Delete(ruta);
            }
        }

        [TestMethod]
        public void AdministradorNoPuedeDesactivarSuPropiaCuenta()
        {
            string ruta = CrearRutaTemporal();
            try
            {
                UsuarioController usuarios = CrearControlador(ruta);
                Usuario administrador = usuarios.ObtenerUsuarios().Single(
                    usuario => usuario.Rol == RolUsuario.Administrador);
                SesionUsuarioService.IniciarSesion(administrador);

                Assert.ThrowsException<InvalidOperationException>(
                    () => usuarios.CambiarEstadoCuenta(administrador.Id, activar: false));
            }
            finally
            {
                File.Delete(ruta);
            }
        }

        [TestMethod]
        public void RestablecimientoObligaACrearUnaContrasenaNueva()
        {
            string ruta = CrearRutaTemporal();
            try
            {
                UsuarioController usuarios = CrearControlador(ruta);
                LoginController login = new(usuarios);
                Usuario administrador = usuarios.ObtenerUsuarios().Single(
                    usuario => usuario.Rol == RolUsuario.Administrador);
                Usuario participante = usuarios.ObtenerUsuarios().Single(
                    usuario => usuario.Rol == RolUsuario.Usuario);
                SesionUsuarioService.IniciarSesion(administrador);

                string temporal = usuarios.RestablecerContrasena(participante.Id);
                Assert.AreEqual(UsuarioController.ContrasenaTemporalUsuario, temporal);
                Assert.IsNull(login.Autenticar("participante", "ClaveAnterior!"));

                Usuario autenticado = login.Autenticar("participante", temporal)!;
                Assert.IsTrue(autenticado.DebeCambiarContrasena);
                Usuario actualizado = login.CompletarCambioObligatorio(
                    participante.Id,
                    temporal,
                    "ClaveDefinitiva!");

                Assert.IsFalse(actualizado.DebeCambiarContrasena);
                Assert.IsNull(login.Autenticar("participante", temporal));
                Assert.IsNotNull(login.Autenticar("participante", "ClaveDefinitiva!"));
            }
            finally
            {
                File.Delete(ruta);
            }
        }

        private static string CrearRutaTemporal()
        {
            return Path.Combine(
                Path.GetTempPath(),
                $"quiniegol-administracion-{Guid.NewGuid():N}.json");
        }

        private static UsuarioController CrearControlador(string ruta)
        {
            JsonRepository<Usuario> repositorio = new(ruta);
            repositorio.GuardarTodos(new List<Usuario>
            {
                new()
                {
                    Id = 1,
                    Nombre = "Administrador",
                    NombreUsuario = "admin",
                    Correo = "admin@quinegol.local",
                    ContrasenaHash = ContrasenaService.CrearHash("ClaveAdmin!"),
                    Rol = RolUsuario.Administrador,
                    Activo = true,
                    PaisPreferido = "Sin definir"
                },
                new()
                {
                    Id = 2,
                    Nombre = "Participante",
                    NombreUsuario = "participante",
                    Correo = "participante@example.com",
                    ContrasenaHash = ContrasenaService.CrearHash("ClaveAnterior!"),
                    Rol = RolUsuario.Usuario,
                    Activo = true,
                    PaisPreferido = "Costa Rica"
                }
            });
            return new UsuarioController(repositorio);
        }
    }
}

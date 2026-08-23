using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    [TestClass]
    public class AccesoQuinielaTests
    {
        private readonly Quiniela _quiniela = new()
        {
            Id = 3,
            Nombre = "Privada",
            CreadorUsuarioId = 10,
            IntegrantesIds = new List<int> { 10, 20 }
        };

        [TestMethod]
        public void IntegrantePuedeConsultarQuiniela()
        {
            Usuario integrante = CrearUsuario(20, RolUsuario.Usuario);

            Assert.IsTrue(
                AccesoQuinielaService.PuedeConsultar(_quiniela, integrante)
            );
        }

        [TestMethod]
        public void UsuarioAjenoNoPuedeConsultarQuiniela()
        {
            Usuario ajeno = CrearUsuario(99, RolUsuario.Usuario);

            Assert.IsFalse(
                AccesoQuinielaService.PuedeConsultar(_quiniela, ajeno)
            );
            Assert.ThrowsException<UnauthorizedAccessException>(() =>
                AccesoQuinielaService.ExigirConsulta(_quiniela, ajeno)
            );
        }

        [TestMethod]
        public void AdministradorPuedeConsultarCualquierQuiniela()
        {
            Usuario administrador =
                CrearUsuario(500, RolUsuario.Administrador);

            Assert.IsTrue(
                AccesoQuinielaService.PuedeConsultar(
                    _quiniela,
                    administrador)
            );
        }

        [TestMethod]
        public void UsuarioAjenoPuedeUnirseSinObtenerAccesoAntes()
        {
            Usuario ajeno = CrearUsuario(99, RolUsuario.Usuario);

            Assert.IsFalse(
                AccesoQuinielaService.PuedeConsultar(_quiniela, ajeno)
            );
            Assert.IsTrue(
                AccesoQuinielaService.PuedeUnirse(_quiniela, ajeno)
            );
        }

        [TestMethod]
        public void IntegranteYAdministradorNoNecesitanAutoInscribirse()
        {
            Usuario integrante = CrearUsuario(20, RolUsuario.Usuario);
            Usuario administrador =
                CrearUsuario(500, RolUsuario.Administrador);

            Assert.IsFalse(
                AccesoQuinielaService.PuedeUnirse(_quiniela, integrante)
            );
            Assert.IsFalse(
                AccesoQuinielaService.PuedeUnirse(_quiniela, administrador)
            );
        }

        private static Usuario CrearUsuario(int id, RolUsuario rol)
        {
            return new Usuario
            {
                Id = id,
                Nombre = $"Usuario {id}",
                Rol = rol,
                Activo = true
            };
        }
    }
}

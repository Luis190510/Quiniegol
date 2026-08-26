using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Repositories;
using Quiniegol.Services;

namespace Quiniegol.Tests
{
    /// <summary>
    /// Test class for <see cref="QuinielaController"/>.
    /// </summary>
    [TestClass]
    public class QuinielaControllerTest
    {
        private string _folder = "";
        private JsonRepository<Quiniela> _pools = null!;
        private JsonRepository<Pronostico> _predictions = null!;
        private JsonRepository<Usuario> _users = null!;
        private QuinielaController _controller = null!;

        [TestInitialize]
        public void PrepareController()
        {
            _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _pools = new JsonRepository<Quiniela>(Path.Combine(_folder, "quinielas.json"));
            _predictions = new JsonRepository<Pronostico>(Path.Combine(_folder, "pronosticos.json"));
            _users = new JsonRepository<Usuario>(Path.Combine(_folder, "usuarios.json"));
            _users.GuardarTodos(CreateUsers());
            _controller = new QuinielaController(
                _pools,
                _predictions,
                new UsuarioController(_users));
            SesionUsuarioService.IniciarSesion(CreateUsers()[1]);
        }

        [TestCleanup]
        public void RemoveFiles()
        {
            SesionUsuarioService.CerrarSesion();
            if (Directory.Exists(_folder))
            {
                Directory.Delete(_folder, true);
            }
        }

        /// <summary>
        /// A participant should create a private pool as its owner.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldCreatePrivatePool()
        {
            // Arrange
            var members = new List<int> { 3 };

            // Act
            _controller.CrearQuiniela("Amigos", "Prueba", members);

            // Assert
            var saved = _pools.ObtenerTodos().Single();
            Assert.AreEqual(2, saved.CreadorUsuarioId);
            CollectionAssert.AreEquivalent(new List<int> { 2, 3 }, saved.IntegrantesIds);
        }

        /// <summary>
        /// A duplicated pool name should be rejected.
        /// </summary>
        [TestMethod]
        public void DuplicatedPoolNameShouldBeRejected()
        {
            // Arrange
            _controller.CrearQuiniela("Amigos", "", new List<int>());

            // Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                _controller.CrearQuiniela("amigos", "", new List<int>()));
        }

        /// <summary>
        /// A participant should see only pools where it is a member.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldSeeOnlyItsPools()
        {
            // Arrange
            SavePools(
                CreatePool(1, "Propia", 2, 2),
                CreatePool(2, "Ajena", 3, 3));

            // Act
            var result = _controller.ObtenerQuinielas();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Propia", result[0].Nombre);
        }

        /// <summary>
        /// A participant should join an available private pool.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldJoinAvailablePool()
        {
            // Arrange
            SavePools(CreatePool(1, "Trabajo", 3, 3));

            // Act
            _controller.UnirseAQuiniela(1);

            // Assert
            Assert.IsTrue(_pools.ObtenerTodos().Single().IntegrantesIds.Contains(2));
        }

        /// <summary>
        /// A participant should not join the same pool twice.
        /// </summary>
        [TestMethod]
        public void ParticipantShouldNotJoinTwice()
        {
            // Arrange
            SavePools(CreatePool(1, "Trabajo", 3, 2, 3));

            // Act and Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                _controller.UnirseAQuiniela(1));
        }

        /// <summary>
        /// The owner should add and remove another participant.
        /// </summary>
        [TestMethod]
        public void OwnerShouldManageMembers()
        {
            // Arrange
            SavePools(CreatePool(1, "Amigos", 2, 2));

            // Act
            _controller.AgregarIntegrante(1, 3);
            _controller.EliminarIntegrante(1, 3);

            // Assert
            CollectionAssert.AreEqual(
                new List<int> { 2 },
                _pools.ObtenerTodos().Single().IntegrantesIds);
        }

        /// <summary>
        /// A member should see the private prediction summary.
        /// </summary>
        [TestMethod]
        public void MemberShouldSeePrivateSummary()
        {
            // Arrange
            SavePools(CreatePool(1, "Amigos", 2, 2, 3));
            _predictions.GuardarTodos(new List<Pronostico>
            {
                new()
                {
                    Id = 1,
                    UsuarioId = 3,
                    GoleadoresConfirmados = true,
                    GoleadoresLocalPronosticados = new() { "Ana" }
                }
            });

            // Act
            var result = _controller.ObtenerResumenIntegrantes(1);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result.Single(item => item.Id == 3).PronosticosConGoleadores);
        }

        /// <summary>
        /// The administrator should delete any private pool.
        /// </summary>
        [TestMethod]
        public void AdministratorShouldDeleteAnyPool()
        {
            // Arrange
            SavePools(CreatePool(1, "Amigos", 2, 2));
            SesionUsuarioService.IniciarSesion(CreateUsers()[0]);

            // Act
            _controller.EliminarQuiniela(1);

            // Assert
            Assert.AreEqual(0, _pools.ObtenerTodos().Count);
        }

        private void SavePools(params Quiniela[] pools)
        {
            _pools.GuardarTodos(pools.ToList());
        }

        private static Quiniela CreatePool(
            int id, string name, int owner, params int[] members)
        {
            return new Quiniela
            {
                Id = id,
                Nombre = name,
                CreadorUsuarioId = owner,
                IntegrantesIds = members.ToList()
            };
        }

        private static List<Usuario> CreateUsers()
        {
            return new List<Usuario>
            {
                new()
                {
                    Id = 1, Nombre = "Administrador", NombreUsuario = "admin",
                    Correo = "admin@quinegol.com", ContrasenaHash = "hash",
                    Rol = RolUsuario.Administrador, Activo = true
                },
                new()
                {
                    Id = 2, Nombre = "Daniel", NombreUsuario = "daniel",
                    Correo = "daniel@quinegol.com", ContrasenaHash = "hash",
                    Rol = RolUsuario.Usuario, Activo = true
                },
                new()
                {
                    Id = 3, Nombre = "Luis", NombreUsuario = "luis",
                    Correo = "luis@quinegol.com", ContrasenaHash = "hash",
                    Rol = RolUsuario.Usuario, Activo = true
                }
            };
        }
    }
}

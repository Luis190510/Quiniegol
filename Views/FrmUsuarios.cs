using Quiniegol.Controllers;

namespace Quiniegol.Views
{
    /// <summary>Permite al administrador consultar y crear cuentas.</summary>
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioController _usuarioController;
        private readonly LoginController _loginController;

        public FrmUsuarios()
        {
            InitializeComponent();
            _usuarioController = new UsuarioController();
            _loginController = new LoginController(_usuarioController);
            CargarUsuarios();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            using FrmRegistroUsuario registro = new(_loginController);

            if (registro.ShowDialog(this) == DialogResult.OK)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = _usuarioController
                .ObtenerUsuarios()
                .Select(usuario => new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.NombreUsuario,
                    usuario.Correo,
                    usuario.Rol,
                    usuario.PaisPreferido,
                    usuario.Puntos,
                    usuario.Activo
                })
                .ToList();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>Solicita y valida las credenciales de acceso.</summary>
    public partial class FrmLogin : Form
    {
        private readonly LoginController _loginController;

        /// <summary>Crea la pantalla de inicio de sesión.</summary>
        /// <param name="loginController">Controlador de autenticación.</param>
        public FrmLogin(LoginController loginController)
        {
            _loginController = loginController ??
                throw new ArgumentNullException(nameof(loginController));

            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            lblError.ForeColor = Color.Firebrick;

            Usuario? usuario = _loginController.Autenticar(
                txtIdentificador.Text,
                txtContrasena.Text
            );

            if (usuario == null)
            {
                lblError.Text = "Usuario/correo o contraseña incorrectos.";
                txtContrasena.Clear();
                txtContrasena.Focus();
                return;
            }

            SesionUsuarioService.IniciarSesion(usuario);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            using FrmRegistroUsuario registro = new(_loginController);

            if (registro.ShowDialog(this) == DialogResult.OK)
            {
                txtIdentificador.Text = registro.NombreUsuarioRegistrado;
                txtContrasena.Clear();
                lblError.ForeColor = Color.DarkGreen;
                lblError.Text =
                    "Cuenta creada. Ingrese la contraseña que eligió.";
                txtContrasena.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

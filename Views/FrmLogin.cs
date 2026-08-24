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

            if (usuario.DebeCambiarContrasena)
            {
                using FrmCambioContrasena cambio = new(
                    _loginController,
                    usuario,
                    txtContrasena.Text);
                if (cambio.ShowDialog(this) != DialogResult.OK ||
                    cambio.UsuarioActualizado == null)
                {
                    lblError.Text =
                        "Debe cambiar la contraseña temporal para ingresar.";
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                    return;
                }

                usuario = cambio.UsuarioActualizado;
            }

            SesionUsuarioService.IniciarSesion(usuario);
            MostrarNotificacionesPendientes(usuario);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MostrarNotificacionesPendientes(Usuario usuario)
        {
            List<NotificacionPronosticoItem> pendientes =
                new NotificacionPronosticoController()
                    .ObtenerPendientes(usuario);
            if (pendientes.Count == 0)
            {
                return;
            }

            string detalle = string.Join(
                Environment.NewLine,
                pendientes.Select(pendiente =>
                    $"• {pendiente.FechaHora:dd/MM/yyyy HH:mm} - " +
                    pendiente.Partido));
            string encabezado = pendientes.Count == 1
                ? "Tiene 1 partido sin pronosticar en las próximas 24 horas:"
                : $"Tiene {pendientes.Count} partidos sin pronosticar " +
                  "en las próximas 24 horas:";

            MessageBox.Show(
                this,
                $"{encabezado}{Environment.NewLine}{Environment.NewLine}{detalle}" +
                $"{Environment.NewLine}{Environment.NewLine}" +
                "Las horas se calculan usando la fecha simulada.",
                "Pronósticos pendientes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

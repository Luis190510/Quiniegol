using Quiniegol.Controllers;
using Quiniegol.Models;

namespace Quiniegol.Views
{
    /// <summary>
    /// Solicita reemplazar una contraseña temporal antes de iniciar sesión.
    /// </summary>
    public partial class FrmCambioContraseña : Form
    {
        private readonly LoginController _loginController;
        private readonly Usuario _usuario;
        private readonly string _contrasenaTemporal;

        /// <summary>Cuenta actualizada después del cambio exitoso.</summary>
        public Usuario? UsuarioActualizado { get; private set; }

        public FrmCambioContraseña(
            LoginController loginController,
            Usuario usuario,
            string contrasenaTemporal)
        {
            _loginController = loginController ??
                throw new ArgumentNullException(nameof(loginController));
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            _contrasenaTemporal = contrasenaTemporal;
            InitializeComponent();
            lblCuenta.Text = $"Cuenta: {_usuario.NombreUsuario}";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNuevaContrasena.Text != txtConfirmacion.Text)
                {
                    throw new ArgumentException(
                        "La nueva contraseña y su confirmación no coinciden.");
                }

                UsuarioActualizado = _loginController.CompletarCambioObligatorio(
                    _usuario.Id,
                    _contrasenaTemporal,
                    txtNuevaContrasena.Text);
                MessageBox.Show(
                    "La contraseña fue actualizada correctamente.",
                    "Contraseña actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

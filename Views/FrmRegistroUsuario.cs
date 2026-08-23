using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>Recopila todos los datos de una nueva cuenta participante.</summary>
    public partial class FrmRegistroUsuario : Form
    {
        private readonly LoginController _loginController;

        /// <summary>Nombre que puede colocarse en el acceso tras registrarse.</summary>
        public string NombreUsuarioRegistrado { get; private set; } = "";

        public FrmRegistroUsuario(LoginController loginController)
        {
            _loginController = loginController ??
                throw new ArgumentNullException(nameof(loginController));

            InitializeComponent();
            cmbPais.DataSource = PaisesService.ObtenerTodos().ToList();
            cmbPais.SelectedIndex = -1;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtContrasena.Text != txtConfirmacion.Text)
                {
                    throw new ArgumentException(
                        "Las contraseñas ingresadas no coinciden."
                    );
                }

                Usuario usuario = _loginController.RegistrarCuenta(
                    txtNombre.Text,
                    cmbPais.Text,
                    txtNombreUsuario.Text,
                    txtCorreo.Text,
                    txtContrasena.Text
                );

                NombreUsuarioRegistrado = usuario.NombreUsuario;

                MessageBox.Show(
                    "La cuenta fue registrada. Ya puede iniciar sesión.",
                    "Registro completo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

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

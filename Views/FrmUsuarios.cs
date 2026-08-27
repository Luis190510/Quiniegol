using Quiniegol.Controllers;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>
    /// Permite al administrador registrar, activar, desactivar y restablecer cuentas.
    /// </summary>
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioController _usuarioController;
        private readonly LoginController _loginController;

        public FrmUsuarios()
        {
            InitializeComponent();
            SesionUsuarioService.ExigirAdministrador();
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
                .ObtenerUsuariosParaAdministracion()
                .Select(usuario => new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.NombreUsuario,
                    usuario.Correo,
                    usuario.Rol,
                    usuario.PaisPreferido,
                    usuario.Puntos,
                    usuario.Activo,
                    CambioPendiente = usuario.DebeCambiarContrasena
                })
                .ToList();

            dgvUsuarios.ClearSelection();
            ActualizarBotones();
        }

        private void btnRestablecer_Click(object sender, EventArgs e)
        {
            var seleccion = ObtenerSeleccion();
            if (!seleccion.HasValue)
            {
                MostrarAdvertencia("Debe seleccionar una cuenta.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Desea restablecer la contraseña de {seleccion.Value.Nombre}?",
                "Confirmar restablecimiento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                string temporal = _usuarioController.RestablecerContrasena(
                    seleccion.Value.Id);
                CargarUsuarios();
                MessageBox.Show(
                    $"Contraseña restablecida.\n\nContraseña temporal: {temporal}\n\n" +
                    "El usuario deberá cambiarla en el siguiente inicio de sesión.",
                    "Restablecimiento completo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarAdvertencia(ex.Message);
            }
        }

        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            var seleccion = ObtenerSeleccion();
            if (!seleccion.HasValue)
            {
                MostrarAdvertencia("Debe seleccionar una cuenta.");
                return;
            }

            bool activar = !seleccion.Value.Activo;
            string accion = activar ? "activar" : "desactivar";
            DialogResult confirmacion = MessageBox.Show(
                $"¿Desea {accion} la cuenta de {seleccion.Value.Nombre}?",
                $"Confirmar {accion}",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _usuarioController.CambiarEstadoCuenta(seleccion.Value.Id, activar);
                CargarUsuarios();
                MessageBox.Show(
                    activar
                        ? "La cuenta fue activada."
                        : "La cuenta fue desactivada y ya no puede iniciar sesión.",
                    "Estado actualizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarAdvertencia(ex.Message);
            }
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            var seleccion = ObtenerSeleccion();
            btnRestablecer.Enabled = seleccion.HasValue;
            btnCambiarEstado.Enabled = seleccion.HasValue;
            btnCambiarEstado.Text = seleccion?.Activo == false
                ? "Activar cuenta"
                : "Desactivar cuenta";
        }

        private (int Id, string Nombre, bool Activo)? ObtenerSeleccion()
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                return null;
            }

            DataGridViewRow fila = dgvUsuarios.SelectedRows[0];
            if (fila.Cells["Id"].Value == null || fila.Cells["Activo"].Value == null)
            {
                return null;
            }

            return (
                Convert.ToInt32(fila.Cells["Id"].Value),
                fila.Cells["Nombre"].Value?.ToString() ?? "la cuenta seleccionada",
                Convert.ToBoolean(fila.Cells["Activo"].Value));
        }

        private static void MostrarAdvertencia(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Gestión de usuarios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

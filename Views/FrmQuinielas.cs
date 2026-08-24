using System;
using System.Linq;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmQuinielas : Form
    {
        private readonly QuinielaController
            _quinielaController;

        private readonly UsuarioController
            _usuarioController;

        public FrmQuinielas()
        {
            InitializeComponent();

            _quinielaController =
                new QuinielaController();

            _usuarioController =
                new UsuarioController();

            CargarUsuarios();
            CargarQuinielas();
            CargarQuinielasDisponibles();

            grpUnirse.Visible =
                !SesionUsuarioService.EsAdministrador;
        }

        private void CargarUsuarios()
        {
            var usuarios =
                _usuarioController
                    .ObtenerUsuarios()
                    .Where(usuario =>
                        usuario.Rol == RolUsuario.Usuario)
                    .OrderBy(usuario =>
                        usuario.Nombre
                    )
                    .ToList();

            clbUsuarios.DataSource = null;
            clbUsuarios.DataSource = usuarios;
            clbUsuarios.DisplayMember = "Nombre";
            clbUsuarios.ValueMember = "Id";

            cmbUsuarioIntegrante.DataSource = null;
            cmbUsuarioIntegrante.DataSource =
                usuarios.ToList();

            cmbUsuarioIntegrante.DisplayMember =
                "Nombre";

            cmbUsuarioIntegrante.ValueMember =
                "Id";

            cmbUsuarioIntegrante.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }

        private void CargarQuinielas()
        {
            var quinielas =
                _quinielaController
                    .ObtenerQuinielas();

            cmbQuiniela.DataSource = null;
            cmbQuiniela.DataSource = quinielas;
            cmbQuiniela.DisplayMember = "Nombre";
            cmbQuiniela.ValueMember = "Id";
            cmbQuiniela.DropDownStyle =
                ComboBoxStyle.DropDownList;

            dgvIntegrantes.DataSource = null;
            ConfigurarPermisosQuiniela();
        }

        private void cmbQuiniela_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ConfigurarPermisosQuiniela();
        }

        private void ConfigurarPermisosQuiniela()
        {
            bool puedeAdministrar =
                SesionUsuarioService.EsAdministrador ||
                (cmbQuiniela.SelectedItem is Quiniela quiniela &&
                 quiniela.CreadorUsuarioId ==
                 SesionUsuarioService.UsuarioActual.Id);

            cmbUsuarioIntegrante.Enabled = puedeAdministrar;
            btnAgregarIntegrante.Enabled = puedeAdministrar;
            btnQuitarIntegrante.Enabled = puedeAdministrar;

            if (SesionUsuarioService.EsAdministrador)
            {
                grpIntegrantes.Text = "Administrar integrantes";
            }
            else
            {
                grpIntegrantes.Text = puedeAdministrar
                    ? "Mis quinielas privadas (puede administrar esta)"
                    : "Mis quinielas privadas";
            }
        }

        private void CargarQuinielasDisponibles()
        {
            var disponibles = _quinielaController
                .ObtenerQuinielasDisponibles();

            cmbQuinielaDisponible.DataSource = null;
            cmbQuinielaDisponible.DataSource = disponibles;
            cmbQuinielaDisponible.DisplayMember = "Nombre";
            cmbQuinielaDisponible.ValueMember = "QuinielaId";
            cmbQuinielaDisponible.DropDownStyle =
                ComboBoxStyle.DropDownList;

            bool hayDisponibles = disponibles.Count > 0;
            cmbQuinielaDisponible.Enabled = hayDisponibles;
            btnUnirse.Enabled = hayDisponibles;
            lblNombreQuinielaUnirse.Text = hayDisponibles
                ? "Seleccione una quiniela disponible:"
                : "No hay quinielas disponibles para unirse.";
        }

        private void btnCrearQuiniela_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                var integrantesIds =
                    clbUsuarios
                        .CheckedItems
                        .Cast<Usuario>()
                        .Select(usuario =>
                            usuario.Id
                        )
                        .ToList();

                _quinielaController.CrearQuiniela(
                    txtNombre.Text,
                    txtDescripcion.Text,
                    integrantesIds
                );

                MessageBox.Show(
                    "La quiniela fue creada correctamente.",
                    "Quiniela creada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtNombre.Clear();
                txtDescripcion.Clear();

                for (int indice = 0;
                     indice < clbUsuarios.Items.Count;
                     indice++)
                {
                    clbUsuarios.SetItemChecked(
                        indice,
                        false
                    );
                }

                CargarQuinielas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo crear la quiniela",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnVerIntegrantes_Click(
            object sender,
            EventArgs e)
        {
            CargarIntegrantes();
        }

        private void CargarIntegrantes()
        {
            try
            {
                if (cmbQuiniela.SelectedItem
                    is not Quiniela quiniela)
                {
                    MessageBox.Show(
                        "Debe seleccionar una quiniela.",
                        "Dato requerido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                var integrantes =
                    _quinielaController
                        .ObtenerResumenIntegrantes(
                            quiniela.Id
                        );

                dgvIntegrantes.DataSource = null;

                dgvIntegrantes.DataSource = integrantes;

                if (dgvIntegrantes.Columns["PronosticosConGoleadores"]
                    is DataGridViewColumn columnaGoleadores)
                {
                    columnaGoleadores.HeaderText =
                        "Pronósticos con goleadores";
                }

                if (dgvIntegrantes.Columns["PaisPreferido"]
                    is DataGridViewColumn columnaPais)
                {
                    columnaPais.HeaderText = "País preferido";
                }

                if (integrantes.Count == 0)
                {
                    MessageBox.Show(
                        "La quiniela no tiene integrantes.",
                        "Sin integrantes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAgregarIntegrante_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbQuiniela.SelectedItem
                    is not Quiniela quiniela)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una quiniela."
                    );
                }

                if (cmbUsuarioIntegrante.SelectedItem
                    is not Usuario usuario)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un usuario."
                    );
                }

                _quinielaController.AgregarIntegrante(
                    quiniela.Id,
                    usuario.Id
                );

                MessageBox.Show(
                    "El usuario fue agregado a la quiniela.",
                    "Integrante agregado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarQuinielas();

                SeleccionarQuiniela(
                    quiniela.Id
                );

                CargarIntegrantes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo agregar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnQuitarIntegrante_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbQuiniela.SelectedItem
                    is not Quiniela quiniela)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una quiniela."
                    );
                }

                if (cmbUsuarioIntegrante.SelectedItem
                    is not Usuario usuario)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un usuario."
                    );
                }

                _quinielaController.EliminarIntegrante(
                    quiniela.Id,
                    usuario.Id
                );

                MessageBox.Show(
                    "El usuario fue retirado de la quiniela.",
                    "Integrante retirado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarQuinielas();

                SeleccionarQuiniela(
                    quiniela.Id
                );

                CargarIntegrantes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo retirar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void SeleccionarQuiniela(
            int quinielaId)
        {
            for (int indice = 0;
                 indice < cmbQuiniela.Items.Count;
                 indice++)
            {
                if (cmbQuiniela.Items[indice]
                    is Quiniela quiniela &&
                    quiniela.Id == quinielaId)
                {
                    cmbQuiniela.SelectedIndex =
                        indice;

                    break;
                }
            }
        }

        private void btnUnirse_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbQuinielaDisponible.SelectedItem
                    is not QuinielaDisponibleItem quiniela)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una quiniela disponible."
                    );
                }

                _quinielaController.UnirseAQuiniela(
                    quiniela.QuinielaId
                );

                MessageBox.Show(
                    "Ahora pertenece a la quiniela privada.",
                    "Inscripción completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarQuinielas();
                CargarQuinielasDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo completar la inscripción",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}

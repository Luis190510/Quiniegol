using System;
using System.Linq;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Models;

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
        }

        private void CargarUsuarios()
        {
            var usuarios =
                _usuarioController
                    .ObtenerUsuarios()
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
                        .ObtenerIntegrantes(
                            quiniela.Id
                        );

                dgvIntegrantes.DataSource = null;

                dgvIntegrantes.DataSource =
                    integrantes
                        .Select(usuario => new
                        {
                            usuario.Id,
                            usuario.Nombre,
                            usuario.PaisPreferido,
                            usuario.Puntos
                        })
                        .ToList();

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

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Models;

namespace Quiniegol.Views
{
    public partial class FrmPartidos : Form
    {
        private readonly PartidoController _partidoController;
        private readonly SeleccionController _seleccionController;

        public FrmPartidos()
        {
            InitializeComponent();

            _partidoController = new PartidoController();
            _seleccionController = new SeleccionController();

            CargarSelecciones();
            CargarPartidos();
        }

        private void CargarSelecciones()
        {
            var selecciones =
                _seleccionController.ObtenerSelecciones();

            cmbLocal.DataSource = selecciones.ToList();
            cmbLocal.DisplayMember = "Nombre";
            cmbLocal.ValueMember = "Id";

            cmbVisitante.DataSource = selecciones.ToList();
            cmbVisitante.DisplayMember = "Nombre";
            cmbVisitante.ValueMember = "Id";

            cmbLocal.SelectedIndex = -1;
            cmbVisitante.SelectedIndex = -1;
        }

        private void CargarPartidos()
        {
            dgvPartidos.DataSource = null;
            dgvPartidos.DataSource =
                _partidoController.ObtenerPartidos();

            if (dgvPartidos.Columns["FechaHora"] != null)
            {
                dgvPartidos.Columns["FechaHora"]
                    .DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvPartidos.Columns["Anotadores"] != null)
            {
                dgvPartidos.Columns["Anotadores"].Visible = false;
            }
        }

        private void btnRegistrarPartido_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbLocal.SelectedValue == null)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar el equipo local."
                    );
                }

                if (cmbVisitante.SelectedValue == null)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar el equipo visitante."
                    );
                }

                int localId =
                    Convert.ToInt32(cmbLocal.SelectedValue);

                int visitanteId =
                    Convert.ToInt32(cmbVisitante.SelectedValue);

                _partidoController.RegistrarPartido(
                    localId,
                    visitanteId,
                    dtpFechaHora.Value
                );

                MessageBox.Show(
                    "Partido registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                cmbLocal.SelectedIndex = -1;
                cmbVisitante.SelectedIndex = -1;

                CargarPartidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible registrar el partido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnGuardarResultado_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (dgvPartidos.CurrentRow?.DataBoundItem
                    is not Partido partidoSeleccionado)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un partido de la tabla."
                    );
                }

                int golesLocal =
                    Convert.ToInt32(nudGolesLocal.Value);

                int golesVisitante =
                    Convert.ToInt32(nudGolesVisitante.Value);

                _partidoController.GuardarResultado(
                    partidoSeleccionado.Id,
                    golesLocal,
                    golesVisitante
                );

                MessageBox.Show(
                    "Resultado guardado correctamente.",
                    "Resultado actualizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                nudGolesLocal.Value = 0;
                nudGolesVisitante.Value = 0;

                CargarPartidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible guardar el resultado",
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

        private void btnEliminarPartido_Click(
    object sender,
    EventArgs e)
        {
            try
            {
                if (dgvPartidos.CurrentRow?.DataBoundItem
                    is not Partido partidoSeleccionado)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un partido de la tabla."
                    );
                }

                DialogResult confirmacion = MessageBox.Show(
                    "¿Está seguro de que desea eliminar el partido seleccionado?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }

                _partidoController.EliminarPartido(
                    partidoSeleccionado.Id
                );

                MessageBox.Show(
                    "Partido eliminado correctamente.",
                    "Eliminación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarPartidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No fue posible eliminar el partido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void dtpFechaHora_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
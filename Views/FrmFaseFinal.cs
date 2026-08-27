using System;
using System.Windows.Forms;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmFaseFinal : Form
    {
        private readonly CrucesFaseFinalService
            _crucesService;

        public FrmFaseFinal()
        {
            InitializeComponent();

            _crucesService =
                new CrucesFaseFinalService();
        }

        private void btnCalcular_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                dgvClasificados.DataSource = null;

                dgvClasificados.DataSource =
                    _crucesService
                        .ObtenerClasificados();

                dgvCruces.DataSource = null;

                dgvCruces.DataSource =
                    _crucesService
                        .CalcularCruces();

                dgvClasificados
                    .AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode
                        .Fill;

                dgvCruces
                    .AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode
                        .Fill;

                dgvClasificados.ReadOnly =
                    true;

                dgvCruces.ReadOnly =
                    true;

                OcultarIds();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo calcular la fase final",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OcultarIds()
        {
            if (dgvClasificados.Columns["SeleccionId"]
                is DataGridViewColumn columna)
            {
                columna.Visible = false;
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

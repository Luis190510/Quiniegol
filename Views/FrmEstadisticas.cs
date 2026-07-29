using System;
using System.Windows.Forms;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmEstadisticas : Form
    {
        private readonly EstadisticasService
            _estadisticasService;

        public FrmEstadisticas()
        {
            InitializeComponent();

            _estadisticasService =
                new EstadisticasService();

            dtpDesde.Value =
                new DateTime(2026, 6, 1);

            dtpHasta.Value =
                new DateTime(2026, 7, 31);
        }

        private void btnCalcular_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                var estadisticas =
                    _estadisticasService
                        .ObtenerEstadisticas(
                            dtpDesde.Value,
                            dtpHasta.Value
                        );

                dgvEstadisticas.DataSource = null;
                dgvEstadisticas.DataSource =
                    estadisticas;

                dgvEstadisticas.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvEstadisticas.ReadOnly =
                    true;

                dgvEstadisticas
                    .AllowUserToAddRows =
                    false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al calcular estadísticas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
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

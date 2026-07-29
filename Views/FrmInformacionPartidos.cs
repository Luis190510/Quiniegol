using System;
using System.Windows.Forms;
using Quiniegol.Controllers;

namespace Quiniegol.Views
{
    public partial class FrmInformacionPartidos : Form
    {
        private readonly InformacionPartidosController
            _informacionController;

        public FrmInformacionPartidos()
        {
            InitializeComponent();

            _informacionController =
                new InformacionPartidosController();

            CargarInformacion();
        }

        private void CargarInformacion()
        {
            lblFechaSimulada.Text =
                "Fecha simulada: " +
                _informacionController
                    .ObtenerFechaSimulada()
                    .ToString("dd/MM/yyyy HH:mm");

            dgvUltimos.DataSource = null;

            dgvUltimos.DataSource =
                _informacionController
                    .ObtenerUltimosCinco();

            dgvProximos.DataSource = null;

            dgvProximos.DataSource =
                _informacionController
                    .ObtenerProximos24Horas();

            ConfigurarColumnas(
                dgvUltimos
            );

            ConfigurarColumnas(
                dgvProximos
            );
        }

        private void ConfigurarColumnas(
            DataGridView tabla)
        {
            if (tabla.Columns["PartidoId"] != null)
            {
                tabla.Columns["PartidoId"]
                    .Visible = false;
            }

            if (tabla.Columns["FechaHora"] != null)
            {
                tabla.Columns["FechaHora"]
                    .HeaderText = "Fecha y hora";
            }

            if (tabla.Columns["Partido"] != null)
            {
                tabla.Columns["Partido"]
                    .HeaderText = "Partido";
            }

            if (tabla.Columns["Estado"] != null)
            {
                tabla.Columns["Estado"]
                    .HeaderText = "Estado";
            }

            if (tabla.Columns["Marcador"] != null)
            {
                tabla.Columns["Marcador"]
                    .HeaderText = "Marcador";
            }
        }

        private void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CargarInformacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al actualizar",
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

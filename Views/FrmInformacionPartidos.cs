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

        private static void ConfigurarColumnas(
            DataGridView tabla)
        {
            if (tabla.Columns["PartidoId"]
                is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            if (tabla.Columns["FechaHora"]
                is DataGridViewColumn columnaFecha)
            {
                columnaFecha.HeaderText = "Fecha y hora";
            }

            if (tabla.Columns["Partido"]
                is DataGridViewColumn columnaPartido)
            {
                columnaPartido.HeaderText = "Partido";
            }

            if (tabla.Columns["Estado"]
                is DataGridViewColumn columnaEstado)
            {
                columnaEstado.HeaderText = "Estado";
            }

            if (tabla.Columns["Marcador"]
                is DataGridViewColumn columnaMarcador)
            {
                columnaMarcador.HeaderText = "Marcador";
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

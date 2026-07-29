using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmRanking : Form
    {
        private readonly PuntajeController
            _puntajeController;

        private readonly InsigniaService
            _insigniaService;

        public FrmRanking()
        {
            InitializeComponent();

            _puntajeController =
                new PuntajeController();

            _insigniaService =
                new InsigniaService();

            CargarRanking();
        }

        private void CargarRanking()
        {
            try
            {
                _insigniaService
                    .RecalcularInsignias();

                dgvRanking.DataSource = null;

                dgvRanking.DataSource =
                    _puntajeController
                        .ObtenerRanking();

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al cargar el ranking",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvRanking.Columns["Posicion"] != null)
            {
                dgvRanking.Columns["Posicion"]
                    .HeaderText = "Posición";
            }

            if (dgvRanking.Columns["UsuarioId"] != null)
            {
                dgvRanking.Columns["UsuarioId"]
                    .HeaderText = "ID";
            }

            if (dgvRanking.Columns["Nombre"] != null)
            {
                dgvRanking.Columns["Nombre"]
                    .HeaderText = "Usuario";
            }

            if (dgvRanking.Columns["PaisPreferido"] != null)
            {
                dgvRanking.Columns["PaisPreferido"]
                    .HeaderText = "País preferido";
            }

            if (dgvRanking.Columns["Puntos"] != null)
            {
                dgvRanking.Columns["Puntos"]
                    .HeaderText = "Puntos";
            }

            if (dgvRanking.Columns["Insignias"] != null)
            {
                dgvRanking.Columns["Insignias"]
                    .HeaderText = "Insignias";
            }
        }

        private void btnActualizarRanking_Click(
            object sender,
            EventArgs e)
        {
            CargarRanking();

            MessageBox.Show(
                "Los puntajes fueron actualizados correctamente.",
                "Ranking actualizado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}

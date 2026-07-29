using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System;
using System.Windows.Forms;
using Quiniegol.Controllers;

namespace Quiniegol.Views
{
    public partial class FrmRanking : Form
    {
        private readonly PuntajeController
            _puntajeController;

        public FrmRanking()
        {
            InitializeComponent();

            _puntajeController =
                new PuntajeController();

            CargarRanking();
        }

        private void CargarRanking()
        {
            try
            {
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
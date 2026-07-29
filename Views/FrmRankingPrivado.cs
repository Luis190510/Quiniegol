using System;
using System.Windows.Forms;
using Quiniegol.Controllers;
using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmRankingPrivado : Form
    {
        private readonly QuinielaController
            _quinielaController;

        private readonly RankingPrivadoController
            _rankingController;

        private readonly InsigniaService
            _insigniaService;

        public FrmRankingPrivado()
        {
            InitializeComponent();

            _quinielaController =
                new QuinielaController();

            _rankingController =
                new RankingPrivadoController();

            _insigniaService =
                new InsigniaService();

            CargarQuinielas();
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
        }

        private void btnConsultar_Click(
            object sender,
            EventArgs e)
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

                _insigniaService
                    .RecalcularInsignias();

                var ranking =
                    _rankingController
                        .ObtenerRanking(
                            quiniela.Id
                        );

                dgvRanking.DataSource = null;
                dgvRanking.DataSource = ranking;

                ConfigurarColumnas();

                if (ranking.Count == 0)
                {
                    MessageBox.Show(
                        "La quiniela seleccionada no tiene integrantes.",
                        "Ranking vacío",
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

        private void btnCerrar_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}

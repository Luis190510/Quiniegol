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
            CambiarTitulo("Posicion", "Posición");
            CambiarTitulo("UsuarioId", "ID");
            CambiarTitulo("Nombre", "Usuario");
            CambiarTitulo("PaisPreferido", "País preferido");
            CambiarTitulo("Puntos", "Puntos");
            CambiarTitulo("Insignias", "Insignias");
        }

        private void CambiarTitulo(string nombre, string titulo)
        {
            if (dgvRanking.Columns[nombre] is DataGridViewColumn columna)
            {
                columna.HeaderText = titulo;
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

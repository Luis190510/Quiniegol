using System;
using System.Windows.Forms;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    public partial class FrmTablaGrupos : Form
    {
        private readonly TablaPosicionesService
            _tablaService;

        public FrmTablaGrupos()
        {
            InitializeComponent();

            _tablaService =
                new TablaPosicionesService();

            CargarGrupos();
        }

        private void CargarGrupos()
        {
            cmbGrupo.DataSource = null;

            cmbGrupo.DataSource =
                _tablaService.ObtenerGrupos();

            cmbGrupo.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }

        private void btnCalcular_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string grupo =
                    cmbGrupo.SelectedItem
                        ?.ToString()
                    ?? "";

                dgvTabla.DataSource = null;

                dgvTabla.DataSource =
                    _tablaService
                        .CalcularTabla(
                            grupo
                        );

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al calcular la tabla",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvTabla.Columns["SeleccionId"] != null)
            {
                dgvTabla.Columns["SeleccionId"]
                    .Visible = false;
            }

            CambiarTitulo(
                "Posicion",
                "Pos."
            );

            CambiarTitulo(
                "Seleccion",
                "Selección"
            );

            CambiarTitulo(
                "PartidosJugados",
                "PJ"
            );

            CambiarTitulo(
                "PartidosGanados",
                "PG"
            );

            CambiarTitulo(
                "PartidosEmpatados",
                "PE"
            );

            CambiarTitulo(
                "PartidosPerdidos",
                "PP"
            );

            CambiarTitulo(
                "GolesFavor",
                "GF"
            );

            CambiarTitulo(
                "GolesContra",
                "GC"
            );

            CambiarTitulo(
                "DiferenciaGoles",
                "DG"
            );

            CambiarTitulo(
                "Puntos",
                "PTS"
            );
        }

        private void CambiarTitulo(
            string nombreColumna,
            string titulo)
        {
            if (dgvTabla.Columns[nombreColumna] != null)
            {
                dgvTabla.Columns[nombreColumna]
                    .HeaderText = titulo;
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

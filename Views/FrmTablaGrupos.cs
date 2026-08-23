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
            if (dgvTabla.Columns["SeleccionId"]
                is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            CambiarTitulo(
                "Posicion",
                "Posición"
            );

            CambiarTitulo(
                "Seleccion",
                "Selección"
            );

            CambiarTitulo(
                "PartidosJugados",
                "Partidos jugados"
            );

            CambiarTitulo(
                "PartidosGanados",
                "Partidos ganados"
            );

            CambiarTitulo(
                "PartidosEmpatados",
                "Partidos empatados"
            );

            CambiarTitulo(
                "PartidosPerdidos",
                "Partidos perdidos"
            );

            CambiarTitulo(
                "GolesFavor",
                "Goles a favor"
            );

            CambiarTitulo(
                "GolesContra",
                "Goles en contra"
            );

            CambiarTitulo(
                "DiferenciaGoles",
                "Diferencia de goles"
            );

            CambiarTitulo(
                "Puntos",
                "Puntos"
            );

            dgvTabla.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;
            dgvTabla.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void CambiarTitulo(
            string nombreColumna,
            string titulo)
        {
            if (dgvTabla.Columns[nombreColumna]
                is DataGridViewColumn columna)
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

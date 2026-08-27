using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>
    /// Muestra la tabla de posiciones de cada grupo con encabezados descriptivos.
    /// </summary>
    public partial class FrmTablaGrupos : Form
    {
        private static readonly Dictionary<string, string> TitulosColumnas = new()
        {
            [nameof(PosicionGrupoItem.Posicion)] = "Posición",
            [nameof(PosicionGrupoItem.Seleccion)] = "Selección",
            [nameof(PosicionGrupoItem.PartidosJugados)] = "Partidos jugados",
            [nameof(PosicionGrupoItem.PartidosGanados)] = "Partidos ganados",
            [nameof(PosicionGrupoItem.PartidosEmpatados)] = "Partidos empatados",
            [nameof(PosicionGrupoItem.PartidosPerdidos)] = "Partidos perdidos",
            [nameof(PosicionGrupoItem.GolesFavor)] = "Goles a favor",
            [nameof(PosicionGrupoItem.GolesContra)] = "Goles en contra",
            [nameof(PosicionGrupoItem.DiferenciaGoles)] = "Diferencia de goles",
            [nameof(PosicionGrupoItem.Puntos)] = "Puntos"
        };

        private readonly TablaPosicionesService _tablaService;

        public FrmTablaGrupos()
        {
            InitializeComponent();
            _tablaService = new TablaPosicionesService();
            CargarGrupos();
        }

        private void CargarGrupos()
        {
            cmbGrupo.DataSource = _tablaService.ObtenerGrupos();
            cmbGrupo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                string grupo = cmbGrupo.SelectedItem?.ToString() ?? string.Empty;
                dgvTabla.DataSource = _tablaService.CalcularTabla(grupo);
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al calcular la tabla",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvTabla.Columns[nameof(PosicionGrupoItem.SeleccionId)]
                is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            foreach ((string propiedad, string titulo) in TitulosColumnas)
            {
                if (dgvTabla.Columns[propiedad] is DataGridViewColumn columna)
                {
                    columna.HeaderText = titulo;
                }
            }

            dgvTabla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTabla.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

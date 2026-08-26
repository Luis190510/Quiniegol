using Quiniegol.Models;
using Quiniegol.Services;

namespace Quiniegol.Views
{
    /// <summary>
    /// Presenta los reportes permitidos para el rol autenticado.
    /// </summary>
    public partial class FrmEstadisticas : Form
    {
        private readonly EstadisticasService _estadisticasService;
        private List<EstadisticaItem> _reporteActual = new();

        public FrmEstadisticas()
        {
            InitializeComponent();
            _estadisticasService = new EstadisticasService();
            dtpDesde.Value = new DateTime(
                2026, 6, 1, 0, 0, 0, DateTimeKind.Unspecified);
            dtpHasta.Value = new DateTime(
                2026, 7, 31, 0, 0, 0, DateTimeKind.Unspecified);
            ConfigurarRol();
            ConfigurarTabla();
        }

        private void ConfigurarRol()
        {
            if (SesionUsuarioService.EsAdministrador)
            {
                lblTitulo.Text = "Reportes del administrador";
                lblDescripcion.Text =
                    "Resultados, aciertos, participación y promedio de goles del rango.";
                return;
            }

            Usuario usuario = SesionUsuarioService.UsuarioActual;
            lblTitulo.Text = "Reportes del usuario";
            lblDescripcion.Text =
                $"Usuario: {usuario.Nombre}. Incluye su probabilidad histórica de acierto.";
        }

        private void ConfigurarTabla()
        {
            dgvEstadisticas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstadisticas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvEstadisticas.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvEstadisticas.ReadOnly = true;
            dgvEstadisticas.AllowUserToAddRows = false;
            dgvEstadisticas.AllowUserToDeleteRows = false;
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                _reporteActual = _estadisticasService.ObtenerReportePorRol(
                    dtpDesde.Value,
                    dtpHasta.Value);
                dgvEstadisticas.DataSource = _reporteActual;
                ConfigurarColumnas();
                ActualizarDescargas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al generar el reporte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDescargarCsv_Click(object sender, EventArgs e)
        {
            DescargarReporte("csv");
        }

        private void btnDescargarTxt_Click(object sender, EventArgs e)
        {
            DescargarReporte("txt");
        }

        private void btnDescargarPdf_Click(object sender, EventArgs e)
        {
            DescargarReporte("pdf");
        }

        private void DescargarReporte(string formato)
        {
            if (_reporteActual.Count == 0)
            {
                MessageBox.Show(
                    "Primero debe generar un reporte.",
                    "Reporte no generado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            formato = formato.ToLowerInvariant();
            string filtro;
            if (formato == "csv")
            {
                filtro = "Archivo CSV (*.csv)|*.csv";
            }
            else if (formato == "pdf")
            {
                filtro = "Archivo PDF (*.pdf)|*.pdf";
            }
            else
            {
                filtro = "Archivo de texto (*.txt)|*.txt";
            }

            string rol = SesionUsuarioService.EsAdministrador
                ? "administrador"
                : "usuario";
            using SaveFileDialog dialogo = new()
            {
                AddExtension = true,
                DefaultExt = formato,
                FileName = $"reporte_{rol}_{DateTime.Now:yyyyMMdd}",
                Filter = filtro,
                OverwritePrompt = true,
                Title = $"Guardar reporte {formato.ToUpperInvariant()}"
            };

            if (dialogo.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                if (formato == "csv")
                {
                    ReporteDescargaService.GuardarCsv(dialogo.FileName, _reporteActual);
                }
                else if (formato == "pdf")
                {
                    ReporteDescargaService.GuardarPdf(dialogo.FileName, _reporteActual);
                }
                else
                {
                    ReporteDescargaService.GuardarTxt(dialogo.FileName, _reporteActual);
                }

                MessageBox.Show(
                    $"El reporte fue guardado correctamente en:\n{dialogo.FileName}",
                    "Descarga completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo guardar el reporte",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            _reporteActual = new List<EstadisticaItem>();
            dgvEstadisticas.DataSource = null;
            ActualizarDescargas();
        }

        private void ActualizarDescargas()
        {
            bool hayReporte = _reporteActual.Count > 0;
            btnDescargarCsv.Enabled = hayReporte;
            btnDescargarTxt.Enabled = hayReporte;
            btnDescargarPdf.Enabled = hayReporte;
        }

        private void ConfigurarColumnas()
        {
            if (dgvEstadisticas.Columns[nameof(EstadisticaItem.Estadistica)]
                is DataGridViewColumn columnaReporte)
            {
                columnaReporte.HeaderText = "Reporte";
                columnaReporte.FillWeight = 42;
            }

            if (dgvEstadisticas.Columns[nameof(EstadisticaItem.Resultado)]
                is DataGridViewColumn columnaResultado)
            {
                columnaResultado.HeaderText = "Resultado";
                columnaResultado.FillWeight = 58;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
